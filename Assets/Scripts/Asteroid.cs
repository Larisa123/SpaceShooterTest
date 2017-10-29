using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public float rotationSize, speed, asteroidDisapearZ;
	//public float scaleMultiplier;
	//public float timeRequiredToScale;
	private Rigidbody rb;
	public GameObject asteroidExplosion;


	// GameController:
	public GameController gameController;

	void Start () {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController> ();
		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Random.insideUnitSphere * rotationSize;
		rb.velocity = -transform.forward * speed; // -forward means backwards

		//transform.localScale = Vector3.Lerp (transform.localScale * scaleMultiplier, transform.localScale, timeRequiredToScale);
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
		Destroy (this.gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Player Bullet") {
			playerHitAsteroid (collision.collider.gameObject);

		}
	}

	void playerHitAsteroid(GameObject bullet) {// player's bullet hit an asteroid
		gameController.scoringSystem.increaseScore (); 
		Destroy (bullet);
		explode ();
	}
}
