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
	[SerializeField] private GameObject healthBarObject;
	[SerializeField] private Image healthBar;

	private int[] lvlUpgPoints = {5, 20, 50, 100, 200};

	//public Transform[] UIImagePositions;
	//public GameObject[] digitSprites1;
	//public GameObject[] digitSprites2;
	//public GameObject[] digitSprites3;
	//private GameObject[,] digitSprites;

	[SerializeField] private GameObject scoreTextObject;
	[SerializeField] private Text scoreText;

	[SerializeField] private GameController gameController;
	public GameState gameState;


	void Start() {
		//gameController = GetComponent<GameController> ();
		resetScoringSystem (); // initializes the score and level
	}

	public void resetScoringSystem() {
		resetScore ();
		resetLevel ();
		resetPlayersHealth ();
		resetGameState ();
		gameController.player.resetBulletType ();
		StartCoroutine (gameController.SpawnAsteroids ());
	}

	public int getScore () { return score; }
	public void increaseScore() { this.score++; updateUI (); checkForUpgrades ();}
	public void reduceScore() { 
		if (getScore () <= 0) 
			return;
		this.score--; 
		updateUI (); 
		checkForUpgrades ();
	}
	
	void resetScore() { 
		this.score = 0; 
		scoreTextObject.SetActive (true);
		updateUI ();}

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
		StartCoroutine (gameController.SpawnShieldPickUps ());
	}

	void OnUpdate() {
		updateUI ();
	}

	// Player:

	public void reducePlayersHealth() {
		if (playersHealth > 0.2f) {
			playersHealth -= 0.2f;
			updateHealthBar ();
		} else if (playersHealth == 0.2f)
			gameController.gameOver ();
	}

	public void increasePlayersHealth() {
		if (playersHealth < 0.8f) {
			playersHealth += 0.2f;
			updateHealthBar ();
		}
	}

	public void resetPlayersHealth() {
		playersHealth = 1.0f;
		healthBarObject.SetActive (true);
		healthBar.color = Color.green;
		updateHealthBar ();
	}

	public void updateHealthBar() {
		if (playersHealth < 0.3f)
			healthBar.color = Color.red;
		else if (playersHealth > 0.3f && playersHealth < 0.7f)
			healthBar.color = Color.yellow;
		else if (playersHealth > 0.7f)
			healthBar.color = Color.green;
		
		healthBar.fillAmount = playersHealth;
	}

	// Game:

	public bool checkIfGameOver() {
		if (getScore() < 0) {
			return true;
		} else return false;
	}
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
		scoreText.text = getUIString ();
	}
}
