using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class Score : MonoBehaviour {
	private int score;
	private int level;
	private int[] lvlUpgPoints = {5, 20, 50, 100, 200};
	public RectTransform UIImage1Pos;
	public RectTransform UIImage2Pos;
	public RectTransform UIImage3Pos;
	private Image[] UIImages;

	void Start() {
		resetScoringSystem (); // initializes the score and level
		createUIImages();
	}

	public void resetScoringSystem() {
		resetScore ();
		resetLevel ();
	}

	public int getScore () { return score; }
	public void increaseScore() { this.score++; }
	public void reduceScore() { this.score--; }
	void resetScore() { this.score = 0; }

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
		if (score < 0) {
			return true;
		} else return false;
	}


	// UI:

	private void createUIImages() {
		UIImages = new Image[10]; // 9 digits + 0

		for (int i = 0; i < 10; i++) {
			//UIImages [i] = Image (i.ToString);
		}
	}
		
	private string UI() {
		if (score < 10)
			return string.Format ("00{0}", score);
		if (score >= 10 && score < 100) 
			return string.Format ("0{0}", score);
		if (score >= 100 && score < 1000)
			return score.ToString ();
		return string.Format ("999"); // tu se bo igra zaekrat ustavila
	}

	public void updateUI() {
		string UIScore = UI ();
		/*
		string digit1 = UIScore [0].ToString();
		string digit2 = UIScore [1].ToString();
		string digit3 = UIScore [2].ToString();

		UIImage1 = */
	}
}
