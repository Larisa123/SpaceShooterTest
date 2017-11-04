
using UnityEngine;
using System.Collections;

public class Demon : MonoBehaviour {

	private Transform playerPos;
	//public Transform spawnPos;

	public GameObject fireAnimation;
	public GameObject bullet; // not really a bullet, but I will use this name for convention
	public int demonSpeed;
	public float fireRate;
	//private float nextFire;
	//public float fireRate;

	private Animator animator;

	void Start() {
		playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform> ();
		animator = GetComponent<Animator> ();
		//animator.Play ("demonAttact");
		//fireAnimation.Play ();
		StartCoroutine (shootBulletsCoroutine());
	}


	void Update () {
		this.gameObject.transform.LookAt (playerPos);
	}

	void giveDemonVelocity() {
		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.velocity = (playerPos.position - transform.position).normalized * demonSpeed; 
	}

	IEnumerator shootBulletsCoroutine () {
		Instantiate(bullet, this.gameObject.transform.position, this.gameObject.transform.rotation);
		yield return new WaitForSeconds (fireRate);

	}

	/*

	public void shootBullet() {
		if (Time.time > nextFire ) {
			nextFire = Time.time + fireRate;
			Instantiate(bullet, this.gameObject.transform.position,  this.gameObject.transform.rotation);
			//GetComponent<AudioSource>().Play (); // sound effect
			//StartCoroutine(thrustPlayerBack());
		}
	}
	*/

}
