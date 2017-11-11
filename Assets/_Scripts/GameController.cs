using UnityEngine;
using System.Collections;
using System.Collections.Generic;


enum TutorialState {Start, HowToMove, HowToShoot, Finished}

public class GameController : MonoBehaviour {

	// ASTEROIDS:
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public Vector3 asteroidSpawnMax;
	public Vector3 asteroidSpawnMin;
	private static int asteroidCount = 0;
	public int asteroidSpawnCount;
	public int asteroidMaxOnScreen;
	public GameObject asteroid;
	private static List<GameObject> asteroidsOnScreen;

	// ENEMIES - DEMONS:
	public GameObject demon;
	private static int demonCount = 0;
	public int howManyHasHeKilled = 0;
	private int[] maxDemonsOnScreen; // depends on the level
	private static List<GameObject> demonsOnScreen;

	//PickUps
	public GameObject shieldPickUp;
	private static int shieldPickUpCount = 0;
	private static List<GameObject> shieldpickupsOnScreen;
	private int[] maxShieldPickUpsOnScreen = {1, 2, 3, 4, 5, 7}; // depends on the level

	//Player:
	public GameObject player;
	public PlayerController playerController;

	// Score:
	public Score scoringSystem;

	// Canvas, Cameras:
	public GameObject welcomeScreen;

	public Camera mainCamera;
	private Shake shakeScript;
	[SerializeField] private GameObject secondCameraScreen;


	void Start () {
		instantiateOnStart ();
	}

	void instantiateOnStart() {
		showWelcomeScreen ();
		instantiateLists ();
		resetMaxDemonsOnScreenList ();
		playerController = player.GetComponent<PlayerController> ();
		shakeScript = mainCamera.GetComponent<Shake> ();
		//TODO: create game over screen and play again button
	}

	void instantiateLists() {
		demonsOnScreen = new List<GameObject> ();
		asteroidsOnScreen = new List<GameObject> ();
		shieldpickupsOnScreen = new List<GameObject> ();
	}

	// Clicks:

	public void OnPlayButtonClick() { // gets called automatically when the user clicks the Play button on Welcome screen
		startNewGame(); // resets the score, game state and stuff like that
	}


	// Game:

	public void endTheGame() {
		scoringSystem.gameState = GameState.GameOverScreen;
		hidePlayer ();
		stopCoroutines ();

		// empty lists:
		foreach(string name in new string[3] {"Demon", "Asteroid", "ShieldPickUp"}) {
			emptyList(name);
		}
	}

	public void startNewGame() {
		hideWelcomeScreen ();
		showPlayer ();
		scoringSystem.resetScoringSystem ();
		resetMaxDemonsOnScreenList ();

		showSecondCameraScreen ();
		resetCounters ();
		StartCoroutine (SpawnAsteroidsAndDemons ());

		// call other functions
	}

	public void gameOver() {
		endTheGame ();
		//scoringSystem.resetScoringSystem ();
		// show game over screen
	}

	void stopCoroutines() {
		//try { StopCoroutine (SpawnAsteroidsAndDemons()); } catch {}
		//try { StopCoroutine (SpawnShieldPickUps()); } catch {}
		try { stopCoroutines(); } catch {Debug.Log("Coroutines werent properly stopped");}
	}
		
	// Welcome, game over screen:

	void showWelcomeScreen() { welcomeScreen.SetActive (true); }
	void hideWelcomeScreen() { welcomeScreen.SetActive (false); }

	void showPlayer() { player.SetActive (true); }
	void hidePlayer() { player.SetActive (false); }

	// Camera:

	public void shakeCamera() {
		StartCoroutine(shakeScript.CameraShake ());
	}

	void showSecondCameraScreen () {
		secondCameraScreen.SetActive (true);
	}

	// Asteroids:

