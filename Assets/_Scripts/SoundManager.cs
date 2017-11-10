using UnityEngine;
using System.Collections;

// I used a tutorial from Ray Wenderlich's book on Sound Managers to create this script and changed it a bit

public class SoundManager : MonoBehaviour {
	/*
	public static SoundManager Instance = null; // a single instance of sound manager
	private AudioSource audioSource;

	// Sounds:
	public AudioClip shieldPickedUp;
	public AudioClip explosion;
	public AudioClip bulletShoot;
	public AudioClip levelUpgrade;
	public AudioClip pop;

	void Start () {
		ensureNoCopies ();
		loadSounds ();
	}

	void ensureNoCopies() {
		// ensure there is only one copy of this object:
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}
	}

	void loadSounds() {
		AudioSource[] sources = GetComponents<AudioSource>();
		foreach (AudioSource source in sources) {
			if (source.clip == null) {
				audioSource = source;
			}
		}
	}
	
	public void PlayOneShot(AudioClip clip) {
		audioSource.PlayOneShot(clip);
	}
	*/
}
