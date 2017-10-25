using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float tilt;
	public int speed;
	public float xMin, xMax; // boundary

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis ("Horizontal");

		Rigidbody rb = GetComponent<Rigidbody> ();

		rb.velocity = new Vector3 (moveHorizontal, 0.0f, transform.position.z) * speed;

		rb.position = new Vector3(
			Mathf.Clamp(rb.position.x, xMin, xMax),
			0.0f,
			Mathf.Clamp(rb.position.z, rb.position.z, rb.position.z)
		);

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}
}
