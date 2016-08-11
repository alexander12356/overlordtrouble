/*using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class SearchScriptById_Enc : MonoBehaviour {

	public Dropdown drop;
	[SerializeField] private GameObject[] deltaGameObjects;
	[SerializeField] private InputField inf;




	//добавить поиск по итемам
	public void SearchByID ()
	{
		try{
			if (drop.value == 0 && inf.text != null && inf.text != " ") {
				deltaGameObjects = gameObject.GetComponent<GameController> ().MonstersList;
				for (int i = 0; i < deltaGameObjects.Length; i++) {
					if (deltaGameObjects [i].GetComponent<MonsterScript> ().ID == int.Parse(inf.text)) {
						gameObject.GetComponent<GameController> ().StepCounter = i;
						gameObject.GetComponent<GameController> ().ChangeMonsterFace ();
					}
				}
			}
			if (drop.value == 1) {
			}
			if (drop.value == 2) {
			}
			if (drop.value == 3) {
			}
		}
		catch(Exception e)
		{
			Debug.Log ("Ошибка ввода поиска по ИД: " + e);
		}
	}		
}
*/