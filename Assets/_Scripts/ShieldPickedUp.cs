using UnityEngine;
using System.Collections;

public class ShieldPickedUp : MonoBehaviour {
	// TO DO: wrong name - should be pick up, without the past tense
	public float speed;
	public float removeZ;
	private GameController gameController;

	void Start() {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		giveVelocity ();
	}

	void Update() {
		checkIfOutOfBounds ();
	}

	void giveVelocity() {
		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Random.insideUnitSphere;
		rb.velocity = (-transform.position).normalized * speed; 
	}

	void checkIfOutOfBounds() {
		if (transform.position.z < removeZ)
			Destroy (this.gameObject);
	}

	void OnDestroy() {
		gameController.reduceCounterOf ("ShieldPickUp");
	}
		
}
