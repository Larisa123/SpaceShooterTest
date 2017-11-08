using UnityEngine;
using System.Collections;

public class ShieldPickedUp : MonoBehaviour {
	// TO DO: wrong name - should be pick up, without the past tense
	public float speed;
	public float removeZ;
	private GameController gameController;
	private AudioSource shieldPickUpSound;

	void Start() {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		shieldPickUpSound = GetComponent<AudioSource> ();
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
		

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.gameObject.CompareTag ("Player")) {
			Debug.Log ("Player picked up shield from shield");
			shieldPickUpSound.Play ();
		}
	}
}
