using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float tilt;
	public int speed;
	public float xMin, xMax; // boundary
	public float yMin, yMax;


	public GameObject bullet;
	public Transform bulletSpawn;
	public float fireRate;

	private float nextFire;

	void Update (){
		if (Time.time > nextFire ) {
			nextFire = Time.time + fireRate;
			Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
			//GetComponent<AudioSource>().Play ();
		}
	}

	void FixedUpdate () {
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
}
