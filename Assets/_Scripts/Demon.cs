
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


	public Animator animator;
	private GameController gameController;

	void Start() {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController> ();
		playerPos = gameController.player.GetComponent<Transform> ();

		giveDemonVelocity();
		StartCoroutine (shootBulletsCoroutine());
	}


	void Update () {
		this.gameObject.transform.LookAt (playerPos);
		//transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * demonSpeed);
	}

	void OnCollisionEnter(Collision collision) {
		GameObject collider = collision.collider.gameObject;
		if (collider.tag == "PlayerBullet")
			bulletHitDemon(collider);
	}

	/*
	void setTargetPosition() {
		targetPos = new Vector3(
			Random.Range(playerScript.boundaryMin.x, playerScript.boundaryMax.x),
			Random.Range(playerScript.boundaryMin.y, playerScript.boundaryMax.y),
			Random.Range(minDistanceFromPlayer, minDistanceFromPlayer * 1.5f)
		);
	}
	*/

	void bulletHitDemon(GameObject bulletInstance) {
		//gameController.reduceCounterOf ("Demon");
		gameController.scoringSystem.increaseScore ();
		Destroy (bulletInstance);
		Destroy (this.gameObject);
	}

	void giveDemonVelocity() {
		Rigidbody rb = GetComponent<Rigidbody> ();
		PlayerController player = gameController.player;
		// target position somewhere towards the player bpundary but with a bigger z:
		//Vector3 minAdjustment = new Vector3(2.0f, 2.0f, minDistanceFromPlayer);
		//Vector3 maxAdjustment = new Vector3(2.0f, 2.0f, minDistanceFromPlayer + 2.0f);

		Vector3 min = player.boundaryMin + new Vector3(2.0f, 2.0f, minDistanceFromPlayer);
		Vector3 max = player.boundaryMax + new Vector3(2.0f, 2.0f, minDistanceFromPlayer + 2.0f);

		Vector3 targetPosition = gameController.randomPositionInBoundary (min, max);
		rb.velocity = (targetPosition - transform.position).normalized * demonSpeed; 
	}


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
		Destroy (explosionInstance, 1.0f);
	}

	void OnDestroy() {
		gameController.reduceCounterOf ("Demon");
		explode ();
	}
}
