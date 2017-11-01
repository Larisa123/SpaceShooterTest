using UnityEngine;
using System.Collections;

public class Demon : MonoBehaviour {

	public Transform playerPos;

	public GameObject fireAnimation;
	public Animator animator;

	void Start() {
		playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform> ();
		animator = GetComponent<Animator> ();
		animator.Play ("demonAttact");
		//fireAnimation.Play ();

	}

	void Update () {
		this.gameObject.transform.LookAt (playerPos);
	}
}
