using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MissionStart : MonoBehaviour {

	public Text text;

	[Tooltip("Contenido de las frases")]
	public string [] phrase;

	[Tooltip("Tiempo de cambio entre frases")]
	public float changeTime;

	[Tooltip("Tiempo de espera hasta la carga de la siguiente escena")]
	public float waitingTime;

	private int numberOfPhrase;
	private int currentPhrase;

	private AsyncOperation asyncOp;



	void Awake(){
		
		asyncOp=SceneManager.LoadSceneAsync ("Mission1",LoadSceneMode.Single);
		asyncOp.allowSceneActivation = false;
		
	}


	// Use this for initialization
	void Start () {
		 

		numberOfPhrase = 0;
		currentPhrase = 0;

		foreach(string str in phrase){
			numberOfPhrase++;
		}

		InvokeRepeating ("newText",2.0f,changeTime);

		Invoke ("chargeMainScene",waitingTime);


	}

	void OnDestroy(){
		CancelInvoke ("newText");
	}


	private void newText(){
	
		if (currentPhrase < numberOfPhrase) {

			text.text = phrase [currentPhrase];
			currentPhrase++;	

		} else {
			text.text = " ";
			CancelInvoke ("newText");
		}

	}

	private void chargeMainScene(){		
		/*
			Va cargando la escena y cuando este lista la lanza en lugar de 
			detener completamente el juego
		*/

		asyncOp.allowSceneActivation = true;
		//SceneManager.LoadSceneAsync ("Mission1",LoadSceneMode.Single);

	}


}
