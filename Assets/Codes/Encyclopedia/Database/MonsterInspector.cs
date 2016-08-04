using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[CustomEditor(typeof(MonsterManager))]
internal class MonsterInspector : Editor {
	bool showingMonsters = false;

	bool EnabledBase = false;

	void OnInspectiorUpdate(){
		
	}
	public override void OnInspectorGUI ()
	{
		MonsterManager monstermanager = target as MonsterManager;

		List<Monster> monsters = new List<Monster> ();
		List<MonsterID> ids = new List<MonsterID> ();
		
		for (int i = 0; i < monstermanager.monsterList.Count; i++) {
			if (monstermanager.monsterList [i].GetType() == typeof(Monster)) {
				monsters.Add (monstermanager.monsterList [i]);
			}
			if (monstermanager.monsterList [i].GetType () == typeof(MonsterID)) {
				monsters.Add (monstermanager.monsterList [i]);
			}
		}

		EditorGUILayout.LabelField ("Total Monsters Cout: " + monsters.Count.ToString());

		showingMonsters = EditorGUILayout.Foldout (showingMonsters, "Monsters Main Options:");
		if (showingMonsters) {
			EditorGUI.indentLevel = 1;
			for (int i = 0; i < monsters.Count; i++) {
				EditorGUILayout.LabelField ("ID: "+monsters [i].ID + " |Name: " + monsters[i].name + " |Attack: " + monsters[i].attack + " |HP: "+ monsters[i].hp + " ");

				EditorGUI.indentLevel = 2;
				monsters [i].ID = EditorGUILayout.IntField ("ID: ", monsters [i].ID);
				monsters [i].name = EditorGUILayout.TextField ("Name: ", monsters [i].name);
				monsters [i].description = EditorGUILayout.TextField ("Description: ", monsters [i].description);
				monsters [i].attack = EditorGUILayout.FloatField ("Attack: ", monsters [i].attack);
				monsters [i].hp = EditorGUILayout.IntField ("HP: ", monsters [i].hp);
				monsters [i].monsterSprite = (Sprite)EditorGUILayout.ObjectField ("Sprite",monsters [i].monsterSprite, typeof(Sprite));
				GUILayout.Space (20f);
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Advanced Options")) {
					MonsterDataBaseManager window = (MonsterDataBaseManager)EditorWindow.CreateInstance (typeof(MonsterDataBaseManager));
					EditorUtility.DisplayDialog ("Подсказка!", "Выбери объект MonsterManager, затем выбери строку MonsterEdit, в Pop-menu", "Хорошо");
					window.Show ();
				}
				if (GUILayout.Button ("Remove")) {
					monstermanager.monsterList.Remove (monsters [i]);
				}
				GUILayout.EndHorizontal ();
				GUILayout.Space (20f);
			}
			if (GUILayout.Button ("Add New Monster")) {
				Monster newMonster = (Monster)ScriptableObject.CreateInstance<Monster> ();
				newMonster.ID = monsters.Count + 1;
				newMonster.name = null;
				newMonster.description = null;
				newMonster.attack = 0;
				newMonster.hp = 0;
				newMonster.EpicPercent = 0.0f;
				newMonster.RarePercent = 0.0f;
				newMonster.SimplePercent = 0.0f;
				newMonster.GoldMin = 0.0f;
				newMonster.GoldMax = 0.0f;
				newMonster.GoldDrop = 0;
				newMonster.monsterSprite = null;
				monstermanager.monsterList.Add (newMonster);
			}
			EditorGUI.indentLevel = 0;
		}
		EnabledBase = EditorGUILayout.ToggleLeft (":I'm Expert", EnabledBase);

		if (EnabledBase) {
			base.OnInspectorGUI ();
		}
	}
}
