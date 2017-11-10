using UnityEngine;
using System.Collections;

public class Shoot : StateMachineBehaviour {

}



/*
	using UnityEngine;
	using System.Collections.Generic;
	using System.Collections;


	public class GameController : MonoBehaviour {

		// ...
		private List<GameObject> demonsOnScreen;
		// ...

		void Start () {
			demonsOnScreen = new List<GameObject> ();
			//...
		}

		void instantiateDemon(Vector3 spawnPosition, Quaternion spawnRotation) {
			GameObject demonInstance = Instantiate (demon, spawnPosition, spawnRotation) as GameObject;
			demonsOnScreen.Add (demonInstance);
		}

		void removeDemonFromArray(GameObject demonInstance) {
			demonsOnScreen.Remove (demonInstance);
		}

		public IEnumerator SpawnAsteroids () {
			//...
		}
	}
*/