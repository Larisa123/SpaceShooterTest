using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public float rotationSize, speed, asteroidDisapearZ;
	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Random.insideUnitSphere * rotationSize;
		rb.velocity = -transform.forward * speed; // -forward means backwards
	}

	void Update() {

		if (transform.position.z < asteroidDisapearZ)
			Destroy (gameObject);
	}

}
