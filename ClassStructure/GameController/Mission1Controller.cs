using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mission1Controller : MonoBehaviour {


	[Tooltip("Enemy Prefab")]
	public GameObject enemyPrefab;

	[Tooltip("Zona de respawn")]
	public Transform[] enemyRespawn;
	private int countRespawnZones;

	[Tooltip("Numero total de enemigos a generar")]
	public int totalEnemies;

	//Numero de enemigos que quedan por generar
	private int enemyNumberNotGenerated;

	[Tooltip("Numero de enemigos que pueden estar al mismo tiempo en escena")]
	public int maxCurrentEnemy;

	//Numero de enemigos que estan actualmente en la escena
	private int currentEnemy;	 



	[Tooltip("Tiempo de respawn")]
	public float respawnTime;

	//Numero de enemigos eliminados
	private int numberEnemyDead;

	[Tooltip("Personaje principal")]
	public GameObject mainCharacter;
	private MoveBehaviour moveControllerCharacter;

	[Tooltip("Canvas del personaje")]
	public MainCanvas canvas;

	[Tooltip("Camara encargada de realizar la cinematica")]
	public GameObject cinematicCamera;

	[Tooltip("Audio de la cinematica")]
	public AudioSource cinematicAudio;

	[Tooltip("Audio por defecto")]
	public AudioSource defaultAudio;

	//Tiempos de la animacion 
	[Tooltip("Duracion de la animacion")]
	public float animationDuration;
	private float currentTime;

	//Controladores de estado de la animacion
	private bool isAnimationActive;
	private bool isAnimationDone;

	[Tooltip("Luz principal")]
	public GameObject mainLight;
	private Animator lightAnimator;

	public static Mission1Controller instance = null;

	//Singletone pattern
	void Awake(){
		
		if (instance == null) {
			
			instance = this;

		} else {
			
			if(instance!=this){
				
				Destroy (gameObject);
			}
		}
	}

	// Use this for initialization
	void Start () {


		countRespawnZones = 0;

		//Establecer las zonas de respawn
		foreach(Transform transformAux in enemyRespawn){
			countRespawnZones++;
		}

		moveControllerCharacter = mainCharacter.GetComponent<MoveBehaviour> ();
		lightAnimator = mainLight.GetComponent<Animator> ();
	 	isAnimationActive = false;
		isAnimationDone = false;
		currentTime = 0.0f;
		cinematicAudio.Stop ();
		numberEnemyDead = 0;
		 
		enemyNumberNotGenerated = totalEnemies;
		currentEnemy = 0;

	}

	void Update(){

		if(isAnimationActive){

			currentTime += Time.deltaTime;

			if (currentTime > animationDuration || Input.GetButton("PickUp")) {
				
				mainCharacter.SetActive(true);
				canvas.setActivation (true);
				cinematicCamera.SetActive(false);
				isAnimationActive = false;

			}

		}
		
	}


	 
	void OnTriggerEnter(Collider collision){
	
		if((!isAnimationDone)&&(collision.tag=="Player")){

			//Iniciar cinematica

			mainCharacter.SetActive(false);
			canvas.setActivation (false);
			cinematicCamera.SetActive(true);

			cinematicAudio.Play ();

			defaultAudio.Pause ();

			canvas.setTextAdvert ("MISION 1: Defender El Pilar",25.0f);


			//Evita repetir la cinematica por cada vez que el personaje pase por la zona
			isAnimationDone = true;
			isAnimationActive = true;

			//Comenzar generacion de enemigos
			InvokeRepeating("newEnemy",2.0f,respawnTime);
		}

	}

	 

	private void newEnemy(){

		if (enemyNumberNotGenerated > 0) {

			if (currentEnemy < maxCurrentEnemy) {

				//Zona aleatoria de respawn
				Transform tranformAux=enemyRespawn[Random.Range(0,countRespawnZones)];
				Instantiate (enemyPrefab,tranformAux.position ,tranformAux.rotation);
				enemyNumberNotGenerated--;

				//Evita tener mas que currentEnemy enemy en la escena
				currentEnemy++;
			
			}



		} else {

			CancelInvoke ("newEnemy");
			  
		}

	}

	//Indica que el pilar ha sido destruido y la mision ha fracasado
	public void pillarDestroy(){
	
		canvas.setTextAdvert ("Mision Fracasada: Han destruido el pilar");
		lightAnimator.SetTrigger ("RedLight");
		Invoke ("loadFailedMissionScene", 4.0f);
	}

	public void characterIsDead(){
		
		canvas.setTextAdvert ("Mision Fracasada: Has Muerto");
		moveControllerCharacter.isDead ();
		lightAnimator.SetTrigger ("RedLight");
		Invoke ("loadFailedMissionScene", 4.0f);
	}

	private void loadFailedMissionScene(){		  
		SceneManager.LoadSceneAsync ("SceneFailed", LoadSceneMode.Single);
	}


	public void enemyIsDead(){
		
		numberEnemyDead++;
		currentEnemy--;

		//Indica que has eliminado a todos los enemigos
		if(numberEnemyDead==totalEnemies){
 
			defaultAudio.Play ();
			cinematicAudio.Stop ();
			canvas.setTextAdvert("Misión finalizada, ve hacia el portal ", 5.0f);

		}
	
	}
	  

}
