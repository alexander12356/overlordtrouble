using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

		[SerializeField] private Sprite[] MonsterFace;  


	void Start()
	{
		
	}

	void Awake()
	{
		MonsterFace = Resources.LoadAll<Sprite> ("Sprites/Encyclopedia/Mon");
	}

	void ChangeMonsterFace(){
		GameObject Image = GameObject.Find ("Splash");
		for (int i = 0; i < MonsterFace.Length; i++) {
			Image.GetComponent<Image> ().sprite = MonsterFace [i];
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			ChangeMonsterFace ();
		}

	}
}
