using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// ASTEROIDS:
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public Vector3 asteroidSpawnMax;
	public Vector3 asteroidSpawnMin;
	public int asteroidCount;
	public int asteroidMaxOnScreen;
	public GameObject asteroid;

	// ENEMIES - DEMONS:
	public GameObject demon;
	private int demonCount;
	private int[] maxDemonsOnScreen = {0, 4, 7, 10, 15, 20}; // depends on the level


	// Score:
	public Score scoringSystem;

	void Start () {
		scoringSystem = GetComponent<Score> ();
		Debug.Log (scoringSystem.getLevel ());
		StartCoroutine (SpawnAsteroids ());

		/*
		switch (scoringSystem.getLevel ()) {
		case 1: 
			StartCoroutine (SpawnWaves ());
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			break;
		default: 
			break;
		}
		*/

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

	IEnumerator SpawnAsteroids () {
		yield return new WaitForSeconds (startWait);
		while (scoringSystem.gameState == GameState.Playing) {
			for (int i = 0; i < asteroidCount; i++)
			{
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
		if (objectsTag == "Demon")
			demonCount++;
	}

	public void reduceCounterOf(string objectsTag) {
		if (objectsTag == "Asteroid") {
			if (asteroidCount > 0) 
				asteroidCount--;
		}
		if (objectsTag == "Demon") {
			if (demonCount > 0) 
				demonCount--;
		}
	}

	public void resetCounterOf(string objectsTag) {
		if (objectsTag == "Asteroid")
			asteroidCount = 0;
		if (objectsTag == "Demon")
			demonCount = 0;
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
		demonRb.velocity = Vector3.zero; // TO DO: don't just stop it!
	}

	int getMaxDemonsOnScreen() {
		return maxDemonsOnScreen [scoringSystem.getLevel () - 1];
	}

}
