using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {

	private GameObject weaponGameObject;
	private StatsWeapon statsWeapon;
	private Image image; 
	private string weaponName;

	void Awake(){

		statsWeapon = GetComponent<StatsWeapon> ();
		weaponGameObject = gameObject;
		weaponName = weaponGameObject.name;
		image = GetComponent<Image> ();

	}


	public StatsWeapon getStatsWeapon(){
		return this.statsWeapon;
	}
	  

	public Image getImage(){
		return image;
	}

	public GameObject getGameObject(){
		return weaponGameObject;
	}

	public string getNameWeapon(){
		return weaponName;
	}

}
