using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum GameState {
	WelcomeScreen,
	Start,
	Playing,
	GameOverScreen,
}


public class Score : MonoBehaviour {
	private int score;
	private int level;
	private float playersHealth; // from 0 to 1
	private int[] lvlUpgPoints = {5, 20, 50, 100, 200};

	public Transform[] UIImagePositions;
	public GameObject[] digitSprites1;
	public GameObject[] digitSprites2;
	public GameObject[] digitSprites3;
	private GameObject[,] digitSprites;

	private GameController gameController;
	public GameState gameState;


	void Start() {
		gameController = GetComponent<GameController> ();
		createDigitSpritesTable();
		resetScoringSystem (); // initializes the score and level
	}

	public void resetScoringSystem() {
		resetScore ();
		resetLevel ();
		resetPlayersHealth ();
		//resetGameState ();
		gameController.player.resetBulletType ();
	}

	public int getScore () { return score; }
	public void increaseScore() { this.score++; updateUI (); checkForUpgrades ();}
	public void reduceScore() { this.score--; updateUI (); checkForUpgrades ();}
	void resetScore() { this.score = 0; updateUI ();}

	public int getLevel() { return level; }
	void resetLevel() { this.level = 1;}

	public void resetGameState() {
		gameState = GameState.Playing;
	}

	void checkForUpgrades() {
		int previousLevel = getLevel();
		int scr = getScore ();
		if (scr > 0 && scr <= lvlUpgPoints [0]) {
			level = 1;
		} else if (scr > lvlUpgPoints [0] && scr <= lvlUpgPoints [1]) {
			level = 2;
		} else if (scr > lvlUpgPoints [1] && scr <= lvlUpgPoints [2]) {
			level = 3;
		} else if (scr > lvlUpgPoints [2] && scr <= lvlUpgPoints [3]) {
			level = 4;
		} else if (scr > lvlUpgPoints [3] && scr <= lvlUpgPoints [4]) {
			level = 5;
		} else {
			level = 6;
		}
		if (getLevel () > previousLevel)
			levelUpgraded ();
	}

	void levelUpgraded() {
		// TO DO: show something on screen (as congratulations)
		gameController.player.increaseBulletType ();
	}

	void OnUpdate() {
		updateUI ();
	}

	// Player:

	public void reducePlayersHealth() {
		if (playersHealth > 0.2f)
			playersHealth -= 0.2f;
		Debug.Log ("Players health decreased");
	}

	public void increasePlayersHealth() {
		if (playersHealth < 0.8f)
			playersHealth += 0.2f;
		Debug.Log ("Players health decreased");
	}

	public void resetPlayersHealth() {
		playersHealth = 1.0f;
	}

	// Game:

	public bool checkIfGameOver() {
		if (getScore() < 0) {
			return true;
		} else return false;
	}

	private void createDigitSpritesTable() {
		digitSprites = new GameObject[3, 10];
		for (int j=0; j < 10; j++) {
			digitSprites [0, j] = digitSprites1[j];
			digitSprites[1, j] = digitSprites2[j];
			digitSprites[2, j] = digitSprites3[j];
		}
	}


	// UI:
	/*
	private void createUIImages() {
		UIImages = new Image[10]; // 9 digits + 0

		for (int i = 0; i < 10; i++) {
			//UIImages [i] = Image (i.ToString);
		}
	}
	*/
		
	private string getUIString() {
		int scr = getScore ();

		if (scr < 10)
			return string.Format ("00{0}", scr);
		if (scr >= 10 && scr < 100) 
			return string.Format ("0{0}", scr);
		if (scr >= 100 && scr < 1000)
			return scr.ToString ();
		return string.Format ("999"); // tu se bo igra zaekrat ustavila
	}

	private void updateUI() {
		if (getScore () < 0) {
			gameController.gameOver ();
		} else {
			string UIScore = getUIString ();
			char[] digitCharArray;
			int digit;
			GameObject sprite;

			for (int i = 0; i < 3; i++) {
				digitCharArray = UIScore.ToCharArray();
				digit = int.Parse (digitCharArray[i].ToString());
				// deactivate all others digits and activate current digit
				for (int j = 0; j < 10; j++) {
					sprite = digitSprites [i, digit];
					Debug.Log ("indeks: "+i+" "+digit + " " + sprite.name);
					if (j == digit)
						sprite.SetActive (true);
					else
						sprite.SetActive (false);
				}
			}
		}
	}
}
