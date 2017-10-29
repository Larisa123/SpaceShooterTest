using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public float rotationSize, speed, asteroidDisapearZ;
	//public float scaleMultiplier;
	//public float timeRequiredToScale;
	private Rigidbody rb;
	public GameObject asteroidExplosion;



	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Random.insideUnitSphere * rotationSize;
		rb.velocity = -transform.forward * speed; // -forward means backwards

		//transform.localScale = Vector3.Lerp (transform.localScale * scaleMultiplier, transform.localScale, timeRequiredToScale);
	}

	void Update() {

		if (transform.position.z < asteroidDisapearZ)
			Destroy (gameObject);
	}

	void explode () {
		Instantiate (asteroidExplosion, this.transform.position, this.transform.rotation);
		Destroy (this.gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Player Bullet") {
			Destroy (collision.collider.gameObject);
			explode ();

		}
	}
}
