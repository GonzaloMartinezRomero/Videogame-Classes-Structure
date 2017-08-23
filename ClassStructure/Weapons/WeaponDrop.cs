using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour {
	
	[Tooltip("Posicion dentro de la jerarquia del personaje donde la arma va a ser agregada")] 
	public Transform characterWeapons;

	[Tooltip("Prefab del arma")] 
	public GameObject weaponPrefab;

	//Copia del arma que se le asigna al personaje
	private GameObject weaponGameObject;

	//Class weapon asociada al arma
	private Weapon weaponReference;

	[Tooltip("Canvas donde va a aparecer el texto de recoger")] 
	public MainCanvas mainCanvas;

	[Tooltip("Equipo donde va a ser agregada el arma")] 
	public Equipment equipment;

	//Indica si el arma ha sido recogida
	private bool pickUp;

	[Tooltip("Tiempo restante antes de desaparecer el arma")] 
	public float remainTime;
	private float currentTime;

	// Use this for initialization
	void Start () {
		 
		//Crea arma en el personaje
		weaponGameObject =	(GameObject)Instantiate (weaponPrefab,characterWeapons,false);
		weaponGameObject.layer = 8;// Corresponde con layer="Player";
		 
		//Obtiene referencia a class Weapon
		weaponReference = weaponGameObject.GetComponent<Weapon> ();

		//Desactiva su visualizacion
		weaponGameObject.SetActive(false);

		pickUp = false;
		 
		currentTime = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButton ("PickUp")) {

			if (pickUp) {
				
                //Se anade al equipo 
				equipment.addWeapon (weaponReference);

				//Se elimina el texto 
				mainCanvas.setTextAdvert("");

				//Destruye el this objeto
				Destroy (gameObject);
			}
		}

		//En caso de pasar el tiempo limite, se eliminan los o
		if (currentTime > remainTime) {

			Destroy (weaponGameObject);
			Destroy (gameObject);

		} else {
			currentTime += Time.deltaTime;
		}

	}

	void OnTriggerEnter(Collider collision){
		 
		mainCanvas.setTextAdvert("Recoger Arma");
		pickUp = true;

	}
	 

	void OnTriggerExit(Collider collider){

		mainCanvas.setTextAdvert("");
		pickUp = false;
	
	}

}
