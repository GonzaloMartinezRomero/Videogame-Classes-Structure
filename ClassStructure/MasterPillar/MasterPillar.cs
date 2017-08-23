using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MasterPillar : MonoBehaviour {

	private bool isAtacked;
	public Mission1Controller controller;
	public int hp;
	private Queue<GameObject> enemyQueue;

	//Numero maximo de enemigos que pueden ser anadidos a la cola
	private int maxElements;

	// Use this for initialization
	void Start () {

		maxElements = 15;

		isAtacked = false;
		enemyQueue = new Queue<GameObject> (maxElements);
	}

	public void setDamage(int quantity){

		hp -= quantity;
	
		if (hp < 0) {
			Destroy (gameObject);
			controller.pillarDestroy ();
		}
	}

	public bool isBeingAtacked(){
		return isAtacked;
	}


	void OnTriggerEnter(Collider collider){
		 
		if (collider.tag == "Enemy") {

			isAtacked = true;

			/*
				Para evitar el resize O(n) se dejan de meter 
				elementos a partir de max-1 para mantener
				las insercciones en O(1)
			*/
			if (enemyQueue.Count < (maxElements - 1)) {
				
				enemyQueue.Enqueue(collider.gameObject);				 
			
			}
				

		}

	}

	public GameObject getEnemy(){

		try{

			return enemyQueue.Dequeue ();
				
		}catch(Exception){
			//Caso de que la cola este vacia
			return null;			 
		}

	}



}
