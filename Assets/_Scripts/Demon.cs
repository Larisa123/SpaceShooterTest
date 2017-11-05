
using UnityEngine;
using System.Collections;

public class Demon : MonoBehaviour {

	private Transform playerPos;
	//public Transform spawnPos;

	public GameObject demonExplosion;
	public GameObject bullet; // not really a bullet, but I will use this name for convention
	public int demonSpeed;
	public float fireRate;
	public float startBulletShootWait;
	//private float nextFire;
	//public float fireRate;

	public Animator animator;
	private GameController gameController;

	void Start() {
		playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform> ();
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController> ();
		//animator = GetComponent<Animator> ();

		giveDemonVelocity();
		StartCoroutine (shootBulletsCoroutine());
	}


	void Update () {
		this.gameObject.transform.LookAt (playerPos);
	}

	void OnCollisionEnter(Collision collision) {
		GameObject collider = collision.collider.gameObject;
		if (collider.tag == "PlayerBullet")
			bulletHitDemon(collider);
	}

	void bulletHitDemon(GameObject bullet) {
		Debug.Log("Demon Destroyed");
		Instantiate (demonExplosion, this.transform.position, this.transform.rotation);
		Destroy (bullet);
		Destroy (this.gameObject);
	}

	void giveDemonVelocity() {
		// TO DO: Use lerp or something instead of velocity, to go quickly at the beginning and slower after that
		// choose spawn positions from which you will shoot, each one will get taken (stored in array) to prevent them shooting from the same point
		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.velocity = (playerPos.position - transform.position).normalized * demonSpeed; 
	}


	IEnumerator shootBulletsCoroutine () {
		yield return new WaitForSeconds (startBulletShootWait);
		while (gameController.scoringSystem.gameState == GameState.Playing) {
			shootBullet ();
			yield return new WaitForSeconds (fireRate);
		}
	}

	public void shootBullet() { // actually a fireball
		Debug.Log("shoot fireball in demon's courutine");
		animator.Play("demonAttact");
		Instantiate(bullet, this.gameObject.transform.position, this.gameObject.transform.rotation);
	}

	/*

	public void shootBullet() {
		if (Time.time > nextFire ) {
			nextFire = Time.time + fireRate;
			Instantiate(bullet, this.gameObject.transform.position,  this.gameObject.transform.rotation);
			//GetComponent<AudioSource>().Play (); // sound effect
			//StartCoroutine(thrustPlayerBack());
		}
	}
	*/

}
