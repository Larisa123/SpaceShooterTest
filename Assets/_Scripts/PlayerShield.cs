using UnityEngine;
using System.Collections;

public class PlayerShield : MonoBehaviour {


	void OnCollisionEnter(Collision collision) {
		Destroy (collision.collider.gameObject);
	}
}
