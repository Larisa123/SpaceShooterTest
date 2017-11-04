using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public float rotationSize, speed, asteroidDisapearZ;
	//public float scaleMultiplier;
	//public float timeRequiredToScale;
	private Rigidbody rb;
	public GameObject asteroidExplosion;
	private GameObject player; // ship


	// GameController:
	private GameController gameController;

	void Start () {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController> ();
		player = GameObject.FindGameObjectWithTag("Player") as GameObject;
		giveAsteroidVelocity (); 

		// TO DO: make the asteroid come in the player boundary, not directlly the center

		//transform.localScale = Vector3.Lerp (transform.localScale * scaleMultiplier, transform.localScale, timeRequiredToScale);
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
		Instantiate (asteroidExplosion, this.transform.position, this.transform.rotation);
		//Destroy (asteroidExplosion, 1.0f);
		Destroy (this.gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "PlayerBullet") {
			playerHitAsteroid (collision.collider.gameObject);

		} else if (collision.collider.tag == "Player") {
			asteroidHitPlayer(collision.collider.gameObject);

		}
	}

	void playerHitAsteroid(GameObject bullet) {// player's bullet hit an asteroid
		gameController.scoringSystem.increaseScore (); 
		Destroy (bullet);
		explode ();
	}

	void asteroidHitPlayer(GameObject player) {// player's bullet hit an asteroid
		gameController.gameOver();
		//Destroy (player);
		explode ();
	}
}
