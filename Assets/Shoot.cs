using UnityEngine;
using System.Collections;

public class Shoot : StateMachineBehaviour {
	//public Demon demonScript;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Demon demon = GameObject.FindGameObjectWithTag("Demon").GetComponent<Demon> ();
		//demon.shootBullet ();
	}
}
