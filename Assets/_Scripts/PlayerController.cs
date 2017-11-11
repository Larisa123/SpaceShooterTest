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
	public GameObject playerExplosion;
	public BulletType bulletType = BulletType.Regular;
	public float bulletImpulse;


	private GameObject shield;
	public float shieldDuration;
	[HideInInspector] public bool hasShieldOn = false;

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
		rb = GetComponent<Rigidbody> ();

	}

	void Update () {
		if (gameController.scoringSystem.gameState == GameState.Playing) {
			shootBullets ();
		} else {
			try { StopCoroutine (endShield()); } catch {}
		}
	}

	void FixedUpdate () {
		if (gameController.scoringSystem.gameState == GameState.Playing)
			movePlayer ();
	}

	void OnCollisionEnter(Collision collision) {
		if (hasShieldOn && collision.collider.tag != "PlayerShieldPickUp") {
			Destroy (collision.collider);
			return;
		}

		if (collision.collider.tag == "DemonBullet") {
			demonHitPlayer (collision.collider.gameObject);

		} else if (collision.collider.tag == "PlayerShieldPickUp") {
			shieldPickedUp (collision.collider.gameObject);

		} 
	}

	// Move player:

	void movePlayer() {
		// This code will only get exectuted it game state is set to .playing
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

	public void explode () {
		GameObject explosionInstance = Instantiate (playerExplosion, this.transform.position, this.transform.rotation) as GameObject;
		gameController.showPlayer (false);
		Destroy (explosionInstance, 1.0f);
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
		GameObject bulletInstance = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
		Rigidbody bulletInstanceRb = bulletInstance.GetComponent<Rigidbody> ();
		bulletInstanceRb.velocity = new Vector3 (0.0f, 0.0f, 1.0f) * bulletImpulse;
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
		GameObject audioSourceObject = shieldPickUp.GetComponent<AudioSource> ().gameObject;
		audioSourceObject.SetActive (true);
		if (hasShieldOn) {
			gameController.scoringSystem.increaseScore ();
			return;
		}
		Destroy (shieldPickUp, 0.2f);
		showShield (true);
		StartCoroutine (endShield());
	}

	IEnumerator endShield() {
		if (gameController.scoringSystem.gameState != GameState.Playing) {
			showShield (false);
			yield break;
		}

		yield return new WaitForSeconds (shieldDuration);
		showShield (false);
	}

	public void showShield (bool value) {
		shield.SetActive (value);
		hasShieldOn = value;
	}


	// Demon Bullets:

	void demonHitPlayer(GameObject demonBullet) {
		gameController.shakeCamera ();
		Destroy (demonBullet);
		gameController.scoringSystem.reducePlayersHealth ();
	}
}
