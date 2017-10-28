using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// ASTEROIDS:
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public Vector3 spawnValues;
	[SerializeField] private float spawnVariationZ;
	[SerializeField] private float spawnVariationY;
	public int asteroidCount;
	public GameObject asteroid;

	void Start () {

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
			Random.Range (-spawnValues.x, spawnValues.x), 
			Random.Range (spawnValues.y, spawnValues.y + spawnVariationY), 
			Random.Range (spawnValues.z, spawnValues.z + spawnVariationZ)
		);
		Quaternion spawnRotation = Quaternion.identity;
		Instantiate (asteroid, spawnPosition, spawnRotation);
	}
}
