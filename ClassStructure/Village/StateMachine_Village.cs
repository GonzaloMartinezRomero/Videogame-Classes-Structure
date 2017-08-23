using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine_Village : MonoBehaviour {

	//----- Estados Del Agente ------
	public StateGoTo[] position;
	public StateIdle stateIdle;
	public StateGoTo goToEnemy;
	public StateAtack atackEnemy;

	//Estado actual del agente
	private State currentState;
	 
	public MasterPillar masterPillar;

	[Tooltip("Distancia a partir de la cual empieza a atacar")]
	public float atackDistance;

	//---- Check Points ----
	private int numberOfPositions;
	private int lastPosition;  

	//Indica si el estado de reposo esta activo
	private bool isIdleModeActive;

	//Enemigo al que atacar
	private GameObject enemy;

	//Indica si la maquina esta en funcionamiento
	private bool isMachineRunning;

	// Use this for initialization
	void Start () {		 
		 
		enemy = null;

		currentState = stateIdle;

		lastPosition = -1;

		numberOfPositions = 0;

		foreach(StateGoTo goTo in position){
			numberOfPositions++;
		}

		isIdleModeActive = false;

		isMachineRunning = false;
	}

	void OnDestroy(){
		CancelInvoke ("runStateMachine");
	}

	void OnTriggerEnter(Collider collider){

		//En caso de entrar en un checkpoint me quedo parado
		if (collider.tag == "CheckPoint") {			

			currentState.stopState ();
			currentState = stateIdle;
			stateIdle.startState ();
		} 

	}


	public void initStateMachine(){

		if (!isMachineRunning) {
		
			setIdleMode ();
			isMachineRunning = true;

		}	

	}



	//Asigna el modo alerta 
	private void setAlertMode(){
	
		if(isIdleModeActive){
			
			CancelInvoke ("runStateMachine");
			InvokeRepeating ("runStateMachine",0.0f,0.25f);
			isIdleModeActive = false;
		}



	}

	//Asigna el modo idle
	private void setIdleMode(){
	
		if (!isIdleModeActive) {
			
			CancelInvoke ("runStateMachine");
			InvokeRepeating ("runStateMachine",0.0f,UnityEngine.Random.Range(10.0f,30.0f));
			isIdleModeActive = true;

		}


	}




	private bool checkDistance(float distance,Vector3 dest){

		//VecorDestino-VectorOrigen
		Vector3 dist=dest-gameObject.transform.position;
		 
		return distance > dist.magnitude;

	}

	/*
		Genera un numero aleatorio dentro del rango de posiciones 
		distintas a la generada anteriormente
	*/
	private int randomPosition(){

		//Generar numero aleatorio
		int currentPosition = UnityEngine.Random.Range (0, numberOfPositions);


		//Si la ultima posicion es la misma, avanzo a la siguiente
		if (currentPosition == lastPosition) {

			currentPosition = (currentPosition + 1)% numberOfPositions;

		} 


		lastPosition = currentPosition;

		return currentPosition;
	}


	//Estado de defender el pilar
	private bool defendPillar(){	

		//En caso de que el master pillar este siendo atacado
		if (masterPillar!=null && masterPillar.isBeingAtacked()) {

			//Hay que asignarle un enemigo al que atacar
			if (enemy == null) {

				//Se solicita un enemigo
				enemy = masterPillar.getEnemy ();
			 
				//Si no hay disponibles, no se ataca
				if (enemy == null)							
					return false;
			}
			 
			//Activa el modo de alerta
			this.setAlertMode ();

			//Distancia me permite atacar al enemigo
			if (checkDistance (atackDistance, enemy.transform.position)) {

				if (!atackEnemy.isStateActive ()) {

					atackEnemy.setEnemyPosition (enemy);
					currentState.stopState ();
					currentState = atackEnemy;
					atackEnemy.startState ();

				}

			} else {

				//En caso contrario voy a por el

				if(!goToEnemy.isStateActive()){
					
					goToEnemy.setDestination(enemy);
					goToEnemy.setCharacterSpeed (4.0f);
		
					currentState.stopState ();
					currentState = goToEnemy;
					goToEnemy.startState ();
					 

				}

			}

			return true;
		} 

		return false;
	}



	private void runStateMachine(){

		//Si no puedo defender el masterpillar me quedo en estado idle
		if(!defendPillar()){

			this.setIdleMode ();	

			currentState.stopState ();
			currentState = position [randomPosition ()];
			currentState.startState ();
			 

		}
		 
		 
	}

	//Detiene el funcionamiento de la maquina de estados
	public void stopStateMachine(){

		if(isMachineRunning){
			
			CancelInvoke ("runStateMachine"); 
			currentState.stopState ();
			isMachineRunning = false;

		}

	}
	
	 
}
