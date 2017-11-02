using UnityEngine;
using System.Collections;

public class Demon : MonoBehaviour {

	public Transform playerPos;
	public Transform spawnPos;

	public GameObject fireAnimation;
	public GameObject bullet; // not really a bullet, but I will use this name for convention

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

	public void shootBullet() {
		Instantiate(bullet, spawnPos.position, spawnPos.rotation);
			//GetComponent<AudioSource>().Play (); // sound effect
			//StartCoroutine(thrustPlayerBack());
	}
}
