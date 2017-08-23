using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndedCanvas : MonoBehaviour {

	public Button restart;
	public Button exit;


	// Use this for initialization
	void Start () {

		restart.onClick.AddListener (restartGame);
		exit.onClick.AddListener (exitGame);


	}


	public void restartGame(){

		SceneManager.LoadSceneAsync ("Mission1");

	}

	public void exitGame(){
		Application.Quit ();
	}



}
