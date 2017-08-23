using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeature:MonoBehaviour {

	public int hp;
	public int exp;
	public int damage;
	public int defense;
	 

	public int getHP(){
		return hp;
	}

	/*
	 	Si el ataque recibido es menor que la cantidad de defensa, se le sube
		vida, por eso cuando es menor de 0, se asigna 0
	*/
	public void receiveDamage(int damage){
		hp -= (damage>0) ? damage : 0;
	}

	public bool isDead(){
		return hp < 0;
	}


	public int getExp(){
		return exp;
	}

	public int getDamage(){
		return damage;
	}

	public int getDefense(){
		return defense;
	}


}
