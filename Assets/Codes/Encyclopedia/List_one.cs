using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class List_one : MonoBehaviour {
	private GameObject prefabOption;
	//[SerializeField] private GameObject Listed;
	[SerializeField]private GameObject Content;
	private int Length = 0;
	private GameObject deltaGO;
	private GameObject[] DeltaGO;
	private GameObject GameCnrl;
	void Awake(){
		prefabOption = Resources.Load<GameObject> ("Prefabs/Encyclopedia/ListOption");

		GameCnrl = GameObject.Find ("GameManager");
	}

	void Start(){
		Length = gameObject.GetComponent<GameController> ().MonstersList.Length;
		ChangeList ();
		DeltaGO = GameObject.FindGameObjectsWithTag ("ListOption");
	}

	void Update(){
		
	}

	private void ChangeList(){
		for (int i = 0; i < Length; i++) {
			deltaGO = Instantiate (prefabOption);
			deltaGO.transform.SetParent (Content.transform);
			deltaGO.transform.localScale = new Vector3 (1, 1, 1);
			deltaGO.transform.localPosition = new Vector3 (550, -10, 0);
			if (gameObject.GetComponent<GameController> ().MonstersList [i].GetComponent<MonsterScript> ().Visability == true) {
				deltaGO.name = i.ToString ();
				deltaGO.GetComponentInChildren<Text> ().text = gameObject.GetComponent<GameController>().MonstersList[i].GetComponent<MonsterScript>().name;
				deltaGO.GetComponentInChildren<Image>().sprite  = gameObject.GetComponent<GameController> ().MonstersList [i].GetComponent<MonsterScript> ().monsterSprite;
			} else {
				deltaGO.GetComponentInChildren<Text>().text = "UNKNOW";
				deltaGO.GetComponent<Button> ().interactable = false;
				deltaGO.GetComponentInChildren<Image> ().enabled = false;
			}
			deltaGO.GetComponentInChildren<Text> ().fontSize = Screen.height/26;
			deltaGO.GetComponent<Button> ().onClick.AddListener(()=>ChangeOnClick ());
		}
	}
	public void ChangeOnClick(){
		gameObject.GetComponent<GameController> ().StepCounter = int.Parse (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
	}

}
