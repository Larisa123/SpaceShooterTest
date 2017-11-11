using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public float rotationSize, speed, asteroidDisapearZ;
	private Rigidbody rb;
	public GameObject asteroidExplosion;
	private GameObject player; // ship


	// GameController:
	private GameController gameController;

	void Start () {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController> ();
		player = GameObject.FindGameObjectWithTag("Player") as GameObject;
		giveAsteroidVelocity (); 
		transform.localScale *= Random.Range (0.7f, 1.0f); // just to make them look a bit more random
	}

	void giveAsteroidVelocity() {
		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Random.insideUnitSphere * rotationSize;
		rb.velocity = (player.transform.position - transform.position).normalized * speed; 
	}

	void Update() {
		checkIfAsteroidOutOfBounds ();
	}

	void checkIfAsteroidOutOfBounds() {
		if (transform.position.z < asteroidDisapearZ) {
			gameController.scoringSystem.reduceScore ();
			if (gameController.scoringSystem.checkIfGameOver())
				gameController.gameOver ();
			Destroy (gameObject);
		}
	}

	void explode () {
		GameObject explosionInstance = Instantiate (asteroidExplosion, this.transform.position, this.transform.rotation) as GameObject;
		Destroy (explosionInstance, 0.5f);
		Destroy (this.gameObject);
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "PlayerBullet") {
			playerHitAsteroid (collider.gameObject);

		} else if (collider.tag == "Player") {
			asteroidHitPlayer(collider.gameObject);

		} else if (collider.tag == "PlayerShield") {
			asteroidHitShield ();
		}
	}

	void playerHitAsteroid(GameObject bullet) {// player's bullet hit an asteroid
		gameController.scoringSystem.increaseScore (); 
		//gameController.reduceCounterOf ("Asteroid");
		Destroy (bullet);
		explode (); 
	}

	void asteroidHitShield() {
		gameController.scoringSystem.increaseScore (); 
		explode (); 
	}

	void asteroidHitPlayer(GameObject player) {// player's bullet hit an asteroid
		gameController.gameOver();
		//TODO:Destroy (player);
		explode ();
	}

	void OnDestroy() {
		gameController.removeFromList ("asteroid", this.gameObject);
	}
}
