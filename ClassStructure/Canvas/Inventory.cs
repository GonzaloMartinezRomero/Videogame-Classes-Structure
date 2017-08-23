using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	//Boton asignado para cerrar el inventario
	public Button btnCloseInventary; 

	//Texto donde aparecen los stats del personaje
	public Text statusHP,statusSP,statusDefense,statusFisAtack,statusMagAtack;

	//Prefab del boton que aparece por cada arma del equipo
	public GameObject buttonPrefab;

	//GameObject donde aparecen los botones asociados a las armas
	public GameObject weaponSlots;

	//Referencia el personaje
	public Feature characterFeature;

	//Referencia al equipo
	public Equipment equipment;

	//Almacena la correspondencia entre las funciones de los botones y las armas
	private Dictionary<string,Button> weaponTree;



	/*
		Para una mayor velocidad de carga, el inventario se carga en el awake, ya que,
		si se carga en Start(), se carga cuando se activa el objeto, lo cual, lo hace 
		durante el juego. Es preferible tener todos los atributos ya cargados
	*/
	void Awake(){
		

		//Listener para cerrar el inventario 
		btnCloseInventary.onClick.AddListener (closeInventary);
		 	 
		weaponTree = new Dictionary<string,Button> ();

		//Cargo las armas del inventario con el objetivo de no hacerlo in game
		loadWeapons();

		//Al iniciar el inventario esta desactivado
		gameObject.SetActive (false);

	}



	public void updateStatsCanvas(){

		statusHP.text="HP Maxima: "+characterFeature.getMaxHp();
		statusSP.text = "SP Maxima: " + characterFeature.getMaxSp();
		statusFisAtack.text = "Ataque Fisico: " + characterFeature.getDamageFisic();
		statusMagAtack.text = "Ataque Magico: " + characterFeature.getDamageMagic();
		statusDefense.text = "Defensa: " + characterFeature.getDefense();

	}

	private void loadWeapons(){		


		foreach (Weapon weaponAux in equipment.getAllWeapons().Values) {
			
			//Si el arma no esta ya anadida al inventario, la anado
			if (!weaponTree.ContainsKey (weaponAux.getNameWeapon ())) {
				
				//Creo el boton
				GameObject buttonGameObject=(GameObject)Instantiate (buttonPrefab);
				Button button = buttonGameObject.GetComponent<Button> ();

				//Asigno al menu de las armas
				button.transform.SetParent(weaponSlots.transform) ;

				//Asigna funcion al hacer click
				button.onClick.AddListener (delegate() {

					setWeaponCharacter(weaponAux);
				});

				//Asignar la imagen del arma al boton
				button.GetComponent<Image>().sprite=weaponAux.getImage().sprite;

				//Almaceno en el tree O(1)
				weaponTree.Add(weaponAux.getNameWeapon(),button);

			}

		}

		

	}

	public void setWeaponCharacter(Weapon weaponSelected){
		
	 	characterFeature.setWeapon (weaponSelected);
		updateStatsCanvas ();
	}


	public void openInventary(){
		
		//Evita tener que cargar el todo el inventario para que solo sea cargado
		//cuando hay armas nuevas o se ha borrado alguna
		if(equipment.getNumberOfWeapons()!=weaponTree.Count)
			loadWeapons ();

		updateStatsCanvas ();
		gameObject.SetActive(true);	
	}

	public void closeInventary(){
		gameObject.SetActive (false);	
	}


	 
}
