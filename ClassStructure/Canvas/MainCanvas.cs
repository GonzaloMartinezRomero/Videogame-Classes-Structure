using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainCanvas : MonoBehaviour {

	//---Abrir el menu
	public Button btnOpenMenu;
	public Menu menu;


	//---Abrir el inventario
	public Button btnOpenInventary;
	public Inventory inventory;

	//---Control canvas consumo de magia
	public Image imageSPMagick;

	public Image imageHP;

	//---Proporcion del consumo de magia respecto al maximo
	private float proportionCanvasMagick;

	//---Proporcion del consumo de hp respecto al maximo
	private float proportionCanvasHP;


	public Text textLvlCanvas;
	public Text textExpCanvas;	

	//Referencia al stats del personaje
	public Feature characterFeature;

	//Referencia a las magicksBehaviour
	public MagickBehaviour magickBehaviour;


	public Text textAdvert;
	public GameObject textDamagePrefab;


	//---Controles magia 1---
	public Button btnMagick1;
	public Image imgMagick1;
	private bool isActiveMG1;
	private float proportionDurationMG1;

	//---Controles magia 2
	public Button btnMagick2;
	public Image imgMagick2;
	private bool isActiveMG2;
	private float proportionDurationMG2;

	//---Controles magia 3
	public Button btnMagick3;
	public Image imgMagick3;
	private bool isActiveMG3;
	private float proportionDurationMG3;

	//---Controles magia 4
	public Button btnMagick4;
	public Image imgMagick4;
	private bool isActiveMG4;
	private float proportionDurationMG4;



	// Use this for initialization
	void Start () {
		
		//Boton para abrir el menu
		btnOpenMenu.onClick.AddListener(openMenu);

		//Boton para abrir el inventario
		btnOpenInventary.onClick.AddListener (openInventory);

		//Asignacion de una funcion al pulsar el boton
		btnMagick1.onClick.AddListener (executeMagick1);
		btnMagick2.onClick.AddListener (executeMagick2);
		btnMagick3.onClick.AddListener (executeMagick3);
		btnMagick4.onClick.AddListener (executeMagick4);

		//Estado inicial de las magias
		isActiveMG1 = isActiveMG2= isActiveMG3= isActiveMG4= false;

		//Proporcion entre la duracion de la magia y el rellenado de la imagen 
		proportionDurationMG1=1.0f/magickBehaviour.getDurationMagick1();
		proportionDurationMG2=1.0f/magickBehaviour.getDurationMagick2();
		proportionDurationMG3=1.0f/magickBehaviour.getDurationMagick3();
		proportionDurationMG4=1.0f/magickBehaviour.getDurationMagick4();
		 
	
		//Texto inicial del nivel
		textLvlCanvas.text = "Nivel: " +  characterFeature.getLvl().ToString();
		
		//Texto inciaal para la experiencia
		textExpCanvas.text = "Experiencia: " + characterFeature.getCurrentExp().ToString()+"/"+characterFeature.getMaxExp().ToString();

		//Proporcion entre la longitud de la barra de magia y la cantidad total
		proportionCanvasMagick = 1.0f / (float)characterFeature.getMaxSp();
		proportionCanvasHP= 1.0f / (float)characterFeature.getMaxHp();

		textAdvert.text = "";
	}
	
	// Update is called once per frame
	void Update () {

		//-------- Controlador de avance del rellenado de la imagen de las magias ---------
		if (isActiveMG1) {

			if (imgMagick1.fillAmount > 0.0f) {
				imgMagick1.fillAmount -= proportionDurationMG1 * Time.deltaTime;
			} else {
				isActiveMG1 = false;
				magickBehaviour.revertEfectMagick1 ();
			}
				

		}  

		if (isActiveMG2) {

			if (imgMagick2.fillAmount > 0.0f)
				imgMagick2.fillAmount -= proportionDurationMG2 * Time.deltaTime;
			else
				isActiveMG2 = false;
		
		}

		if (isActiveMG3) {

			if (imgMagick3.fillAmount > 0.0f)
				imgMagick3.fillAmount -= proportionDurationMG3 * Time.deltaTime;
			else
				isActiveMG3 = false;

		}

		if (isActiveMG4) {

			if (imgMagick4.fillAmount > 0.0f)
				imgMagick4.fillAmount -= proportionDurationMG4* Time.deltaTime;
			else
				isActiveMG4 = false;

		}


	}

	/*
		En caso de restar hp o sp, la proporcion que se le resta a la barra se actualiza
		respecto a cantidadActual/cantidadMaxima
	*/
	public void updateMagickCanvas( ){
		
		imageSPMagick.fillAmount =1.0f- (proportionCanvasMagick * (float)characterFeature.getCurrentSp()); 
		 
	}

	public void updateHPCanvas(){
		imageHP.fillAmount =1.0f- (proportionCanvasHP * (float)characterFeature.getCurrentHp()); 

	}

	 

	public void updateLvlCanvas(){
		textLvlCanvas.text ="Nivel: " +  characterFeature.getLvl().ToString();
	}

	public void updateExp(){
		textExpCanvas.text = "Experiencia: " + characterFeature.getCurrentExp().ToString()+"/"+characterFeature.getMaxExp().ToString();
	}
	 
	/*
		En caso de haberse modificado el max de hp o sp
		se debe actualizar con el fin de que la proporcion establecida en 
		el canvas sea la correcta
	*/
	public void updateProportionMagick(){
		proportionCanvasMagick = 1.0f / (float)characterFeature.getMaxSp();
	}

	public void updateProportionHP(){
		proportionCanvasMagick = 1.0f / (float)characterFeature.getMaxHp();
	}



	public void executeMagick1(){
		
		if (magickBehaviour.executeMagick1 ()) {

			/*
				Elimina el ultimo objeto almacenado por el raycast para evitar anomalias
				al pulsar la barra o el enter, ya que se selecciona el objeto almacenado 
				por el raycast
			*/	
			//EventSystem.current.SetSelectedGameObject(null);
			isActiveMG1 = true;
			imgMagick1.fillAmount = 1.0f;
			updateMagickCanvas ();
		}

	}

	public void executeMagick2(){

		if (magickBehaviour.executeMagick2 ()) {
			
			//EventSystem.current.SetSelectedGameObject(null);
			isActiveMG2 = true;
			imgMagick2.fillAmount = 1.0f;
			updateMagickCanvas ();
		}

	}

	public void executeMagick3(){

		if (magickBehaviour.executeMagick3 ()) {

			//EventSystem.current.SetSelectedGameObject(null);
			isActiveMG3 = true;
			imgMagick3.fillAmount = 1.0f;
			updateMagickCanvas ();
		}

	}

	public void executeMagick4(){

		if (magickBehaviour.executeMagick4 ()) {
			
			//EventSystem.current.SetSelectedGameObject(null);
			isActiveMG4 = true;
			imgMagick4.fillAmount = 1.0f;
			updateMagickCanvas ();
		}

	}

	public void openMenu(){
		menu.interactMenu ();
	}

	public void openInventory(){
		inventory.openInventary ();
	}

	public void setTextAdvert(string newText){
		textAdvert.text = newText;
	
	}

	//Pone un texto por pantalla el cual se elimina pasado un tiempo time
	public void setTextAdvert(string newText, float time){
	
		setTextAdvert(newText);
		Invoke ("clearText",time);

	}

	private void clearText(){
		textAdvert.text = "";
	}




	public void setTextDamage(int damage){
	
	
		//Crear nueva instancia del texto
		GameObject prefabAux=(GameObject)Instantiate(textDamagePrefab,gameObject.transform,false);
		 
		//Asigna la cantidad de damage recibida
		(prefabAux.GetComponent<Text> ()).text=damage.ToString(); 

		//Se elimina
		Destroy (prefabAux,0.4f);
	}

	public void setActivation(bool state){
		gameObject.SetActive (state);
	}


}
