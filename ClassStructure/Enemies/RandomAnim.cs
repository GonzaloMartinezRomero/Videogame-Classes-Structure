using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnim : MonoBehaviour {

	public Animator animator;
	public string[] animatorName;


	// Use this for initialization
	void Start () {
			
		int numberOfAnimation = 0;

		foreach(string str in animatorName){
			numberOfAnimation++;
		}


		animator.SetTrigger(animatorName[Random.Range (0,numberOfAnimation)]);

	}



}
