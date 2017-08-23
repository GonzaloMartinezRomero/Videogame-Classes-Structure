using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SchedulerScene : MonoBehaviour {

	private AsyncOperation asyncOp;

	// Use this for initialization
	void Start () {
		
		asyncOp=SceneManager.LoadSceneAsync ("SceneStart",LoadSceneMode.Single);
		asyncOp.allowSceneActivation = false;

		InvokeRepeating ("loadScene",1.0f,2.0f);
	}


	private void loadScene(){
		if (asyncOp.progress > 0.8f) {
			
			CancelInvoke ("loadScene");
			asyncOp.allowSceneActivation = true;

		}
	}
	 
	
	 
}
