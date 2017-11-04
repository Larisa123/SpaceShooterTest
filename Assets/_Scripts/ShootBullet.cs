using UnityEngine;
using System.Collections;

public class ShootBullet : MonoBehaviour {
	public int bulletImpulse;
	public int removeBulletZ;
	private GameObject demon;


	// Use this for initialization
	void Start () {
		demon = GameObject.FindGameObjectWithTag("Demon");
		//if (this.gameObject.tag == "PlayerBullet")
		shoot ();
	}

	public void shoot() {
		Rigidbody rb = GetComponent<Rigidbody> ();

		switch (this.gameObject.tag) {
		case "PlayerBullet":
			rb.velocity = new Vector3 (0.0f, 0.0f, 1.0f) * bulletImpulse;
			//rb.AddForce (0.0f, 0.0f, bulletImpulse, ForceMode.Impulse);
			break;
		case "DemonBullet":
			//Transform bulletSpawn = GameObject.FindGameObjectWithTag ("DemonBulletSpawn").GetComponent<Transform> ();
			//GameObject bullet = Instantiate (this.gameObject, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
			//Rigidbody bulletRb = bullet.GetComponent<Rigidbody> (); 
			//rb.velocity = demon.transform.forward * bulletImpulse;
			rb.velocity = new Vector3 (0.0f, 0.0f, -1.0f) * bulletImpulse;
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