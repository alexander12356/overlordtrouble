using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject[] MonstersList;
	public GameObject[] ItemsList;
	public GameObject[] WeaponList;
	public GameObject[] ArmorList;

	[SerializeField] private Image Splash = null;
	[SerializeField] private Text MonsterID = null;
	[SerializeField] private Text MonsterName = null;
	[SerializeField] private Text MonsterDescription = null;
	[SerializeField] private Text MonsterAttack = null;
	[SerializeField] private Text MonsterHp = null;
	[SerializeField] private Text MonsterDefence = null;
	[SerializeField] private Text MonsterElements = null;
	[SerializeField] private Text MonsterGoldDrop = null;
	[SerializeField] private Text MonsterSimpleDrop = null;
	[SerializeField] private Text MonsterRareDrop = null;
	[SerializeField] private Text MonsterEpicDrop = null;

	[SerializeField] private InputField inf = null;

	[SerializeField] private GameObject[] currentList;

	private float fontSize = Screen.height / 10;

	public int StepCounter = 0;
	private int CurrentValue_OnDropDown = 0;
	void Start(){
		ChangeMonsterFace ();
	}

	void Awake()
	{
		CurrentValue_OnDropDown = gameObject.GetComponent<SearchScriptById_Enc> ().drop.value;
		MonstersList = Resources.LoadAll<GameObject> ("Prefabs/Monsters Prefab/");
		SettingsText ();
		SwitchList ();
	}



	void SettingsText(){
		GameObject[] MonsterText = GameObject.FindGameObjectsWithTag ("Encycl");
		for (int i = 0; i < MonsterText.Length; i++) {
			MonsterText [i].GetComponent<Text> ().fontSize = Screen.height / 15;
			MonsterText [i].GetComponent<Text> ().color = Color.black;
			MonsterText [i].GetComponent<Text> ().fontStyle = FontStyle.Bold;
		}
	}

	public void ChangeMonsterFace(){
		if (StepCounter < currentList.Length&&StepCounter > -1) {
			Splash.sprite = MonstersList [StepCounter].GetComponent<MonsterScript> ().monsterSprite;
			MonsterID.text = "ID: " + MonstersList [StepCounter].GetComponent<MonsterScript> ().ID;
			MonsterName.text = "Monster name: " + MonstersList [StepCounter].GetComponent<MonsterScript> ().name;
			MonsterDescription.text = "Monster description: "+ MonstersList [StepCounter].GetComponent<MonsterScript> ().description;
			MonsterAttack.text = "Damage: " + Mathf.RoundToInt(MonstersList [StepCounter].GetComponent<MonsterScript> ().attack).ToString();
			MonsterHp.text = "Hp: " + Mathf.RoundToInt(MonstersList [StepCounter].GetComponent<MonsterScript> ().hp).ToString();
			MonsterDefence.text = "Defence: " + Mathf.RoundToInt(MonstersList [StepCounter].GetComponent<MonsterScript> ().Defence).ToString();
			MonsterElements.text = "Monster Elements: ";
			Elements ();		
			MonsterGoldDrop.text = "Gold drop: ~" + MonstersList [StepCounter].GetComponent<MonsterScript> ().GoldDrop;
			MonsterSimpleDrop.text = "Simple item drop Rate: " + MonstersList [StepCounter].GetComponent<MonsterScript> ().SimplePercent + "%";
			MonsterRareDrop.text = "Rare item drop Rate: " + MonstersList [StepCounter].GetComponent<MonsterScript> ().RarePercent + "%";
			MonsterEpicDrop.text = "Epic item drop Rate: " + MonstersList [StepCounter].GetComponent<MonsterScript> ().EpicPercent + "%";
		}
	}
	void Elements (){
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Dark == true) {
			MonsterElements.text += "Dark. ";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Light == true) {
			MonsterElements.text += "Light. ";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Fire == true) {
			MonsterElements.text += "Fire. ";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Water == true) {
			MonsterElements.text += "Water. ";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Air == true) {
			MonsterElements.text += "Air. ";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Earth == true) {
			MonsterElements.text += "Earth. ";
		}
	}

	void FixedUpdate () {
		ChangeMonsterFace ();
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
				StepCounter++;
				ChangeMonsterFace ();
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				StepCounter--;
				ChangeMonsterFace ();
		}
		if (gameObject.GetComponent<SearchScriptById_Enc> ().drop.value != CurrentValue_OnDropDown) {
			SwitchList();
			CurrentValue_OnDropDown = gameObject.GetComponent<SearchScriptById_Enc> ().drop.value;
		}
	}

	void SwitchList(){
		switch (gameObject.GetComponent<SearchScriptById_Enc> ().drop.value) {
		case 0:
			currentList = MonstersList;
			break;
		case 1:
			currentList = ItemsList;
			break;
		case 2:
			currentList = WeaponList;
			break;
		case 3:
			currentList = ArmorList;
			break;
		}
	}
}
