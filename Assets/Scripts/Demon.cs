using UnityEngine;
using System.Collections;

public class Demon : MonoBehaviour {

	public Transform playerPos;

	void Update () {
		this.gameObject.transform.LookAt (playerPos);
	}
}
