using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


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
	private int[] maxAsteroidsOnScreen = {7, 10, 14, 19, 23, 27};
	public GameObject asteroid;
	private static List<GameObject> asteroidsOnScreen;

	// ENEMIES - DEMONS:
	public GameObject demon;
	private static int demonCount = 0;
	[HideInInspector] public int howManyHasHeKilled = 0;
	private const int allowedNumberOfDemonsKilled = 4;
	private int[] maxDemonsOnScreen; // depends on the level
	private static List<GameObject> demonsOnScreen;
	[HideInInspector] public bool killedByDemons = false;

	//PickUps
	public GameObject shieldPickUp;
	private static int shieldPickUpCount = 0;
	private static List<GameObject> shieldpickupsOnScreen;
	private int[] maxShieldPickUpsOnScreen = {1, 2, 3, 4, 5, 7}; // depends on the level

	//Player:
	public GameObject player;
	[HideInInspector] public PlayerController playerController;

	// Score:
	public Score scoringSystem;

	// Canvas, Cameras:
	public GameObject welcomeScreen;
	public GameObject gameOverScreen;
	public Text gameOverMessageText;
	[HideInInspector] private bool gameOverScreenShowing = false;

	public Camera mainCamera;
	private Shake shakeScript;
	[SerializeField] private GameObject secondCameraScreen;


	void Start () {
		instantiateOnStart ();
	}

	void instantiateOnStart() {
		showWelcomeScreen (true);
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

	public void OnPlayButtonClick() { // gets called automatically when the user clicks the Play button
		startNewGame(); // resets the score, game state and stuff like that
	}


	// Game:

	public void startNewGame() {
		if (gameOverScreenShowing)
			showGameOverScreen (false);
		else showWelcomeScreen (false);

		showPlayer (true);
		scoringSystem.resetScoringSystem ();
		scoringSystem.showPlayCanvasComponents (true);
		resetMaxDemonsOnScreenList ();

		showSecondCameraScreen (true);
		resetCounters ();
		StartCoroutine (SpawnAsteroidsAndDemons ());

		// call other functions
	}

	public void endTheGame() {
		scoringSystem.gameState = GameState.GameOverScreen;
		scoringSystem.showPlayCanvasComponents (false);
		showSecondCameraScreen (false);
		showPlayer (false);
		//stopCoroutines ();

		// empty lists:
		foreach(string name in new string[3] {"Demon", "Asteroid", "ShieldPickUp"}) {
			emptyList(name);
		}
	}

	public void gameOver() {
		endTheGame ();
		setGameOverScreen ();
		//scoringSystem.resetScoringSystem ();
		// show game over screen
		//TODO: some objects dont get destroyed, destroy them manually!
	}
		
	void setGameOverScreen() {
		string appropriateMessage = getAppropriateGameOverText ();
		gameOverMessageText.text = appropriateMessage;
		showGameOverScreen (true);

		//TODO: why doesnt it work??
	}
		
	string getAppropriateGameOverText() {
		if (tooManyDemonsKilled()) {
			if (killedByDemons)
				return GameOverText.KilledByDemonsKilledToManyDemons;
			else
				return GameOverText.KilledByAsteroidKilledToManyDemons;
		}
		if (killedByDemons)
			return GameOverText.KilledByDemons;
		return GameOverText.KilledByAsteroid;
	}

	void stopCoroutines() {
		//try { StopCoroutine (SpawnAsteroidsAndDemons()); } catch {}
		//try { StopCoroutine (SpawnShieldPickUps()); } catch {}
		//try { stopCoroutines(); } catch {Debug.Log("Coroutines werent properly stopped");}
	}
		
	// Welcome, game over screen:

	void showWelcomeScreen(bool value) { welcomeScreen.SetActive (value); }
	void showGameOverScreen(bool value) { gameOverScreen.SetActive (value); gameOverScreenShowing = value; }

	// Camera:

	public void shakeCamera() {
		StartCoroutine(shakeScript.CameraShake ());
	}

	void showSecondCameraScreen (bool value) {
		secondCameraScreen.SetActive (value);
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

	bool tooManyDemonsKilled() {
		return howManyHasHeKilled >= allowedNumberOfDemonsKilled;
	}

	// Pick Ups:

	public IEnumerator SpawnShieldPickUps () {
		if (scoringSystem.gameState != GameState.Playing)
			yield break;
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

	//Player:

	void showPlayer(bool value) { player.SetActive (value); }

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
		GameObject[] tempArray = new GameObject[list.Count];
		int i = 0;

		foreach (GameObject objectInstance in list) {
			tempArray [i] = objectInstance;
			i++;
		}
		list.Clear ();
		for (int j = 0; j < tempArray.Length ;j++) {
			Destroy (tempArray[j]);
		}

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
		killedByDemons = false;
	}

	int getMaxNumberOnScreen(string objectsName) {
		switch (objectsName) {
		case "Asteroid":
			return maxAsteroidsOnScreen[scoringSystem.getLevel () - 1];
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