	public IEnumerator SpawnAsteroidsAndDemons () {
		//yield return new WaitForSeconds (startWait);
		while (scoringSystem.gameState == GameState.Playing) {
			
			for (int i = 0; i < asteroidSpawnCount; i++) {
				instantiateAsteroidAndDemon ();
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}
		
	public void instantiateAsteroidAndDemon() {
		Vector3 spawnPosition = randomPositionInBoundary (asteroidSpawnMin, asteroidSpawnMax);
		Quaternion spawnRotation = Quaternion.identity;

		if (shouldCreateNew ("Asteroid")) {
			instantiate("Asteroid", spawnPosition, spawnRotation);
		}
		if (shouldCreateNew("Demon"))
			instantiate ("Demon", spawnPosition, spawnRotation);
	}


	// Demons:

	public void makeDemonsAngrier() {
		foreach (GameObject demon in demonsOnScreen) {
			Demon demonScript = demon.GetComponent<Demon> ();
			demonScript.increaseShootingRate ();
		}
		// increase the number of demons that should come out of rocks:
		for (int i = 0; i < maxDemonsOnScreen.Length; i++) {
			maxDemonsOnScreen [i]++;
		}
	}

	void resetMaxDemonsOnScreenList() {
		maxDemonsOnScreen = new int[6] {3, 4, 7, 10, 15, 20};
	}

	// Pick Ups:

	public IEnumerator SpawnShieldPickUps () {
		if (!shouldCreateNew("ShieldPickUp"))
			yield break;

		yield return new WaitForSeconds (5.0f * scoringSystem.getLevel());
		for (int i = 0; i < getMaxNumberOnScreen("ShieldPickUp"); i++) {
			instantiateShieldPickUp ();
			yield return new WaitForSeconds (spawnWait * 100.0f * scoringSystem.getLevel());
		}
	}

	public void instantiateShieldPickUp() {
		Vector3 spawnPosition = new Vector3 (
			Random.Range (-5.0f, 5.0f), 
			Random.Range (-3.0f, 3.0f), 
			Random.Range (5.0f, 10.0f)
		);

		Quaternion spawnRotation = Quaternion.identity;

		instantiate ("ShieldPickUp", spawnPosition, spawnRotation);
	}
		

	// Generic:

	void instantiate(string name, Vector3 spawnPosition, Quaternion spawnRotation) {
		GameObject objectToInstantiate = (name == "Asteroid") ? asteroid : (name == "Demon") ? demon : shieldPickUp;
		GameObject objectInstance = Instantiate (objectToInstantiate, spawnPosition, spawnRotation) as GameObject;

		addToList (name, objectInstance); // addToList also increases the appropriate counter
	}

	public void removeFromList(string name, GameObject objectInstance) {
		getCorrectList(name).Remove(objectInstance);
		reduceCounterOf (name);
	}

	void addToList (string name, GameObject objectInstance) {
		getCorrectList (name).Add (objectInstance);
		increaseCounterOf (name);
	}

	public void emptyList(string name) {
		List<GameObject> list = getCorrectList (name);

		/*
		List<GameObject> tempList = new List<GameObject> ();
		foreach (GameObject objectInstance in list) {
			tempList.Add (objectInstance);
		}
		list.Clear ();
		foreach (GameObject objectInstance in tempList) {
			Destroy (objectInstance);
		}
		*/

	}

	static List<GameObject> getCorrectList(string name) {
		return (name == "Asteroid") ? asteroidsOnScreen : (name == "Demon") ? demonsOnScreen : shieldpickupsOnScreen;
	}

	public void increaseCounterOf(string objectsTag) {
		if (objectsTag == "Asteroid")
			asteroidCount++;

		else if (objectsTag == "Demon")
			demonCount++;

		else if (objectsTag == "ShieldPickUp")
			shieldPickUpCount++;
	}

	public void reduceCounterOf(string objectsTag) {
		if (objectsTag == "Asteroid") {
			if (asteroidCount > 0) 
				asteroidCount--;

		} else if (objectsTag == "Demon") {
			if (demonCount > 0) 
				demonCount--;

		} else if (objectsTag == "ShieldPickUp") {
			if (shieldPickUpCount > 0) 
				shieldPickUpCount--;
		}
	}

	public void resetCounters() {
		asteroidCount = 0;
		demonCount = 0;
		shieldPickUpCount = 0;
		howManyHasHeKilled = 0;
	}

	int getMaxNumberOnScreen(string objectsName) {
		switch (objectsName) {
		case "Asteroid":
			return asteroidMaxOnScreen;
		case "Demon":
			return maxDemonsOnScreen [scoringSystem.getLevel () - 1];
		case "ShieldPickUp":
			return maxShieldPickUpsOnScreen [scoringSystem.getLevel () - 1];
		default:
			return 0; // shoudn't even come to this, but if it does, it ensures the game doesnt crash
		} 
	}

	bool shouldCreateNew(string objectsName) {
		switch (objectsName) {
		case "Asteroid":
			return asteroidCount < getMaxNumberOnScreen("Asteroid");
		case "Demon":
			if (demonCount < getMaxNumberOnScreen("Demon")) {
				if (Random.Range (1, 10) % 2 == 0)  // just a random condition with probabilty of being true equal to 1/2
					return true;
			} // else:
			return false;
		case "ShieldPickUp":
			return shieldPickUpCount < getMaxNumberOnScreen("ShieldPickUp");
		default:
			return false; // shoudn't even come to this, but if it does, it ensures the game doesnt crash
		} 
	}

	public Vector3 randomPositionInBoundary(Vector3 min, Vector3 max) {
		return new Vector3 (
			Random.Range (min.x, max.x), 
			Random.Range (min.y, max.y), 
			Random.Range (min.z, max.z)
		);
	}
}

public struct GameOverText:string {
	public const string KilledByDemonsKilledToManyDemons = "I told you, killing them is not the way to go, " +
		"try to make them accept you!\n\n" +
		"I will give you one more chance:";
	public const string KilledByDemons = "Great job on trying to avoid killing the demons! " +
		"They seem to have not accepted you yet..\n\n" +
		"Try again:";
	public const string KilledByAsteroid = "Nice job on trying to avoid killing the demons!\n\n " +
		"Try again and also be more careful of rocks:";
	public const string KilledByAsteroidKilledToManyDemons = "You got killed by an asteroid this time but you had that coming. " +
		"You killed way to many of them, they turn very hostile " +
		"when their family members are dying so be careful!\n\n" +
		"I will give you one more chance:";

	public string getAppropriateText(bool killedToManyDemons, bool killedByDemons) {
		if (killedToManyDemons) {
			if (killedByDemons) return GameOverText.KilledByDemonsKilledToManyDemons;
			else return GameOverText.KilledByAsteroidKilledToManyDemons
		}
	}
}
