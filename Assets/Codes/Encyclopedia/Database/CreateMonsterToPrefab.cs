#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateMonsterToPrefab{
	const string menuTitle = "RPG Manager/Create Prefab Selected Object";

	[MenuItem(menuTitle)]
	static void CreatePrefab()
	{
		GameObject obj = Selection.activeGameObject;
		string name = obj.name;


		//Хочу сюда пихнуть добавление в общий List вещей и монстров) 
		Object prefab;
		if (obj.GetComponent<MonsterScript>() != null) {
			prefab = PrefabUtility.CreatePrefab ("Assets/Resources/Prefabs/Monsters Prefab/" + name + ".prefab", obj);
		} else {
			prefab = PrefabUtility.CreatePrefab ("Assets/Resources/Prefabs/" + name + ".prefab", obj);
		}
		PrefabUtility.ReplacePrefab (obj, prefab);
		AssetDatabase.Refresh ();
	}

}
#endif