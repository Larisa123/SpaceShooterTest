using UnityEngine;
using System.Collections;

public class ShieldPickedUp : MonoBehaviour {

	void OnCollisionEnter(Collision collision) {
		Destroy (collision.collider.gameObject);
		Debug.Log ("something entered shield");
	}
}
