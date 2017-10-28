using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {
	public int bulletImpulse;
	public int removeBulletZ;

	// Use this for initialization
	void Start () {
		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.velocity = new Vector3 (0.0f, 0.0f, 1.0f) * bulletImpulse;
		rb.AddForce (0.0f, 0.0f, bulletImpulse, ForceMode.Impulse);
	}


	void Update() {
		if (transform.position.z > removeBulletZ)
			Destroy (this.gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Asteroid") {
			Destroy (this.gameObject);
			DestroyObject (collision.collider.gameObject);
		}
	}

}
