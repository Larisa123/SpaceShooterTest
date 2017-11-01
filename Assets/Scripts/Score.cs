using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Score : MonoBehaviour {
	private int score;
	private int level;
	private int[] lvlUpgPoints = {5, 20, 50, 100, 200};
	public Transform[] UIImagePositions;
	public GameObject[] digitSprites;
	//private Image[] UIImages;
	public GameController gameController;


	void Start() {
		gameController = GetComponent<GameController> ();
		resetScoringSystem (); // initializes the score and level
		//createUIImages();
	}

	public void resetScoringSystem() {
		resetScore ();
		resetLevel ();
	}

	public int getScore () { return score; }
	public void increaseScore() { this.score++; updateUI (); checkForUpgrades ();}
	public void reduceScore() { this.score--; updateUI (); checkForUpgrades ();}
	void resetScore() { this.score = 0; updateUI ();}

	public int getLevel() { return level; }
	void resetLevel() { this.level = 1;}


	void checkForUpgrades() {
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
	}

	// Game:

	public bool checkIfGameOver() {
		if (getScore() < 0) {
			return true;
		} else return false;
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
			int index;
			GameObject sprite;

			for (int i = 0; i < 3; i++) {
				index = int.Parse (UIScore [i].ToString ());
				Debug.Log (string.Format("Score: {0}, sprite index: {1}, image name: {2}", UIScore, index, UIImagePositions[i].name));
				sprite = digitSprites [index];
				sprite.transform.position = UIImagePositions[i].position;
				sprite.transform.SetAsFirstSibling ();
			}
		}
	}
}
