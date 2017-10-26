using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject[] hazards;
	public Vector3 spawnValues;
	[SerializeField] private float spawnVariationZ;
	[SerializeField] private float spawnVariationY;
	public int hazardCount;
	public GameObject hazard;
	public float spawnWait;
	public float startWait;
	public float waveWait;

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

	IEnumerator SpawnWaves () {
		yield return new WaitForSeconds (startWait);
		while (true) {
			for (int i = 0; i < hazardCount; i++)
			{
				//GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (
					Random.Range (-spawnValues.x, spawnValues.x), 
					Random.Range (spawnValues.y, spawnValues.y + spawnVariationY), 
					Random.Range (spawnValues.z, spawnValues.z + spawnVariationZ)
				);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}
}
