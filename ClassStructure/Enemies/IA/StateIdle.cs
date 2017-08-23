using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : MonoBehaviour,State {

	private Animator animator;
	public string nameAnim;
	private bool isActive;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		isActive = false;

	}



	public void startState(){

		isActive = true;	 	
		animator.SetBool (nameAnim, true);

	}

	public void stopState(){
		isActive = false;
		animator.SetBool (nameAnim, false);
	}


	public bool isStateActive(){
		return isActive;
	}



}
