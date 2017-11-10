using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

//public struct BulletType {
//}

public enum BulletType {Regular, TwoRegular, ThreeRegular, ThreeRegularBigExplosion, SuperExplosion}

public class PlayerController : MonoBehaviour {

	// Player - SHIP:
	public float tilt; // when player moves
	public float backThrust; // when player shoots a bullet
	public float backThrustTime;
	public float speed;
	public Vector3 boundaryMin; // player boundary
	public Vector3 boundaryMax; // player boundary
	private Rigidbody rb;
	//private bool movingRight = false;
	public GameObject playerExplosion;
	public BulletType bulletType = BulletType.Regular;
	public float bulletImpulse;


	private GameObject shield;
	public float shieldDuration;
	public bool hasShieldOn = false;
	//private AudioSource shieldPickUpSound;

	// BULLET:
	public GameObject bullet;
	public Transform bulletSpawn;
	public Transform[] bulletSpawnTwoBullets;
	public float fireRate;
	private float nextFire;

	//Game:
	public GameController gameController;


	void Start() {
		//shieldPickUpSound = GetComponent<AudioSource> ();
		shield = this.gameObject.transform.FindChild("Player Shield").gameObject;
		//gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController> ();
		rb = GetComponent<Rigidbody> ();

	}

	void Update () {
		shootBullets ();
	}

	void FixedUpdate () {
		movePlayer ();
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "DemonBullet") {
			demonHitPlayer (collision.collider.gameObject);

		} else if (collision.collider.tag == "PlayerShieldPickUp") {
			Debug.Log ("Player picked up shield from player");
			shieldPickedUp (collision.collider.gameObject);

		} 
	}

	// Move player:

	void movePlayer() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		rb.velocity = new Vector3 (moveHorizontal * speed, moveVertical * speed, rb.velocity.z);

		rb.position = new Vector3(
			Mathf.Clamp(rb.position.x, boundaryMin.x, boundaryMax.x),
			Mathf.Clamp(rb.position.y, boundaryMin.y, boundaryMax.y),
			Mathf.Clamp(rb.position.z, boundaryMin.z, boundaryMax.z)
		);

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);

	}

	IEnumerator thrustPlayerBack () {
		rb.velocity = -transform.forward * backThrust;
		yield return new WaitForSeconds (backThrustTime / 3);
		rb.velocity = new Vector3 (transform.position.x, transform.position.y, 0.0f);
		yield return new WaitForSeconds (backThrustTime);
		rb.position = new Vector3 (transform.position.x, transform.position.y, 0.0f);
	}

	void explode () {
		GameObject explosionInstance = Instantiate (playerExplosion, this.transform.position, this.transform.rotation) as GameObject;
		Destroy (this.gameObject);
		Destroy (explosionInstance, 0.3f);
	}

	// Bullets:

	void shootBullets() {
		if (Time.time > nextFire ) {
			nextFire = Time.time + fireRate;

			if (bulletType == BulletType.Regular) {
				shootRegularBullet ();
			} else if (bulletType == BulletType.TwoRegular) {
				shootTwoRegularBullets ();
			} else if (bulletType == BulletType.ThreeRegular) {
				shootRegularBullet ();
				shootTwoRegularBullets ();
			} else if (bulletType == BulletType.ThreeRegularBigExplosion) {
				shootRegularBullet ();
				shootTwoRegularBullets ();
				//shootBigExplosion (); tu je treba preverit se da je ze cas za novo eksplozijo, ne gre na vsaki nextFire
			}
		}
	}

	void shootRegularBullet() {
		//Instantiate(bullet, transform.position, transform.rotation);
		//GetComponent<AudioSource>().Play (); // sound effect
		//StartCoroutine(thrustPlayerBack());
		GameObject bulletInstance = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
		Rigidbody bulletInstanceRb = bulletInstance.GetComponent<Rigidbody> ();
		bulletInstanceRb.velocity = new Vector3 (0.0f, 0.0f, 1.0f) * bulletImpulse;
		//bulletInstanceRb.angularVelocity = transform.rotation * transform.forward * bulletImpulse;
	}

	void shootTwoRegularBullets() {
		float angle = -0.2f;
		foreach (Transform trans in bulletSpawnTwoBullets) {
			angle = -angle;
			GameObject bulletInstance = Instantiate(bullet, trans.position, trans.rotation) as GameObject;
			Rigidbody bulletInstanceRb = bulletInstance.GetComponent<Rigidbody> ();
			bulletInstanceRb.velocity = new Vector3 (angle, 0.0f, 1.0f) * bulletImpulse;
		}
	}

	public void resetBulletType() {
		bulletType = BulletType.Regular;
	}

	public void increaseBulletType() {
		switch (bulletType) {
		case BulletType.Regular:
			bulletType = BulletType.TwoRegular;
			break;
		case BulletType.TwoRegular:
			bulletType = BulletType.ThreeRegular;
			break;
		case BulletType.ThreeRegular:
			bulletType = BulletType.ThreeRegularBigExplosion;
			break;
		case BulletType.ThreeRegularBigExplosion:
			bulletType = BulletType.SuperExplosion;
			break;
		default:
			break;
		} 
	}
		
	// Pick Ups:

	void shieldPickedUp(GameObject shieldPickUp) {
		if (hasShieldOn) {
			gameController.scoringSystem.increaseScore ();
			return;
		}
		GameObject audioSourceObject = shieldPickUp.GetComponent<AudioSource> ().gameObject;
		audioSourceObject.SetActive (true);
		Destroy (shieldPickUp, 0.2f);
		//GameObject shieldInstance = Instantiate (shield, this.transform.position, this.transform.rotation) as GameObject;
		//Instantiate (shield, this.transform.position, this.transform.rotation);
		hasShieldOn = true;
		shield.SetActive (true);
		StartCoroutine (endShield());
	}

	IEnumerator endShield() {
		yield return new WaitForSeconds (shieldDuration);
		shield.SetActive (false);
		hasShieldOn = false;
	}


	// Demon Bullets:

	void demonHitPlayer(GameObject demonBullet) {
		gameController.shakeCamera ();
		Destroy (demonBullet);
		gameController.scoringSystem.reducePlayersHealth ();
	}
}
