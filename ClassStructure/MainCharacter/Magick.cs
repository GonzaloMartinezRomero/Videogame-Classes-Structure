using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magick : MonoBehaviour {

	public AudioSource magickSound;

	public GameObject particleSystemMagick;
	public int sp_consume;
	public float duration;

	private bool isActive;
	private float currentTime;


	public int damage;
	private int tempDamage;

	private BoxCollider boxCollider;

	[Tooltip("Tiempo de retardo para activar el collider. 0 si no hace falta")]
	public float activationRetard;

	private bool enableBoxCollider;


	// Use this for initialization
	void Start () {
		
		isActive = false;
		currentTime = 0.0f;
		boxCollider = GetComponent<BoxCollider> ();
		boxCollider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (isActive) {

			if (!magickSound.isPlaying)
				magickSound.Play ();
			
			//Se comprueba que la magia no ha terminado
			if (currentTime > duration) {

				isActive = false;

				particleSystemMagick.SetActive (false);

				damage-=tempDamage;

				boxCollider.enabled = false;

				magickSound.Stop ();

			} else {
				//Comprobar que se quiere activar el collider 
				if (enableBoxCollider) {

					//Esperar el tiempo de retardo para activar collider
					if(currentTime>activationRetard){

						if (!boxCollider.enabled) {
							boxCollider.enabled = true;
						}

						//Una vez activado no hay que comprobarlo mas
						enableBoxCollider = false;
					}
				}

				currentTime += Time.deltaTime;
			}

		}

	}
	/*
		plusMagickDamage: Extra de magia que viene dado por el personjae principal
		Total de damageMagick= DamageMagick(Magia)+DamageMagick(Personaje)
		
		enableBoxCollider: activa o no el collider de la magia asociada en el retardo establecido

	*/
	public void activateMagick(int plusMagickDamage,bool _enableBoxCollider){

		//Activar el sistema de particulas
		particleSystemMagick.SetActive (true);

		//Indicar que la magia esta activada para iniciar contador
		isActive = true;

		//Iniciar contador en 0
		currentTime = 0.0f;

		//Asignar plus de damage proveniente del personaje principal
		tempDamage = plusMagickDamage;
		damage += plusMagickDamage;

		//Indicar si se quiere activar el collider asociado a la magia
		enableBoxCollider = _enableBoxCollider;
	
	}

	public int getSPConsume(){
		return sp_consume;
	}
	public bool isActiveMagick(){
		return isActive;
	}
	public float getDuration(){		
		return duration;
	}
	public int getDamage(){
		return damage;
	}


}
