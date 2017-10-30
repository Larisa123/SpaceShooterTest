using UnityEngine;
using System.Collections;



public class Score : MonoBehaviour {
	private int score;
	private int level;
	private int[] lvlUpgPoints = {5, 20, 50, 100, 200};

	void Start() {
		resetScoringSystem (); // initializes the score and level
	}

	public void resetScoringSystem() {
		resetScore ();
		resetLevel ();
	}

	public int getScore () { return score; }
	public void increaseScore() { this.score++; Debug.Log(getScore());}
	public void reduceScore() { this.score--; Debug.Log(getScore());}
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
}
