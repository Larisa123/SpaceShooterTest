﻿using UnityEngine;
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

	//PickUps
	public GameObject shieldPickUp;
	private static int shieldPickUpCount = 0;

	//Player:
	public PlayerController player;

	// Score:
	public Score scoringSystem;

	// Canvas:
	public GameObject welcomeScreen;

	[SerializeField] private GameObject secondCameraScreen;

	void Start () {
		welcomeScreen.SetActive (true);
		demonsOnScreen = new List<GameObject> ();
		shakeScript = mainCamera.GetComponent<Shake> ();
		//TODO: add a reference and a method for the play button
		//TODO: create game over screen and play again button 

	}

	public void endTheGame() {
		stopCoroutines ();
		emptyDemonArray ();
		//TODO: empty asteroid array - create asteroid array
	}

	void stopCoroutines() {
		try { StopCoroutine (SpawnAsteroids()); } catch {}
		try { StopCoroutine (SpawnShieldPickUps()); } catch {}
	}

	public void startNewGame() {
		welcomeScreen.SetActive (false);
		scoringSystem.resetScoringSystem ();

		showSecondCameraScreen ();
		resetCounters ();
		// call other functions
	}

	public void shakeCamera() {
		StartCoroutine(shakeScript.CameraShake ());
	}

	void showSecondCameraScreen () {
		secondCameraScreen.SetActive (true);
	}
		
	public void gameOver() {
		endTheGame ();
		scoringSystem.resetScoringSystem ();
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
		Vector3 spawnPosition = randomPositionInBoundary (asteroidSpawnMin, asteroidSpawnMax);
		Quaternion spawnRotation = Quaternion.identity;
		Instantiate (asteroid, spawnPosition, spawnRotation);
		increaseCounterOf ("Asteroid");

		if (shouldCreateNewDemon())
			instantiateDemon (spawnPosition, spawnRotation);
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
		GameObject demonInstance = Instantiate (demon, spawnPosition, spawnRotation) as GameObject;
		demonsOnScreen.Add (demonInstance);
		increaseCounterOf("Demon");
	}

	public void removeDemonFromArray(GameObject demonInstance) {
		demonsOnScreen.Remove (demonInstance);
	}

	public void emptyDemonArray() {
		foreach (GameObject demon in demonsOnScreen) {
			Destroy (demon);
		} 
		Debug.Log ("number of items in demonarray after emptying it: " + demonsOnScreen.Count);
	}

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
		
		yield return new WaitForSeconds (5.0f * scoringSystem.getLevel());
		for (int i = 0; i < getMaxShieldPickUps(); i++) {
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
		Instantiate (shieldPickUp, spawnPosition, spawnRotation);
		increaseCounterOf ("ShieldPickUp");
	}
}
