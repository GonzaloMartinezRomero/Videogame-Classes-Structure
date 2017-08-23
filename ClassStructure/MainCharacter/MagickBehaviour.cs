using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;



public class MagickBehaviour : MonoBehaviour {

	//---Controlan la posicion del personaje y sus animaciones  
	public GameObject mainCharacter;
	private Animator animator;

	//---Controlar las animaciones de la camara al realizar las magias
	public GameObject mainCamera;
	private Animator animatorCamera;


	//-----Controles magia 1-----
	public GameObject magick1_GameObject;
	private Magick magick1;

	//---Controles magia 2-------
	public GameObject magick2_GameObject;
	private Magick magick2;

	//---Controles magia 3---------
	public GameObject magick3_GameObject;
	private Magick magick3;	 

	//----Controles magia 4------
	public GameObject magick4_GameObject;
	private Magick magick4; 

	//--- Referencia a los stats del personaje
	private Feature featureCharacter;



	// Use this for initialization
	void Start () {		
 
		magick1 = magick1_GameObject.GetComponent<Magick> ();
		magick2 = magick2_GameObject.GetComponent<Magick> ();
		magick3= magick3_GameObject.GetComponent<Magick> ();
		magick4 =magick4_GameObject.GetComponent<Magick> ();

		//---- Referencia al animador de camara
		animatorCamera = mainCamera.GetComponent<Animator> ();

		 //Animador del personaje
		animator = mainCharacter.GetComponent<Animator> ();

	 
		//Referencia a la clase feature para restar la magia consumida por las magias
		featureCharacter = GetComponent<Feature> ();
	 
	} 
	
	// Update is called once per frame
	void Update () {
		
		 
		if (magick2.isActiveMagick()) {
			
			magick2_GameObject.transform.Translate (Vector3.forward*Time.deltaTime*5.0f);
			 
		}  


	}

	/*
		Para que las animaciones tanto del personaje como de las particulas
		se realicen de forma sincronizada, hay que garantizarse que el personaje
		esta en la animacion de WalkRunAnim
		True si se puede ejecutar
	*/
	public bool executeMagick1(){
		
		if (!magick1.isActiveMagick() && animator.GetCurrentAnimatorStateInfo(0).IsName("WalkRunAnim") && (featureCharacter.consumeMagick (magick1.getSPConsume()))) {
			  
			magick1.activateMagick (featureCharacter.getDamageMagic(),false);	
			 
			animator.SetTrigger ("Magick1");
			animatorCamera.SetTrigger ("CameraMagick1");
			 
			featureCharacter.addDamageFisic (magick1.getDamage());

			return true;
		}
		return false;

	}

	public float getDurationMagick1(){		
		return magick1.getDuration();
	}

	public void revertEfectMagick1(){
		featureCharacter.addDamageFisic (-1*magick1.getDamage());
	}

	public bool executeMagick2(){
		
		if (!magick2.isActiveMagick() && animator.GetCurrentAnimatorStateInfo(0).IsName("WalkRunAnim")&& (featureCharacter.consumeMagick (magick2.getSPConsume()))) {
			 
			magick2_GameObject.transform.position=mainCharacter.transform.position;
			magick2_GameObject.transform.rotation = mainCharacter.transform.rotation;

			magick2.activateMagick (featureCharacter.getDamageMagic(),true);
				 
			animator.SetTrigger ("Magick2"); 

			return true;
		}
		return false;
	}
	public float getDurationMagick2(){
		return magick2.getDuration();
	}


	public bool executeMagick3(){
		
		if (!magick3.isActiveMagick() && animator.GetCurrentAnimatorStateInfo(0).IsName("WalkRunAnim")&& (featureCharacter.consumeMagick (magick3.getSPConsume()))) {
		
			magick3_GameObject.transform.position=mainCharacter.transform.position;

			magick3.activateMagick (featureCharacter.getDamageMagic(),true);

			animator.SetTrigger ("Magick3"); 
			animatorCamera.SetTrigger ("CameraMagick3");
			return true;
			
		}
		return false;
	
	}

	public float getDurationMagick3(){
		return magick3.getDuration();
	}



	public bool executeMagick4(){
	 
		if (!magick4.isActiveMagick() && animator.GetCurrentAnimatorStateInfo(0).IsName("WalkRunAnim")&& (featureCharacter.consumeMagick (magick4.getSPConsume()))) {
			 
			magick4.activateMagick (featureCharacter.getDamageMagic(),true);

			animator.SetTrigger ("Magick4");
			 
			return true;
		}
		return false;
	}

	public float getDurationMagick4(){
		return magick4.getDuration();
	}

}
