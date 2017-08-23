using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {


	//Propiedades de camara
	private Camera cam;
	private Animator animator;
	public int maxZoom;
	public int minZoom;
	public int valueZoom;

 	//Evitan estar continuamente asignando el valor false a la animacion
	private bool turnCameraR;
	private bool turnCameraL;


	// Use this for initialization
	void Start () {		
		cam = GetComponent<Camera> ();		
		animator = GetComponent<Animator> ();
		turnCameraR = turnCameraL= false;
	}
	
	// Update is called once per frame
	void Update () {

		//----------Funciones para hacer scroll con el raton para aplicar el zoom--------
	 
		if (Input.GetAxis ("ScrollMouse") > 0.0f) {
			
			cam.fieldOfView = (cam.fieldOfView<maxZoom) ? cam.fieldOfView+valueZoom : cam.fieldOfView;
		} else {
			
			if(Input.GetAxis ("ScrollMouse") < 0.0f){
				cam.fieldOfView = (cam.fieldOfView>minZoom) ? cam.fieldOfView-valueZoom : cam.fieldOfView;
			}

		}
		 
		//------------- Funciones para hacer girar la camara hacia derecha o izquierda -------- 
		if (Input.GetButton ("CameraTurnLeft")) {
			
			executeTurnCamera ("CameraTurnLeft", true);			 
			turnCameraL = true;

		} else {
			
			if (turnCameraL) {
				
				executeTurnCamera ("CameraTurnLeft", false);
				turnCameraL = false;
			}

		
		}


		if (Input.GetButton ("CameraTurnRight")) {

			executeTurnCamera ("CameraTurnRight", true);
			turnCameraR = true;

		} else {
			if (turnCameraR) {
				
				executeTurnCamera ("CameraTurnRight", false);
				turnCameraR = false;
			}
		}


	
	}



	private void executeTurnCamera(string animationName,bool state){
		
		animator.SetBool (animationName, state);
	}

	 

}
