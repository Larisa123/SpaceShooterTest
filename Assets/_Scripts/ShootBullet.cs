using UnityEngine;
using System.Collections;

public class ShootBullet : MonoBehaviour {
	public int removeBulletZ;

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