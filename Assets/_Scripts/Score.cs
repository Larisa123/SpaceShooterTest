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
	private static int score;
	[SerializeField] private GameObject scoreTextObject;
	[SerializeField] private Text scoreText;

	private static float playersHealth; // from 0 to 1
	[SerializeField] private GameObject healthBarObject;
	[SerializeField] private Image healthBar;

	[SerializeField] private GameObject levelUpAnimation;
	[SerializeField] private float animationDuration;

	[SerializeField] private GameObject plusChange;
	[SerializeField] private GameObject minusChange;


	private static int level;
	private int[] lvlUpgPoints = {5, 20, 50, 100, 200};

	[SerializeField] private GameController gameController;
	public GameState gameState;


	public void resetScoringSystem() {
		resetScore ();
		resetLevel ();
		resetPlayersHealth ();
		resetGameState ();
		gameController.playerController.resetBulletType ();
	}

	public int getScore () { return score; }

	public void increaseScore(Vector3 fromPosition) {
		runShowScoreChangeAnimation (plus: true, atPosition: fromPosition);
		score++; 
		updateUI (); 
		checkForUpgrades ();
	}

	public void reduceScore(Vector3 fromPosition) { 
		if (getScore () <= 0) 
			return;
		runShowScoreChangeAnimation (plus: false, atPosition: fromPosition);
		score--; 
		updateUI (); 
		checkForUpgrades ();
	}
	
	void resetScore() { 
		score = 0; 
		scoreTextObject.SetActive (true);
		updateUI ();
	}

	public int getLevel() { return level; }
	void resetLevel() { level = 1;}

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
		StartCoroutine(runLevelUpAnimation ());
		gameController.playerController.increaseBulletType ();
		StartCoroutine (gameController.SpawnShieldPickUps ());
	}

	void OnUpdate() {
		updateUI ();
	}

	IEnumerator runLevelUpAnimation() {
		showLevelUpAnimation (true);
		yield return new WaitForSeconds(animationDuration);
		showLevelUpAnimation (false);
	}

	void showLevelUpAnimation(bool value) {
		levelUpAnimation.SetActive (value);
	}

	public IEnumerator runShowScoreChangeAnimation(bool plus, Vector3 atPosition) {
		showScoreChange (plus, true, atPosition);
		yield return new WaitForSeconds(animationDuration);
		showScoreChange (plus, false, atPosition);
	}

	void showScoreChange(bool plus, bool value, Vector3 atPosition) {
		if (plus) {
			plusChange.transform.position = atPosition;
			plusChange.SetActive (value);
		} else {
			minusChange.SetActive (value);
			minusChange.transform.position = atPosition;
		}
	}

	// Player:

	public void reducePlayersHealth() {
		if (playersHealth > 0.2f) {
			playersHealth -= 0.2f;
			updateHealthBar ();
		} else if (playersHealth <= 0.2f) {
			playersHealth = 0.0f;
			gameController.killedByDemons = true;
			gameController.gameOver ();
		}
	}

	public void increasePlayersHealth() {
		if (playersHealth < 0.8f) {
			playersHealth += 0.2f;
			updateHealthBar ();
		}
	}

	public void resetPlayersHealth() {
		playersHealth = 1.0f;
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

	public void showPlayCanvasComponents(bool value) {
		healthBarObject.SetActive (value);
		scoreTextObject.SetActive (value);
	}
}


public struct GameOverText {
	public const string KilledByDemonsKilledToManyDemons = "I told you, killing them is not the way to go, " +
		"try to make them accept you!\n\n" +
		"I will give you one more chance:";
	public const string KilledByDemons = "Great job on trying to avoid killing the demons! " +
		"They seem to have not accepted you yet..\n\n" +
		"Try again:";
	public const string KilledByAsteroid = "Nice job on trying to avoid killing the demons!\n\n " +
		"Try again and also be more careful of rocks:";
	public const string KilledByAsteroidKilledToManyDemons = "You got killed by an asteroid this time but you had that coming. " +
		"You killed way too many of them, they turn very hostile " +
		"when their family members are dying so be careful!\n\n" +
		"I will give you one more chance:";
}