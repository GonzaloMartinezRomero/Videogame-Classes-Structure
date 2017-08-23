using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsWeapon:MonoBehaviour {

	public int damage_fisic;
	public int damage_magik;
	public int hp_bonus;
	public int sp_bonus;


	public int getDamageFisic(){
		return damage_fisic;
	}

	public int getDamageMagik(){
		return damage_magik;
	}

	public int getHpBonus(){
		return hp_bonus;
	}

	public int getSpBonus(){
		return sp_bonus;
	}

	 

}
