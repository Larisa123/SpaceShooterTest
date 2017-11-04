using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {
	public int bulletImpulse;
	public int removeBulletZ;


	// Use this for initialization
	void Start () {
		shoot ();
	}

	public void shoot() {
		Rigidbody rb = GetComponent<Rigidbody> ();

		switch (this.gameObject.tag) {
		case "PlayerBullet":
			rb.velocity = new Vector3 (0.0f, 0.0f, 1.0f) * bulletImpulse;
			rb.AddForce (0.0f, 0.0f, bulletImpulse, ForceMode.Impulse);
			break;
		case "DemonBullet":
			rb.velocity = new Vector3 (0.0f, 0.0f, -1.0f) * bulletImpulse;
			rb.AddForce (0.0f, 0.0f, bulletImpulse, ForceMode.Impulse);
			break;
		default:
			break;
		}
	}


	void Update() {
		checkIfOutOfBounds ();
	}

	void checkIfOutOfBounds() {
		switch (this.gameObject.tag) {
		case "PlayerBullet":
			if (transform.position.z > removeBulletZ)
				Destroy (this.gameObject);
			break;
		case "DemonBullet":
			if (transform.position.z > -removeBulletZ)
				Destroy (this.gameObject);
			break;
		}

	}

}
