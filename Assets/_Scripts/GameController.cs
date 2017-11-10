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

	public Camera mainCamera;
	private Shake shakeScript;

	// ENEMIES - DEMONS:
	public GameObject demon;
	private static int demonCount = 0;
	private int[] maxDemonsOnScreen = {3, 4, 7, 10, 15, 20}; // depends on the level
	private int[] numberOfShieldPickUps = {1, 2, 3, 4, 5, 7};
	private List<GameObject> demonsOnScreen;
	private List<GameObject> asteroidsOnScreen;

	//PickUps
	public GameObject shieldPickUp;
	private static int shieldPickUpCount = 0;
	private List<GameObject> shieldpickupsOnScreen;

	//Player:
	public PlayerController player;

	// Score:
	public Score scoringSystem;

	// Canvas:
	public GameObject welcomeScreen;

	[SerializeField] private GameObject secondCameraScreen;

	void Start () {
		showWelcomeScreen ();
		instantiateLists ();
		shakeScript = mainCamera.GetComponent<Shake> ();
		//TODO: add a reference and a method for the play button
		//TODO: create game over screen and play again button 

	}

	void instantiateLists() {
		demonsOnScreen = new List<GameObject> ();
		asteroidsOnScreen = new List<GameObject> ();
		shieldpickupsOnScreen = new List<GameObject> ();
	}

	// Clicks:

	public void OnPlayButtonClick() { // gets called when the user clickes the Play button on Welcome screen
		startNewGame(); // resets the score, game state and stuff like that
	}


	// Game:

	public void endTheGame() {
		stopCoroutines ();
		emptyArray ("Demon");
		//TODO: empty asteroid array - create asteroid array
	}

	public void startNewGame() {
		hideWelcomeScreen ();
		scoringSystem.resetScoringSystem ();

		showSecondCameraScreen ();
		resetCounters ();
		// call other functions
	}

	public void gameOver() {
		endTheGame ();
		scoringSystem.resetScoringSystem ();
		// show game over screen
	}

	void stopCoroutines() {
		try { StopCoroutine (SpawnAsteroids()); } catch {}
		try { StopCoroutine (SpawnShieldPickUps()); } catch {}
	}
		
	// Welcome, game over screen:

	void showWelcomeScreen() { welcomeScreen.SetActive (true); }
	void hideWelcomeScreen() { welcomeScreen.SetActive (false); }

	// Camera:

	public void shakeCamera() {
		StartCoroutine(shakeScript.CameraShake ());
	}

	void showSecondCameraScreen () {
		secondCameraScreen.SetActive (true);
	}
		

	// Asteroids:

	public IEnumerator SpawnAsteroids () {
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

	public Vector3 randomPositionInBoundary(Vector3 min, Vector3 max) {
		return new Vector3 (
			Random.Range (min.x, max.x), 
			Random.Range (min.y, max.y), 
			Random.Range (min.z, max.z)
		);
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
		//Debug.Log (string.Format ("asteroids: {0}, demons: {1}, pickups: {2}", asteroidCount, demonCount, shieldPickUpCount));
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
	}
		

	// Demons:

	public void makeDemonsAngrier() {
		foreach (GameObject demon in demonsOnScreen) {
			Demon demonScript = demon.GetComponent<Demon> ();
			demonScript.increaseShootingRate ();
		}
	}

	public void demonEnteredPlayersShiels(GameObject demon) {
		//reduceCounterOf ("Demon");
		Destroy (demon);
	}
		

	// Generic:

	void instantiate(string name, Vector3 spawnPosition, Quaternion spawnRotation) {
		GameObject objectToInstantiate = (name == "Asteroid") ? asteroid : (name == "Demon") ? demon : shieldPickUp;
		GameObject objectInstance = Instantiate (objectToInstantiate, spawnPosition, spawnRotation) as GameObject;

		addToList (name, objectInstance); // also increases the appropriate counter
	}

	int getMaxNumberOnScreen(string objectsName) {
		switch (objectsName) {
		case "Asteroid":
			return asteroidMaxOnScreen;
		case "Demon":
			return maxDemonsOnScreen [scoringSystem.getLevel () - 1];
		case "ShieldPickUp":
			return numberOfShieldPickUps [scoringSystem.getLevel () - 1];
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

	public void removeFromArray(string name, GameObject objectInstance) {
		getCorrectList(name).Remove(objectInstance);
		reduceCounterOf (name);
	}

	void addToList (string name, GameObject objectInstance) {
		getCorrectList (name).Add (objectInstance);
		increaseCounterOf (name);
	}

	public void emptyArray(string name) {
		foreach (GameObject objectInstance in getCorrectList(name)) {
			Destroy (objectInstance);
		}
		resetCounters ();
		instantiateLists ();
	}

	List<GameObject> getCorrectList(string name) {
		return (name == "Asteroid") ? asteroidsOnScreen : (name == "Demon") ? demonsOnScreen : shieldpickupsOnScreen;
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
}
