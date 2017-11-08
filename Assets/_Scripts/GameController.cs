using UnityEngine;
using System.Collections;

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

	// ENEMIES - DEMONS:
	public GameObject demon;
	private static int demonCount = 0;
	private int[] maxDemonsOnScreen = {3, 4, 7, 10, 15, 20}; // depends on the level
	private int[] numberOfShieldPickUps = {1, 2, 4, 6, 9, 12};

	//PickUps
	public GameObject shieldPickUp;
	private static int shieldPickUpCount = 0;

	//Player:
	public PlayerController player;

	// Score:
	public Score scoringSystem;

	[SerializeField] private GameObject secondCameraScreen;

	void Start () {
		showSecondCameraScreen ();
		resetCounters ();
	}

	void showSecondCameraScreen () {
		secondCameraScreen.SetActive (true);
	}

	/*                        RESTART:
	void Update ()
	{
		if (restart)
		{
			if (Input.GetKeyDown (KeyCode.R))
			{
				Application.LoadLevel (Application.loadedLevel);
			}
		}
	}
	*/

	public void gameOver() {
		Debug.Log ("Game over");
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
		if (!shouldCreateNewAsteroid ())
			return;
		Vector3 spawnPosition = new Vector3 (
			Random.Range (asteroidSpawnMin.x, asteroidSpawnMax.x), 
			Random.Range (asteroidSpawnMin.y, asteroidSpawnMax.y), 
			Random.Range (asteroidSpawnMin.z, asteroidSpawnMax.z)
		);
		Quaternion spawnRotation = Quaternion.identity;
		Instantiate (asteroid, spawnPosition, spawnRotation);
		increaseCounterOf ("Asteroid");

		if (shouldCreateNewDemon())
			instantiateDemon (spawnPosition, spawnRotation);
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

	bool shouldCreateNewAsteroid() {
		return asteroidCount < asteroidMaxOnScreen;
	}

	// Demons:

	bool shouldCreateNewDemon() {
		if (demonCount < getMaxDemonsOnScreen()) {
			if (Random.Range (1, 10) % 2 == 0)  // just a random condition with probabilty of being true equal to 1/2
				return true;
		} // else:
		return false;
	}

	void instantiateDemon(Vector3 spawnPosition, Quaternion spawnRotation) {
		Instantiate (demon, spawnPosition, spawnRotation); // TO DO: DEMON IN NEW METHOD!
		increaseCounterOf("Demon");
	}

	public void demonEnteredPlayersShiels(GameObject demon) {
		Rigidbody demonRb = demon.GetComponent<Rigidbody> ();
		Destroy (demon);
		//demonRb.velocity = Vector3.zero; // TO DO: don't just stop it!
	}

	int getMaxDemonsOnScreen() {
		return maxDemonsOnScreen [scoringSystem.getLevel () - 1];
	}
		
	// Pick Ups:
	int getMaxShieldPickUps() {
		return 	numberOfShieldPickUps [scoringSystem.getLevel () - 1];	
	}

	public IEnumerator SpawnShieldPickUps () {
		if (!(shieldPickUpCount < getMaxShieldPickUps ()))
			yield break;
		
		yield return new WaitForSeconds (2.0f * scoringSystem.getLevel());
		for (int i = 0; i < getMaxShieldPickUps(); i++) {
			instantiateShieldPickUp ();
			yield return new WaitForSeconds (spawnWait * 5.0f * scoringSystem.getLevel());
		}
	}

	public void instantiateShieldPickUp() {
		Vector3 spawnPosition = new Vector3 (
			Random.Range (asteroidSpawnMin.x + 1.0f, asteroidSpawnMax.x - 1.0f), 
			Random.Range (asteroidSpawnMin.y + 1.0f, asteroidSpawnMax.y - 1.0f), 
			Random.Range (7.0f, 12.0f)
		);

		Quaternion spawnRotation = Quaternion.identity;
		Instantiate (shieldPickUp, spawnPosition, spawnRotation);
		increaseCounterOf ("ShieldPickUp");
	}
}
