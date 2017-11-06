
using UnityEngine;
using System.Collections;

public class Demon : MonoBehaviour {

	private Transform playerPos;
	private PlayerController playerScript;
	private Vector3 targetPos;
	//public Transform spawnPos;

	public GameObject demonExplosion;
	public GameObject bullet; // not really a bullet, but I will use this name for convention
	public float demonSpeed;
	public float fireRate;
	public float startBulletShootWait;
	public int minDistanceFromPlayer;
	//private float nextFire;
	//public float fireRate;

	public Animator animator;
	private GameController gameController;

	void Start() {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		playerScript = player.GetComponent<PlayerController> ();
		playerPos = player.GetComponent<Transform> ();
		setTargetPosition ();

		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController> ();
		//animator = GetComponent<Animator> ();

		giveDemonVelocity();
		StartCoroutine (shootBulletsCoroutine());
	}


	void Update () {
		this.gameObject.transform.LookAt (playerPos);
		transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * demonSpeed);
	}

	void OnCollisionEnter(Collision collision) {
		GameObject collider = collision.collider.gameObject;
		if (collider.tag == "PlayerBullet")
			bulletHitDemon(collider);
	}

	void setTargetPosition() {
		targetPos = new Vector3(
			Random.Range(playerScript.boundaryMin.x, playerScript.boundaryMax.x),
			Random.Range(playerScript.boundaryMin.y, playerScript.boundaryMax.y),
			Random.Range(minDistanceFromPlayer, minDistanceFromPlayer * 1.5f)
		);
	}

	void bulletHitDemon(GameObject bulletInstance) {
		gameController.reduceCounterOf ("Demon");
		gameController.scoringSystem.increaseScore ();
		explode (bulletInstance);
	}

	void giveDemonVelocity() {
		// TO DO: Use lerp or something instead of velocity, to go quickly at the beginning and slower after that
		// choose spawn positions from which you will shoot, each one will get taken (stored in array) to prevent them shooting from the same point
		//Rigidbody rb = GetComponent<Rigidbody> ();
		//rb.velocity = (playerPos.position - transform.position).normalized * demonSpeed; 
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
		Instantiate(bullet, this.gameObject.transform.position, this.gameObject.transform.rotation);
	}

	void explode(GameObject bulletInstance) {
		Instantiate (demonExplosion, this.transform.position, this.transform.rotation);
		Destroy (bulletInstance);
		Destroy (this.gameObject);
	}
}
