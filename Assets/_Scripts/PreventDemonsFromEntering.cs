using UnityEngine;
using System.Collections;

public class PreventDemonsFromEntering : MonoBehaviour {
	public GameController gameController;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Demon")
			gameController.demonEnteredPlayersShiels (other.gameObject);
	}
}
