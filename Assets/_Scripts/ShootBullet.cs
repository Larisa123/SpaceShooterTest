using UnityEngine;
using System.Collections;

public class ShootBullet : MonoBehaviour {
	public int bulletImpulse;
	public int removeBulletZ;
	public float fireRate;
	public Rigidbody rb;

	//private GameObject demon;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		//demon = GameObject.FindGameObjectWithTag("Demon");
		addBulletVelocity ();
	}

	public void addBulletVelocity() {

		switch (this.gameObject.tag) {
		case "PlayerBullet":
			rb.velocity = new Vector3 (0.0f, 0.0f, 1.0f) * bulletImpulse;
			//rb.AddForce (0.0f, 0.0f, bulletImpulse, ForceMode.Impulse);
			break;
		case "DemonBullet":
			Transform playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform> ();
			rb.velocity = (playerPos.position - transform.position).normalized * bulletImpulse;
			//rb.AddForce (demon.transform.forward * bulletImpulse, ForceMode.Impulse);
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
			if (transform.position.z < removeBulletZ)
				Destroy (this.gameObject);
			break;
		}

	}

}