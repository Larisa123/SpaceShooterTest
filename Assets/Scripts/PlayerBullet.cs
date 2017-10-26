using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {
	public int bulletSpeed;
	public int removeBulletZ;

	// Use this for initialization
	void Start () {
		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.velocity = new Vector3 (0.0f, 0.0f, 1.0f) * bulletSpeed;
	}


	void Update() {
		if (transform.position.z > removeBulletZ)
			Destroy (this.gameObject);
	}


}
