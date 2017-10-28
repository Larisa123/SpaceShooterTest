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
	public float xMin, xMax; // player boundary
	public float yMin, yMax; // player boundary
	private Rigidbody rb;
	//private bool movingRight = false;



	// BULLET:
	public GameObject bullet;
	public Transform bulletSpawn;
	public float fireRate;
	private float nextFire;

	void Start() {
		rb = GetComponent<Rigidbody> ();

	}

	void Update () {
		shootBullets ();
	}

	void FixedUpdate () {
		movePlayer ();
	}

	// Move player:

	void movePlayer() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		rb.velocity = new Vector3 (moveHorizontal * speed, moveVertical * speed, rb.velocity.z);

		rb.position = new Vector3(
			Mathf.Clamp(rb.position.x, xMin, xMax),
			Mathf.Clamp(rb.position.y, yMin, yMax),
			Mathf.Clamp(rb.position.z, -2.0f, 0.0f)
		);

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);

		/*              /// Z silami:
		 * 
		bool wantsToGoRight = moveHorizontal > 0.0f;
		if ((wantsToGoRight && !movingRight) || (!wantsToGoRight && movingRight))
			rb.velocity = Vector3.zero; // vstavimo ga, ce hoce menjati smer
		//Vector3 velocity = new Vector3 (moveHorizontal, moveVertical, 0.0f) * speed;
		Quaternion rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
		//rb.AddForce (velocity);
		rb.AddForce(moveHorizontal * speed, moveVertical * speed, 0.0f, ForceMode.Impulse);
		movingRight = moveHorizontal > 0.0f;

		*/
	}

	IEnumerator thrustPlayerBack () {
		rb.velocity = -transform.forward * backThrust;
		Debug.Log ("pushed it back");
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


	// Asteroids:

	void OnCollisionEnter(Collision collision) {
		Debug.Log (collision.collider.tag);
	}

}
