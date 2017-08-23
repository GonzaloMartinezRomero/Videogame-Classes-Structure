using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBehaviour : MonoBehaviour {

	private Animator animator;
	private StateMachine_Village stateMachine;
	public int hp;
	private bool isDead;

	// Use this for initialization
	void Start () {
		
		animator=GetComponent<Animator> ();
		stateMachine = GetComponent<StateMachine_Village> ();
		stateMachine.initStateMachine ();
		isDead = false;
	}

	public void setDamage(int quantity){

		hp -= quantity;

		if (hp < 0 && !isDead) {
			
			stateMachine.stopStateMachine ();
			animator.SetTrigger ("Dead");
			Destroy (gameObject,3.0f);
			isDead = true;
		}

	}

	 

}
