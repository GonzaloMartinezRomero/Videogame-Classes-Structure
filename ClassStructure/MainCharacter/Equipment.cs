using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Equipment : MonoBehaviour {

	//Arma por defecto
	public Weapon weaponDefault;

	//Lista con todas las armas que tiene el personaje
	Dictionary<String,Weapon> weaponTree;

	//Numero maximo de slots para guardar armas
	private int maxWeaponsSlots; 

	//Referencia al personaje
	private Feature featureCharacter;


	void Awake(){

		maxWeaponsSlots = 6; 

		//Inicializacion del la ed para las armas
		weaponTree = new Dictionary<String,Weapon>();

		//Lista de armas por defecto
		weaponTree.Add(weaponDefault.getNameWeapon(),weaponDefault);

		//Referencia al personaje para equiparle el arma por defecto al inciar
		featureCharacter=GetComponent<Feature>();
		featureCharacter.setWeapon (weaponDefault);
	
	}



	/*
		Eficiency:O(1) 
	*/
	public void addWeapon(Weapon weapon){


		//Comprobar que el gameObject es de tipo weapon y no excede numero de slots 
		if ((weapon.tag == "Weapon") && (weaponTree.Count<maxWeaponsSlots)) {
			
			try{
 
				weaponTree.Add(weapon.getNameWeapon(),weapon);

			}catch(ArgumentException){
				//Arma ya esta agregada
			}

		}
	}


	/*
		Eficiency: O(1) 
	*/
	public void deleteWeapon(string  nameWeapon){
 		
		weaponTree.Remove (nameWeapon);
	 
	}

	//Return todas las armas del inventario
	public Dictionary<String,Weapon> getAllWeapons(){
		return this.weaponTree;
	}

	public int getNumberOfWeapons(){
		return this.weaponTree.Count;
	}

}
