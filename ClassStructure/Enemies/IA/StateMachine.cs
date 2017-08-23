using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine : MonoBehaviour {

	//---- Estados Del Agente ----
	public StateIdle stateIdle;
	public StateGoTo stateGoToPillar;
	public StateGoTo stateGoToCharacter;
	public StateAtack stateAtackCharacter;
	public StateAtack stateAtackPillar;
	public StateAtack stateAtackVillage;
	 
	  
	//Posicion del enemigo
	public Transform mainCharacterTransform;

	//Posicion del master pillar
	public Transform masterPillarTransform;
	 
	[Tooltip("Distancia a partir de la cual se inicia el state de ataque hacia el peronaje")]
	public float atackDistanceCharacter;

	[Tooltip("Distancia a la que el enemigo puede ver al main character")]
	public float rangeOfView;

	//Estado que acutalmente se esta ejecutando
	private State currentState;

	[Tooltip("Distancia a partir de la cual comienza a lanzar rayos para mayor precision")]
	public float rayCastDistance;

	[Tooltip("Veces por segundo que se refresca para ver el estado actual")]
	public float refreshFrecuency;

	[Tooltip("Distancia a la que empieza a atacar al masterPillar")]
	public float atackDistancePillar;

	//-----Ray casting-----
	private RaycastHit rayHit;
	private Vector3 directionMainCharacter;
	private Vector3 originRay;

	//Indica si el rayo ha colisionado
	private bool isHit;

	//Indica si la maquina de estados esta en marcha
	private bool isMachineRunning;

	//---Identifican al village que tienen que atacar
	private int maxVillageCounter;
	private Queue<GameObject> villageQueue;



	// Use this for initialization
	void Start () {

		isMachineRunning = false;
		 
		maxVillageCounter = 5;
		villageQueue = new Queue<GameObject> (maxVillageCounter);
		  
	}


	void OnDestroy(){
		CancelInvoke ("startStateMachine");
	}

	void OnTriggerEnter(Collider collider){
		
		//Si detecto a un village dentro de mi collider, lo ataco
		if(collider.gameObject.tag=="Village"){
			
			if(villageQueue.Count<(maxVillageCounter-1))
				villageQueue.Enqueue (collider.gameObject);

		}

	}


	//Inicia el funcionamiento de la maquina de estados
	public void initStateMachine(){
	
		if(!isMachineRunning){

			//Intervalo de tiempo en la que se consulta el estado de la maquina
			InvokeRepeating ("startStateMachine",0.0f,refreshFrecuency);

			currentState = stateIdle;

			isMachineRunning = true;
		}
		
	}


	 
	/*
		Si se cumplen las condicinones para atacar al village return true
	*/
	private bool atackVillage(){

		//Comprobar que hay village a los que atacar
		if (villageQueue.Count>0) {

			GameObject villageAux = villageQueue.Peek ();

			//Comprobar que no han sido ya eliminados
			if (villageAux == null){
				
				villageQueue.Dequeue ();
				return false;
			}
				

			if(!stateAtackVillage.isStateActive()){

				currentState.stopState ();

				stateAtackVillage.setEnemyPosition (villageAux);

				currentState = stateAtackVillage;
		
				currentState.startState ();

			}

			return true;
		}

		return false;
	}


	 
	/*
		Return true si la distancia maxima es sobrepasada por el
		objeto situado en dest
	*/
	private bool checkDistance(float distance,Vector3 dest){

		//Distancia=(VecorDestino-VectorOrigen)
		Vector3 dist=dest-gameObject.transform.position;
		 
		return distance > dist.magnitude;
	}


	/*
		Si las condiciones para atacar al character son las adecuadas devuelve true  
	*/
	private bool atackCharacter(){
	
		//Se comprueba la distancia respecto al personaje principal
		if (checkDistance (rayCastDistance,mainCharacterTransform.position)) {

			//Actualizo el rayo para que la direccion sea la correcta
			originRay = transform.position;
			originRay.y += 1;
			directionMainCharacter = 1.1f * (mainCharacterTransform.position - transform.position); 

			//Debug.DrawRay (originRay,directionMainCharacter);

			//Lanzo el rayo
			isHit = Physics.Raycast (originRay, directionMainCharacter, out rayHit, rangeOfView);

			//Ha dado al personaje principal
			if (isHit && (rayHit.transform.tag == "Player")) {
				
				//La distancia contra el personaje me permite atacarlo
				if (rayHit.distance < atackDistanceCharacter) {

					if(!stateAtackCharacter.isStateActive()){

						currentState.stopState ();
						currentState = stateAtackCharacter;
						stateAtackCharacter.startState ();
					}

				} else {

					//Estoy viendo al peronaje a una distancia adecuada para ir tras el pero no para atacarlo
					if (!stateGoToCharacter.isStateActive ()) {

						currentState.stopState ();
						currentState = stateGoToCharacter;
						stateGoToCharacter.startState ();

					}

				}

				return true;
			}


		}  

		return false;

	}

	// Return true si las condiciones para atacar al masterpillar son las adecuadas
	private bool atackMasterPillar(){

		//Master pillar no ha sido destruido
		if (masterPillarTransform != null) {		

			//Distancia correcta para atacar al pilar maestro
			if (checkDistance (atackDistancePillar, masterPillarTransform.position)) {
				
				if (!stateAtackPillar.isStateActive ()) {

					currentState.stopState ();
					currentState = stateAtackPillar;
					stateAtackPillar.startState ();

				}

			} else {
			

				//Tengo que ir a por el pilar
				if (!stateGoToPillar.isStateActive ()) {

					currentState.stopState ();
					currentState = stateGoToPillar;
					stateGoToPillar.startState ();

				}


			}	

			return true;
		}

		return false;

	}



	private void startStateMachine(){

		//Comprobar el ataque hacia el village
		if (!atackVillage ()) {

			//Comprobar el ataque hacia el character
			if(!atackCharacter()){

				//Comprobar el ataque hacia el masterPillar
				if (!atackMasterPillar()) {

					//Modo idle si no ha podido hacer nada de lo anterior
					if (!stateIdle.isStateActive ()) {

						currentState.stopState ();
						currentState = stateIdle;
						stateIdle.startState ();

					}

				}

			}

		}
 
	}

	//Detiene el funcionamiento de la maquina de estados 
	public void stopStateMachine(){

		if(isMachineRunning){
			
			CancelInvoke ("startStateMachine");
			currentState.stopState ();
			isMachineRunning = false;
			  
		}

	}
	 
}
