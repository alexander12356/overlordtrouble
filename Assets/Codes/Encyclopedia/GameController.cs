using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public GameObject[] MonstersList;

	public int IgnoreSteps = -2;
	[SerializeField] private Image Splash = null;
	[SerializeField] private GameObject MainStatsGroup = null;

	private GameObject[] currentList;

	private float fontSize = Screen.height / 10;

	public int StepCounter = 0;
	private int CurrentValue_OnDropDown = 0;
	void Start(){
		ChangeMonsterFace ();

	}

	void Awake()
	{
		MonstersList = Resources.LoadAll<GameObject> ("Prefabs/Monsters Prefab/");
		SettingsText ();
	}



	void SettingsText(){
		GameObject[] MonsterText = GameObject.FindGameObjectsWithTag ("Encycl");
		for (int i = 0; i < MonsterText.Length; i++) {
			MonsterText [i].GetComponent<Text> ().fontSize = Screen.height /26;
			MonsterText [i].GetComponent<Text> ().color = Color.black;
			MonsterText [i].GetComponent<Text> ().fontStyle = FontStyle.Bold;
		}
	}

	public void ChangeMonsterFace(){
		try{
			if (StepCounter <= MonstersList.Length && StepCounter > -1) {
				if (gameObject.GetComponent<GameController> ().MonstersList [StepCounter].GetComponent<MonsterScript> ().Visability == true) {
					Splash.sprite = MonstersList [StepCounter].GetComponent<MonsterScript> ().monsterSprite;
					MainStatsGroup.GetComponent<MainMonsterInfo> ().MonsterDescription.text = "Monster description:\n " + MonstersList [StepCounter].GetComponent<MonsterScript> ().description;
					MainStatsGroup.GetComponent<MainMonsterInfo> ().MonsterElement.text = "Monster Elements:\n ";
					Elements ();	

				}
				else {
					IgnoreSteps++;
				}
			}
		}
		catch(Exception e){
			Debug.Log ("Out of range Exception. # "+e);
		}
	}

	void Elements (){
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Dark == true) {
			MainStatsGroup.GetComponent<MainMonsterInfo>().MonsterElement.text += "Dark.\n ";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Light == true) {
			MainStatsGroup.GetComponent<MainMonsterInfo>().MonsterElement.text += "Light. \n";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Fire == true) {
			MainStatsGroup.GetComponent<MainMonsterInfo>().MonsterElement.text += "Fire. \n";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Water == true) {
			MainStatsGroup.GetComponent<MainMonsterInfo>().MonsterElement.text += "Water. \n";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Air == true) {
			MainStatsGroup.GetComponent<MainMonsterInfo>().MonsterElement.text += "Air. \n";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Earth == true) {
			MainStatsGroup.GetComponent<MainMonsterInfo>().MonsterElement.text += "Earth. \n";
		}
	}
	void Update(){
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			StepCounter++;

		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			StepCounter--;
		}
		if (StepCounter > (MonstersList.Length-IgnoreSteps)) {
			StepCounter = 0;
		}
		if (StepCounter <= -1) {
			StepCounter = MonstersList.Length - IgnoreSteps;
		}

	}

	void FixedUpdate(){
		
		ChangeMonsterFace ();
	}
}
