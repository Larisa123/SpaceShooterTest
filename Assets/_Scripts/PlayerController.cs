using UnityEngine;
using System.Collections;

//public struct BulletType {
//}

public class PlayerController : MonoBehaviour {

	// Player - SHIP:
	public float tilt; // when player moves
	public float backThrust; // when player shoots a bullet
	public float backThrustTime;
	public float speed;
	public Vector3 playerBoundaryMin; // player boundary
	public Vector3 playerBoundaryMax; // player boundary
	private Rigidbody rb;
	//private bool movingRight = false;
	public GameObject playerExplosion;


	// BULLET:
	public GameObject bullet;
	public Transform bulletSpawn;
	public float fireRate;
	private float nextFire;

	//Game:
	public GameController gameController;


	void Start() {
		//gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController> ();
		rb = GetComponent<Rigidbody> ();

	}

	void Update () {
		shootBullets ();
	}

	void FixedUpdate () {
		movePlayer ();
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "DemonBullet") {
			demonHitPlayer (collision.collider.gameObject);


		} 
	}

	// Move player:

	void movePlayer() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		rb.velocity = new Vector3 (moveHorizontal * speed, moveVertical * speed, rb.velocity.z);

		rb.position = new Vector3(
			Mathf.Clamp(rb.position.x, playerBoundaryMin.x, playerBoundaryMax.x),
			Mathf.Clamp(rb.position.y, playerBoundaryMin.y, playerBoundaryMax.y),
			Mathf.Clamp(rb.position.z, playerBoundaryMin.z, playerBoundaryMax.z)
		);

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);

	}

	IEnumerator thrustPlayerBack () {
		rb.velocity = -transform.forward * backThrust;
		yield return new WaitForSeconds (backThrustTime / 3);
		rb.velocity = new Vector3 (transform.position.x, transform.position.y, 0.0f);
		yield return new WaitForSeconds (backThrustTime);
		rb.position = new Vector3 (transform.position.x, transform.position.y, 0.0f);
	}

	// Bullets:

	void shootBullets() {
		if (Time.time > nextFire ) {
			nextFire = Time.time + fireRate;
			Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
			//GetComponent<AudioSource>().Play (); // sound effect
			//StartCoroutine(thrustPlayerBack());
		}
	}

	// Demon Bullets:

	void demonHitPlayer(GameObject demonBullet) {
		Destroy (demonBullet);
		gameController.scoringSystem.reducePlayersHealth ();
	}


	// Asteroids:


	void explode () {
		Instantiate (playerExplosion, this.transform.position, this.transform.rotation);
		Destroy (this.gameObject);
	}

}
