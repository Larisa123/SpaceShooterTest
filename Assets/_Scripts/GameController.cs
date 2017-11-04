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
	public GameObject asteroid;

	// ENEMIES - DEMONS:
	public GameObject demon;
	private int demonsOnScreen;
	public int maxDemonsOnScreen;
	/*
	public int maxDemonsOnScreen;
	public int totalDemons;
	public float minSpawnTime;
	public float maxSpawnTime;
	public int demonsPerSpawn;

	private int demonsOnScreen = 0;
	private float generatedSpawnTime = 0;
	private float currentSpawnTime = 0;
	*/

	/*
	private int demonsOnScreen = 0;
	public int maxDemonsOnScreen;
	public int demonsPerSpawn;
	public float spawnTime;
	*/

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
		while (true) {
			for (int i = 0; i < asteroidCount; i++)
			{
				instantiateAsteroid ();
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}
		
	public void instantiateAsteroid() {
		Vector3 spawnPosition = new Vector3 (
			Random.Range (asteroidSpawnMin.x, asteroidSpawnMax.x), 
			Random.Range (asteroidSpawnMin.y, asteroidSpawnMax.y), 
			Random.Range (asteroidSpawnMin.z, asteroidSpawnMax.z)
		);
		Quaternion spawnRotation = Quaternion.identity;
		Instantiate (asteroid, spawnPosition, spawnRotation);
		Instantiate (demon, spawnPosition, spawnRotation); // TO DO: DEMON IN NEW METHOD!
	}


	public void demonEnteredPlayersShiels(GameObject demon) {
		Rigidbody demonRb = demon.GetComponent<Rigidbody> ();
		demonRb.velocity = Vector3.zero; // TO DO: don't just stop it!
	}

}
