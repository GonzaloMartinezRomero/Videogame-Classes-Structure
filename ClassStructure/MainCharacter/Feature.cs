using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Feature : MonoBehaviour {

	//---- Cantidad de vida 
	private int hp;
	private int hp_max;

	//-----Cantidad de magia
	private int sp;
	private int sp_max;
	 
	//---Nivel
	private int lvl;


	//--Experiencia
	private int currentExp;
	private int maxExp;
	  

	//---- Damage que puede hacer el personaje
	private int damage_fisic;
	private int damage_magick;

	//-----Defensa del personaje
	private int defense;

	//Arma actualmente asignada
	private  Weapon currentWeapon;

	//Acceso al mainCanvas para indicarle cuando ha habido cambios
	public MainCanvas mainCanvas;

	public Mission1Controller controller;

	void Awake(){
	
		hp = hp_max=2000; 
		sp = sp_max= 1500;

		lvl = 1;
		currentExp = 0;
		maxExp = 1000;

		damage_fisic = 10;
		damage_magick = 20;
		defense = 10;

		currentWeapon = null;

	}




	/*
		Resta la cantidad de magia indicada al sp, indicando posteriormente si se 
		puede realizar la accion.
		En caso de no poder hacerse, no se resta
	*/
	public bool consumeMagick(int quantity){
		 

		if (sp - quantity >= 0) {			

			sp -= quantity;
			return true;
		}

		return false;
	}

	/*
		Anade la cantidad de magia indicada, no permitiendo superar el limite
	*/
	public void addMagick(int quantity){
	
		sp = (sp + quantity) > sp_max ? sp_max : sp += quantity; 
		 
	}



	/*
		Suma los stats proporcionados por el arma al personaje
	*/
	public void setWeapon(Weapon weapon){

		if (currentWeapon == null) {
		
			currentWeapon = weapon;

			//this.hp_max = this.hp_max + currentWeapon.getStatsWeapon ().getHpBonus ();
			//this.sp_max = this.sp_max + currentWeapon.getStatsWeapon ().getSpBonus ();
			this.damage_fisic = this.damage_fisic + currentWeapon.getStatsWeapon ().getDamageFisic (); 
			this.damage_magick = this.damage_magick + currentWeapon.getStatsWeapon ().getDamageMagik (); 		

			currentWeapon.getGameObject ().SetActive (true);
		
		} else {
		
			//Restan los valores antiguos y se le asignan los nuevos
			//this.hp_max = (this.hp_max - currentWeapon.getStatsWeapon().getHpBonus ())+weapon.getStatsWeapon().getHpBonus();
			//this.sp_max = (this.sp_max-currentWeapon.getStatsWeapon().getSpBonus())+ weapon.getStatsWeapon().getSpBonus ();
			this.damage_fisic =(this.damage_fisic-currentWeapon.getStatsWeapon().getDamageFisic()) +weapon.getStatsWeapon().getDamageFisic ();
			this.damage_magick =(this.damage_magick-currentWeapon.getStatsWeapon().getDamageMagik())+weapon.getStatsWeapon().getDamageMagik (); 

			//Desactivo la arma equipada
			currentWeapon.getGameObject ().SetActive (false);

			//Activo la nueva
			weapon.getGameObject ().SetActive (true);

			//Arma actual = nueva
			currentWeapon = weapon;
			 
		}

	}



	public int getCurrentHp(){
		return hp;
	}

	public int getMaxHp(){
		return hp_max;
	} 

	public int getCurrentSp(){
		return sp;
	}

	public int getMaxSp(){
		return sp_max;
	} 

	public int getDamageFisic(){
		return damage_fisic;
	}

	public int getDamageMagic(){
		return damage_magick;
	}

	public int getDefense(){
		return defense;
	}

	public int getLvl(){
		return lvl;
	}

	public int getCurrentExp(){
		return currentExp;
	}

	public int getMaxExp(){
		return maxExp;
	}

	public void addExperience(int quantity){

		int totalExperience = currentExp + quantity;

		if (totalExperience > maxExp) {
			
			lvl += 1;
			currentExp = totalExperience % maxExp;
			maxExp *= 2;
			hp += 100;
			sp += 100;
			damage_magick += 50;
			damage_fisic += 150;
			mainCanvas.updateLvlCanvas ();

		} else {
		
			currentExp = totalExperience;			 

		}
		mainCanvas.updateExp ();

	}


	public void addDamageFisic(int quantity){
		damage_fisic += quantity;
	}

	public void removeHP(int quantity){
		
		int totalQuantity=(quantity-defense)>0 ? (quantity-defense) : 0;

		hp -= totalQuantity;

		if (hp < 0) {
			//Muerto
			hp = 0;
			controller.characterIsDead ();

		} else {

			mainCanvas.setTextDamage (totalQuantity);
		} 

		mainCanvas.updateHPCanvas();

	}


}
