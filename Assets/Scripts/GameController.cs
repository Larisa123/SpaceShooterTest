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

	// Score:
	public Score scoringSystem;

	void Start () {
		scoringSystem = GetComponent<Score> ();
		StartCoroutine (SpawnWaves ());
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

	IEnumerator SpawnWaves () {
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
	}
}
