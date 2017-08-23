using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class StateAtack : MonoBehaviour,State{

	private bool isActive;
	private Animator animator;
	public string nameAnim;

	[Tooltip("Frecuencia con la que se refresca el estado de ataque")]
	public float atackFrecuency;

	 
	private EnemyFeature enemyFeature;
	private EnemyBehaviour enemyBehaviour;

	public GameObject enemyPosition;

	[Tooltip("Distancia a partir de la cual empieza el ataque")]
	public float atackDistance;

	public enum EnemyType{Character,Pillar,Village,Enemy};
	public EnemyType enemyType;

	private bool isAnimationActive;


	// Use this for initialization
	void Start () {

		isAnimationActive = false;

		animator = GetComponent<Animator> ();

		isActive = false; 
		 	
		enemyFeature = GetComponent<EnemyFeature> ();
	 	 
		enemyBehaviour = GetComponent<EnemyBehaviour> ();		 
	
	}
	


	public void startState(){

		isActive = true;	 	

		InvokeRepeating ("realizeAtack",0.0f,atackFrecuency);

	}

	private bool checkDistance(float distance){

		if(enemyPosition!=null){

			//VecorDestino-VectorOrigen
			Vector3 dist=enemyPosition.transform.position-gameObject.transform.position;

			return distance > dist.magnitude;
		}

		return false;

	}

	private void realizeAtack(){

		//Compruebo que la distancia es la correcta para poder atacar
		if (checkDistance (atackDistance)) {

			//Asignar la rotacion 

			/*
				NO SE DEBERIAN DE ACTIVAR LAS ANIMACIONES, YA QUE LAS DEBE
				HACER EL PROPIO AGENTE CUANDO SE LLAME A SU FUNCION
			*/
			//En caso de no estar activa la animacion, se activa
			if (!isAnimationActive){
				animator.SetBool (nameAnim, true);
				isAnimationActive = true;
			}				


			//Realiza ataque al personaje principal
			if (enemyType == EnemyType.Character && enemyBehaviour!=null) {
				
				enemyBehaviour.atackCharacter ();

			} else {

				//Realiza ataque al pilar
				if (enemyType == EnemyType.Pillar && enemyBehaviour!=null) {
					
					enemyBehaviour.atackMasterPillar ();

				} else {
				
					//Realizar ataque al enemigo
					if (enemyType == EnemyType.Enemy) {
						/*
							Situacion en las que village ataca al enemigo							
						*/
						 
					} else {

						//Realizar ataque a los village
						if(enemyType == EnemyType.Village && enemyPosition!=null){
							 
							(enemyPosition.GetComponent<VillageBehaviour>()).setDamage(enemyFeature.getDamage());

						}
					
					}
				
				}
			} 
				
		} else {
			
			if (isAnimationActive) {
				animator.SetBool (nameAnim, false);
				isAnimationActive = false;
			}
		}
		 
	}



	public void stopState(){
		
		isActive = false;
		animator.SetBool (nameAnim,false);
		isAnimationActive = false;
		CancelInvoke ("realizeAtack");

	}


	public bool isStateActive(){
		return isActive;
	}

	void OnDestroy(){
		CancelInvoke ("realizeAtack");
	}


	public void setEnemyPosition(GameObject gameObjectEnemy){
		enemyPosition = gameObjectEnemy;
	}

}
