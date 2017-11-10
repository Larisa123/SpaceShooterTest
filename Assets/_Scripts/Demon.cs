
using UnityEngine;
using System.Collections;

public class Demon : MonoBehaviour {

	private Transform playerPos;

	public GameObject demonExplosion;
	public GameObject bullet; // not really a bullet, but I will use this name for convention
	public float bulletImpulse;
	public float demonSpeed;
	public float fireRate;
	public float startBulletShootWait;
	public int minDistanceFromPlayer;
	private Rigidbody rb;


	public Animator animator;
	private GameController gameController;

	void Start() {
		rb = GetComponent<Rigidbody> ();
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController> ();
		playerPos = gameController.player.GetComponent<Transform> ();

		giveDemonVelocity();
		StartCoroutine (shootBulletsCoroutine());
	}

	void Update () {
		if (gameController.scoringSystem.gameState == GameState.Playing) {
			this.gameObject.transform.LookAt (playerPos);
		} else {
			try { StopCoroutine (shootBulletsCoroutine()); } catch {}
		}
	}

	void OnCollisionEnter(Collision collision) {
		GameObject collider = collision.collider.gameObject;
		if (collider.tag == "PlayerBullet") {
			bulletHitDemon(collider); 
		} else if (collider.tag == "PlayerShield") {
			bulletHitDemon(collider); 
		}
	}

	void bulletHitDemon(GameObject bulletInstance) {
		//gameController.reduceCounterOf ("Demon");
		gameController.scoringSystem.reduceScore ();
		Destroy (bulletInstance);
		explode ();
	}

	void demonEnteredPlayersShiels( ) {
		explode ();
	}

	void giveDemonVelocity() {
		PlayerController player = gameController.player;

		Vector3 min = player.boundaryMin + new Vector3(2.0f, 2.0f, minDistanceFromPlayer);
		Vector3 max = player.boundaryMax + new Vector3(-2.0f, -2.0f, minDistanceFromPlayer + 2.0f);
		//TODO: they dont stop here! change something so they wont be able to come too close to the ship

		Vector3 targetPosition = gameController.randomPositionInBoundary (min, max);
		rb.velocity = (targetPosition - transform.position).normalized * demonSpeed; 
	}

	public void increaseShootingRate() { fireRate = (fireRate > 0.4f)? fireRate / 1.5f: 0.4f ;}

	IEnumerator shootBulletsCoroutine () {
		yield return new WaitForSeconds (startBulletShootWait);
		while (gameController.scoringSystem.gameState == GameState.Playing) {
			shootBullet ();
			yield return new WaitForSeconds (fireRate);
		}
	}

	public void shootBullet() { // actually a fireball
		animator.Play("demonAttack");
		GameObject bulletInstance = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
		Rigidbody bulletInstanceRb = bulletInstance.GetComponent<Rigidbody> ();
		bulletInstanceRb.velocity = (playerPos.position - transform.position).normalized * bulletImpulse;
	}

	void explode() {
		GameObject explosionInstance = Instantiate (demonExplosion, this.transform.position, this.transform.rotation) as GameObject;
		Destroy (explosionInstance, 1.5f);
		Destroy (this.gameObject, 0.2f);
	}

	void OnDestroy() {
		gameController.removeFromList ("Demon", this.gameObject);
		gameController.makeDemonsAngrier ();// other demons should get angry
	}
}
