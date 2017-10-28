using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Player - SHIP:
	public float tilt; // when player moves
	public float backThrust; // when player shoots a bullet
	public float backThrustTime;
	public int speed;
	public float xMin, xMax; // player boundary
	public float yMin, yMax; // player boundary


	// BULLET:
	public GameObject bullet;
	public Transform bulletSpawn;
	public float fireRate;

	private float nextFire;

	void Start() {
	}

	void Update () {
		if (Time.time > nextFire ) {
			shootBullet ();
		}
	}

	void FixedUpdate () {
		movePlayer ();
	}

	// Move player:

	void movePlayer() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.velocity = new Vector3 (moveHorizontal, moveVertical, 0.0f) * speed;

		rb.position = new Vector3(
			Mathf.Clamp(rb.position.x, xMin, xMax),
			Mathf.Clamp(rb.position.y, yMin, yMax),
			0.0f
		);

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}

	IEnumerator thrustPlayerBack () {
		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.AddForce (-transform.forward * backThrust);
		yield return new WaitForSeconds (backThrustTime);
		rb.AddForce (transform.forward * backThrust);
	}

	// Bullets:

	void shootBullet() {
		nextFire = Time.time + fireRate;
		Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
		//GetComponent<AudioSource>().Play (); // sound effect
		StartCoroutine(thrustPlayerBack());
	}

}
