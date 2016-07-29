using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[SerializeField] private GameObject[] MonstersPrefabs;
	[SerializeField] private Sprite[] deltaSprites;

	[SerializeField] private Image Splash;
	[SerializeField] private Text MonsterName;
	[SerializeField] private Text MonsterDescription;
	[SerializeField] private Text MonsterAttack;
	[SerializeField] private Text MonsterHp;
	[SerializeField] private Text MonsterDefence;
	[SerializeField] private Text MonsterElements;
	[SerializeField] private Text MonsterGoldDrop;
	[SerializeField] private Text MonsterSimpleDrop;
	[SerializeField] private Text MonsterRareDrop;
	[SerializeField] private Text MonsterEpicDrop;




	private float fontSize = Screen.height / 10;

	private int PressCounter = 0;

	void Start(){
		ChangeMonsterFace ();
	}

	void Awake()
	{
		MonstersPrefabs = Resources.LoadAll<GameObject> ("Prefabs/Monsters Prefab/");
		SettingsText ();
	}
	void SettingsText(){
		GameObject[] MonsterText = GameObject.FindGameObjectsWithTag ("Encycl");
		for (int i = 0; i < MonsterText.Length; i++) {
			MonsterText [i].GetComponent<Text> ().fontSize = Screen.height / 15;
			MonsterText [i].GetComponent<Text> ().color = Color.white;
			MonsterText [i].GetComponent<Text> ().fontStyle = FontStyle.Bold;
		}
	}

	void ChangeMonsterFace(){
		if (PressCounter < MonstersPrefabs.Length&&PressCounter > -1) {
			Splash.sprite = MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().monsterSprite;
			MonsterName.text = "Monster name: " + MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().name;
			MonsterDescription.text = "Monster description: "+ MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().description;
			MonsterAttack.text = "Damage: " + Mathf.RoundToInt(MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().attack).ToString();
			MonsterHp.text = "Hp: " + Mathf.RoundToInt(MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().hp).ToString();
			MonsterDefence.text = "Defence: " + Mathf.RoundToInt(MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().Defence).ToString();
			MonsterElements.text = "Monster Elements: ";
			Elements ();		
			MonsterGoldDrop.text = "Gold drop: ~" + MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().GoldDrop;
			MonsterSimpleDrop.text = "Simple item drop Rate: " + MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().SimplePercent + "%";
			MonsterRareDrop.text = "Rare item drop Rate: " + MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().RarePercent + "%";
			MonsterEpicDrop.text = "Epic item drop Rate: " + MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().EpicPercent + "%";
		}
	}
	void Elements (){
		if (MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().Dark == true) {
			MonsterElements.text += "Dark. ";
		}
		if (MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().Light == true) {
			MonsterElements.text += "Light. ";
		}
		if (MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().Fire == true) {
			MonsterElements.text += "Fire. ";
		}
		if (MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().Water == true) {
			MonsterElements.text += "Water. ";
		}
		if (MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().Air == true) {
			MonsterElements.text += "Air. ";
		}
		if (MonstersPrefabs [PressCounter].GetComponent<MonsterScript> ().Earth == true) {
			MonsterElements.text += "Earth. ";
		}
	}

	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
				PressCounter++;
				ChangeMonsterFace ();
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				PressCounter--;
				ChangeMonsterFace ();
		}
	}
}
