using UnityEngine;
using System.Collections;

public class AsteroidDecorative : MonoBehaviour {

	void Start () {
		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Random.insideUnitSphere;
	}

}
