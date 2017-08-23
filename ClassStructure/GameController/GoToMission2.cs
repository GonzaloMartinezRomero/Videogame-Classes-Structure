using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMission2 : MonoBehaviour {

	public MainCanvas mainCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter(Collider collider){

		if(collider.tag=="Player"){
			mainCanvas.setTextAdvert (" PARTE NO IMPLEMENTADA =( ",4.0f);

		}
	
	}
}
