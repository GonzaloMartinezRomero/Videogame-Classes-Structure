using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour {

	//Variables para controlar la velocidad del personaje
	public float velocityRun;
	public float velocityWalk;
	public float velocityReverse;
	public float velocityTurn;
	public float mouseSensibility;

	//----MainCamera----
	public GameObject mainCamera;

	//--Audio andar
	public AudioSource walkingAudio;
	private bool isWalking;

	//--Audio ataque
	public AudioSource atackAudio;

	private Animator animatorCharacter;
	private Animator animatorCamera;

	//Representan el estado
	//private float run;
	private float idle;
	private float walk;

	  
	//Evita tener que estar asignando en cada frame false a la animacion 
	private bool isActiveProtect;
	public static bool isAtack;

	private bool characterIsDead;


	// Use this for initialization
	void Start () {
		
		this.animatorCharacter = GetComponent<Animator> ();
		//this.run = 1.0f;
		this.idle = 0.0f;
		this.walk = 0.5f;
	 
		animatorCamera=mainCamera.GetComponent<Animator> ();
		isActiveProtect = false;
		isAtack = false;

		isWalking = false;

		characterIsDead = false;
		 
		 
	}
	
	// Update is called once per frame
	void Update () {

		if (!characterIsDead) {


			//---------------Comprobar si estoy atacando------------------
			if (Input.GetButton("Atack")) {	

				actionAtack ("Atack",true);
				isAtack = true;

				if(!atackAudio.isPlaying)
					atackAudio.Play ();


			}else {
				
				if (isAtack) {
					
					actionAtack ("Atack",false);
					isAtack = false;
					atackAudio.Stop ();
				
				}

			}
				
			//---------------Comprobar la formacion de proteccion------------------
			if (Input.GetButton("Protect")) {
				
				actionProtect ("Protect","CameraProtect",true);
				isActiveProtect = true;

			}else {
				
				if (isActiveProtect) {
					
					actionProtect ("Protect","CameraProtect",false);
					isActiveProtect = false;
				}

			}

			//---------------Comprobar el desplazamiento del personaje------------------

			isWalking = false;

			if (Input.GetButton ("Run")) {
			
				if (Input.GetButton ("Walk")) {

					actionMove ("SpeedCharacter","CameraMove", this.walk, this.velocityWalk); 
					 
				} else {
					
					actionMove("SpeedCharacter","CameraMove", Input.GetAxis("Run"), Input.GetAxis("Run")*this.velocityRun);

					isWalking = true;
				}


			} else { 
				
				actionMove ("SpeedCharacter", "CameraMove",this.idle,0.0f);  
				 
			}

			//--- Comprobacion del sonido al andar
			if (isWalking && !isAtack) {
				
				if (!walkingAudio.isPlaying)
					walkingAudio.Play ();
				
			} else {
				
				if (walkingAudio.isPlaying)
						walkingAudio.Stop ();
			
			}


						
				 
			//---------------Comprobar el desplazamiento hacia los lados mediante teclado------------------

			if (Input.GetButton("Rotate")) {		

				if (Input.GetAxis ("Rotate") > 0.0f) {
					actionRotate (this.velocityTurn,false);					 
				} else {
					actionRotate (this.velocityTurn,true);		
				}

			} 


			//---------------Comprobar el desplazamiento hacia los lados mediante raton------------------
			if(Input.GetButton("Mouse X")){
				 
				actionRotate (Input.GetAxis ("Mouse X")*mouseSensibility,false);		 

			}

		}

	}


	/*
		Realiza la accion de ataque 
	*/
	private void actionAtack(string animationCharacter,bool state){
	
		animatorCharacter.SetBool (animationCharacter, state);
	
	}

	/*
		Realiza la accion de proteccion
	*/
	private void actionProtect(string animationCharacter,string animationCamera,bool state){
	
		animatorCharacter.SetBool (animationCharacter, state);
		animatorCamera.SetBool (animationCamera,state);
	
	}

	/*
		Realiza la accion de correr
		animationCharacter nombre de la animacion
		valueRun valor de transicion de la animacion
		velocityRun velocidad de avance del personaje
	*/
	private void actionMove(string animationCharacter,string animationCamera,float valueRun,float velocityRun){
		
		/*
				Comprueba que el personaje no esta realizando otra animacion que no sea la de movimiento.
				Evita, por ejemplo, que mientras el personaje este atacando se desplace sin realizar la
				animacion del movimiento
		*/

		if(animatorCharacter.GetCurrentAnimatorStateInfo (0).IsName("WalkRunAnim")){

			animatorCamera.SetFloat (animationCamera,valueRun);
			animatorCharacter.SetFloat (animationCharacter, valueRun);
			transform.Translate (Vector3.forward * Time.deltaTime * velocityRun);

		}

	}


	/*
		Realiza la accion de giro
		velocityTurn es la cantidad de desplazamiento angular
		direction True para sentido antihorario, False para contrario
	*/
	private void actionRotate(float velocityTurn,bool direction ){
		
		float dir = direction ? -1.0f : 1.0f;
		 
		transform.Rotate ((Vector3.up * velocityTurn*dir)*Time.deltaTime);	
	
	}

	public void isDead(){

		characterIsDead = true;
		isAtack = false;

		animatorCharacter.SetTrigger ("IsDead");
	
	}



}
