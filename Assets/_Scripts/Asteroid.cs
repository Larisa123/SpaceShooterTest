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
		//GameObject asteroidChil = transform.FindChild ("Asteroid Collision Avoidance Collider").gameObject;
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
		//GameObject explosionEmpty = GameObject.FindGameObjectWithTag ("ExplosionEmpty");
		//explosionInstance.transform.SetParent (explosionEmpty);
		Destroy (explosionInstance, 1.0f);
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
		Destroy (this.gameObject);
		//explode (); // explode is called from ondestroy
	}

	void asteroidHitShield() {
		gameController.scoringSystem.increaseScore (); 
		Destroy (this.gameObject);
		//explode (); // explode is called from ondestroy
	}

	void asteroidHitPlayer(GameObject player) {// player's bullet hit an asteroid
		gameController.gameOver();
		Destroy (this.gameObject);
		//Destroy (player);
		//explode ();
	}

	void OnDestroy() {
		gameController.reduceCounterOf ("Asteroid");
		explode ();
	}
}
