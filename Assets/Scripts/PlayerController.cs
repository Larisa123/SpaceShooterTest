using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float tilt;
	public int speed;
	public float xMin, xMax; // boundary


	public GameObject bullet;
	public Transform bulletSpawn;
	public float fireRate;
	public int bulletSpeed;

	private float nextFire;

	void Update (){
		if (Time.time > nextFire ) {
			nextFire = Time.time + fireRate;
			Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
			Rigidbody bulletRigidBody = bullet.GetComponent<Rigidbody> ();
			bulletRigidBody.velocity = new Vector3 (0.0f, 0.0f, 1.0f) * bulletSpeed;
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
			Mathf.Clamp(rb.position.x, xMin/2, xMax/2),
			0.0f
		);

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}
}
