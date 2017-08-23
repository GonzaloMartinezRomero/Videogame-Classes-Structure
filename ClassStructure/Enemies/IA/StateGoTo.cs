using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateGoTo : MonoBehaviour,State {

	private Animator animator;

	public string nameAnim;

	private bool isActive;

	private NavMeshAgent navMeshAgent;

	//El destino tiene que ser gameobject porque si es transform puede verse modificado y dar posicion erronea
	public GameObject destination;

	[Tooltip("Frecuencia con la que se actualiza la funcion para buscar ")]
	public float refreshFrecuency;

	private float defaultSpeed;



	// Use this for initialization
	void Start () {
		
		isActive = false;
		navMeshAgent = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();
		defaultSpeed = navMeshAgent.speed;

	}



	public void startState(){
		
		isActive = true;

		animator.SetBool (nameAnim,true);

		InvokeRepeating ("searchEnemy",0.0f,refreshFrecuency);


	}

	public void stopState(){
		
		isActive = false;
		animator.SetBool (nameAnim,false);
		CancelInvoke ("searchEnemy");
		navMeshAgent.speed = defaultSpeed;	
		navMeshAgent.Stop();		

	

	}


	public bool isStateActive(){
		return isActive;
	}

	private void searchEnemy(){

		if (destination != null) {		

			navMeshAgent.ResetPath ();
			navMeshAgent.SetDestination(destination.transform.position);

		}
		 
	}

	/*
		En caso de que el objeto sea destruido antes de detener el estado
		se debe cancelar la corutina para evitar referencias a un metodo 
		que no existe
	*/
	void OnDestroy(){
		
		CancelInvoke ("searchEnemy");
	}


	public void setDestination(GameObject gameObjectDestiny){
		
		destination = gameObjectDestiny;	
	}

	public void setCharacterSpeed(float velocity){
		
		navMeshAgent.speed = velocity;
	
	}
	 
}
