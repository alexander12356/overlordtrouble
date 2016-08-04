using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject[] MonstersList;
	public GameObject[] ItemsList;
	public GameObject[] WeaponList;
	public GameObject[] ArmorList;

	[SerializeField] private Image Splash = null;
	[SerializeField] private GameObject MainStatsGroup = null;
	[SerializeField] private GameObject SecondaryMonsterInfo = null;



	[SerializeField] private InputField inf = null;


	private GameObject[] currentList;

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
			MainStatsGroup.GetComponent<MainMonsterInfo>().MonsterID.text = "ID: " + MonstersList [StepCounter].GetComponent<MonsterScript> ().ID;
			MainStatsGroup.GetComponent<MainMonsterInfo>().MonsterName.text = "Monster name: " + MonstersList [StepCounter].GetComponent<MonsterScript> ().name;
			MainStatsGroup.GetComponent<MainMonsterInfo>().MonsterDescription.text = "Monster description: "+ MonstersList [StepCounter].GetComponent<MonsterScript> ().description;
			MainStatsGroup.GetComponent<MainMonsterInfo>().MonsterAttack.text = "Damage: " + Mathf.RoundToInt(MonstersList [StepCounter].GetComponent<MonsterScript> ().attack).ToString();
			MainStatsGroup.GetComponent<MainMonsterInfo>().MonsterHp.text = "Hp: " + Mathf.RoundToInt(MonstersList [StepCounter].GetComponent<MonsterScript> ().hp).ToString();
			MainStatsGroup.GetComponent<MainMonsterInfo>().MonsterDefence.text = "Defence: " + Mathf.RoundToInt(MonstersList [StepCounter].GetComponent<MonsterScript> ().Defence).ToString();
			SecondaryMonsterInfo.GetComponent<SecondaryMonsterInfo>().MonsterElements.text = "Monster Elements: ";
			Elements ();		
			SecondaryMonsterInfo.GetComponent<SecondaryMonsterInfo>().MonsterGoldDrop.text = "Gold drop: ~" + MonstersList [StepCounter].GetComponent<MonsterScript> ().GoldDrop;
			SecondaryMonsterInfo.GetComponent<SecondaryMonsterInfo>().MonsterSimpleDrop.text = "Simple item drop Rate: " + MonstersList [StepCounter].GetComponent<MonsterScript> ().SimplePercent + "%";
			SecondaryMonsterInfo.GetComponent<SecondaryMonsterInfo>().MonsterRareDrop.text = "Rare item drop Rate: " + MonstersList [StepCounter].GetComponent<MonsterScript> ().RarePercent + "%";
			SecondaryMonsterInfo.GetComponent<SecondaryMonsterInfo>().MonsterEpicDrop.text = "Epic item drop Rate: " + MonstersList [StepCounter].GetComponent<MonsterScript> ().EpicPercent + "%";
		}
	}
	void Elements (){
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Dark == true) {
			SecondaryMonsterInfo.GetComponent<SecondaryMonsterInfo>().MonsterElements.text += "Dark. ";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Light == true) {
			SecondaryMonsterInfo.GetComponent<SecondaryMonsterInfo>().MonsterElements.text += "Light. ";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Fire == true) {
			SecondaryMonsterInfo.GetComponent<SecondaryMonsterInfo>().MonsterElements.text += "Fire. ";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Water == true) {
			SecondaryMonsterInfo.GetComponent<SecondaryMonsterInfo>().MonsterElements.text += "Water. ";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Air == true) {
			SecondaryMonsterInfo.GetComponent<SecondaryMonsterInfo>().MonsterElements.text += "Air. ";
		}
		if (MonstersList [StepCounter].GetComponent<MonsterScript> ().Earth == true) {
			SecondaryMonsterInfo.GetComponent<SecondaryMonsterInfo>().MonsterElements.text += "Earth. ";
		}
	}
	void Update(){
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			StepCounter++;
			ChangeMonsterFace ();
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			StepCounter--;
			ChangeMonsterFace ();
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			gameObject.GetComponent<SearchScriptById_Enc> ().drop.Show ();
		}
		if (gameObject.GetComponent<SearchScriptById_Enc> ().drop.value != CurrentValue_OnDropDown) {
			SwitchList();
			CurrentValue_OnDropDown = gameObject.GetComponent<SearchScriptById_Enc> ().drop.value;
			inf.Select ();
		}

	}

	void FixedUpdate () {
		ChangeMonsterFace ();
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
