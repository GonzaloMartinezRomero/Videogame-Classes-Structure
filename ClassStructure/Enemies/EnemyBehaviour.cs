using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour {


	private EnemyFeature enemyFeature;

	private Animator animator;

	public Feature characterFeature; 

	[Tooltip("Permite orientar hacia la camara el texto del damage")]
	public Camera mainCamera;

	public GameObject textDamage;

	private float timeRemaining;

	private bool isDead;

	private StateMachine stateMachine;

	public MasterPillar masterPillar;

	public Mission1Controller controller;

	public AudioSource impactSound;

	 
	// Use this for initialization
	void Start () {

		enemyFeature = GetComponent<EnemyFeature> ();

		animator = GetComponent<Animator> ();

		timeRemaining = 2.0f;

		isDead = false;

		stateMachine = GetComponent<StateMachine> ();

		stateMachine.initStateMachine ();

	}
	
	 


	void OnTriggerEnter(Collider collision){
 
		if (!isDead) {
		
			if (collision.gameObject.tag == "Weapon" && MoveBehaviour.isAtack) {
				receiveDamage (characterFeature.getDamageFisic ());

			} else {
				
				if (collision.gameObject.tag == "Magick") {					
					receiveDamage (((collision.gameObject).GetComponent<Magick> ()).getDamage ());			
				} 

			}
		}
	}






	public void receiveDamage(int quantity){

		//Sonido del impacto
		//impactSound.Play();
		
		//Vida=Damage-Defensa
		int totalDamage = quantity - enemyFeature.getDefense();

		//Quitarle vida 
		enemyFeature.receiveDamage(totalDamage);

		//Mostrar por canvas del personaje el daño causado
		//Crear el texto
		GameObject tmpText=(GameObject)Instantiate(textDamage,gameObject.transform,false);

		//Orientarlo hacia la camara
		tmpText.transform.rotation = mainCamera.transform.rotation;

		//Sobrecargar el texto con el daño recibido
		(tmpText.GetComponentInChildren<Text>()).text=totalDamage.ToString();

		//Destruir objeto pasado un tiempo
		Destroy (tmpText,0.5f);

		if (enemyFeature.isDead()) {

			//Iniciar la animacion de muerte
			animator.SetTrigger ("Die");

			//Anadir la exp al main character
			characterFeature.addExperience (enemyFeature.getExp());

			//Indicar que esta muerto
			isDead = true;

			//Indicar a la maquina de estados que detenga su funcionamiento
			stateMachine.stopStateMachine ();

			controller.enemyIsDead ();

			//Destruir el objeto
			Destroy (gameObject,timeRemaining);

		} else {
			animator.SetTrigger ("Damage");

		}

	} 

	/*
		Realiza un damage al personaje
	*/
	public void atackCharacter(){
		
		//Comprobar que el propio agente no este muerto
		if (!isDead) {

			characterFeature.removeHP (enemyFeature.getDamage ());
			//animator.SetBool ("Atack",true);
			//impactSound.Play ();
		
		}


	}

	 
	public void atackMasterPillar(){
		
		masterPillar.setDamage (enemyFeature.getDamage());
		//Activar la animacion de ataque

	}

	 

}
