using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MonsterDataBaseManager : EditorWindow {
	[MenuItem("RPG Manager/Monster Editor")]
	static void Init(){
		MonsterDataBaseManager	window = (MonsterDataBaseManager)EditorWindow.CreateInstance (typeof(MonsterDataBaseManager));
		window.Show ();
	
	}

	public enum MonsterTypeToCreate{
		Monster,
		MonsterEdit
	}

	public MonsterTypeToCreate currentItemInTheBase = MonsterTypeToCreate.Monster;
	string newMonsterName="";
	string newMonsterDescription="";
	int NewMonsterAttack = 0;
	int NewMonsterHp = 0;
	float NewPercentEpic = 0f;
	float NewPercentRare = 0f;
	float NewPercentSimple = 0f;

	float minGoldDrop = 0;
	float maxGoldDrop = 0;
	int Gold;

	Sprite newSpriteMonster;
	int deltaID;
	int curMonsterID;


	public MonsterManager monsterManager;
	void OnGUI(){
		monsterManager = (MonsterManager)EditorGUILayout.ObjectField (monsterManager, typeof(MonsterManager),true);
		if (monsterManager != null) {
			currentItemInTheBase = (MonsterTypeToCreate)EditorGUILayout.EnumPopup (currentItemInTheBase);
			curMonsterID = monsterManager.monsterList.Count + 1;
			switch (currentItemInTheBase) {
			case MonsterTypeToCreate.Monster:
				newMonsterName = EditorGUILayout.TextField ("Monster name: ", newMonsterName);
				newMonsterDescription = EditorGUILayout.TextField ("Monster description: ", newMonsterDescription);
				NewMonsterAttack = EditorGUILayout.IntField ("Monster Attack: ", NewMonsterAttack);
				NewMonsterHp = EditorGUILayout.IntField ("Monster HP: ", NewMonsterHp);
				curMonsterID = EditorGUILayout.IntField ("Monster ID: ", curMonsterID);
				GUILayout.Space (10f);
				newSpriteMonster = (Sprite)EditorGUILayout.ObjectField ("Sprite", newSpriteMonster, typeof(Sprite));
				NewPercentEpic = EditorGUILayout.Slider ("Epic Percent Drop: ", NewPercentEpic, 0.0f, 10.0f);
				NewPercentRare = EditorGUILayout.Slider ("Rare Percent Drop: ", NewPercentRare, NewPercentEpic, 100.0f - NewPercentEpic * 4);
				NewPercentSimple = EditorGUILayout.Slider ("Simple Percent Drop: ", NewPercentSimple, 100.0f - NewPercentRare - NewPercentEpic, 100.0f - NewPercentRare - NewPercentEpic);
				GUILayout.Space (10f);
				minGoldDrop = EditorGUILayout.FloatField ("Gold Min Drop: ", minGoldDrop);
				maxGoldDrop = EditorGUILayout.FloatField ("Gold Max Drop: ", maxGoldDrop);
				Gold = EditorGUILayout.IntField ("Gold Drop :", Mathf.CeilToInt (Random.Range (minGoldDrop, maxGoldDrop)));


				if (GUILayout.Button ("Add New Item")) {
					Monster newMonster = (Monster)ScriptableObject.CreateInstance<Monster> ();
					newMonster.name = newMonsterName;
					newMonster.description = newMonsterDescription;
					newMonster.attack = NewMonsterAttack;
					newMonster.hp = NewMonsterHp;
					newMonster.EpicPercent = NewPercentEpic;
					newMonster.RarePercent = NewPercentRare;
					newMonster.SimplePercent = NewPercentSimple;
					newMonster.GoldMin = minGoldDrop;
					newMonster.GoldMax = maxGoldDrop;
					newMonster.GoldDrop = Gold;
					newMonster.monsterSprite = newSpriteMonster;
					monsterManager.monsterList.Add (newMonster);
				}
				break;
			case MonsterTypeToCreate.MonsterEdit:
				GUILayout.Space (10f);
				deltaID = EditorGUILayout.IntField ("Enter Monster ID: ", deltaID);
				GUILayout.Space (20f);
				if (deltaID != null) {
						List<Monster> deltaMonsterList = new List<Monster> ();
						for (int i = 0; i < monsterManager.monsterList.Count; i++) {
							if (deltaID == monsterManager.monsterList [i].ID) {
								
								EditorGUILayout.BeginScrollView (Vector2.zero);
								monsterManager.monsterList [i].name = EditorGUILayout.TextField ("Name: ", monsterManager.monsterList [i].name);
								monsterManager.monsterList [i].attack = EditorGUILayout.FloatField ("Attack: ", monsterManager.monsterList [i].attack);
								monsterManager.monsterList [i].hp = EditorGUILayout.IntField ("HP: ", monsterManager.monsterList [i].hp);
								monsterManager.monsterList [i].description = EditorGUILayout.TextField ("Description: ", monsterManager.monsterList [i].description);
								monsterManager.monsterList [i].monsterSprite = (Sprite)EditorGUILayout.ObjectField ("Sprite: ", monsterManager.monsterList [i].monsterSprite, typeof(Sprite));
								monsterManager.monsterList [i].EpicPercent = EditorGUILayout.Slider ("Epic Percent Drop: ", monsterManager.monsterList [i].EpicPercent, 0.0f, 10.0f);
								monsterManager.monsterList [i].RarePercent = EditorGUILayout.Slider ("Epic Percent Drop: ", monsterManager.monsterList [i].RarePercent, monsterManager.monsterList [i].EpicPercent, 100.0f - monsterManager.monsterList [i].EpicPercent * 4);
								monsterManager.monsterList [i].SimplePercent = EditorGUILayout.Slider ("Epic Percent Drop: ", monsterManager.monsterList [i].SimplePercent, 100.0f - monsterManager.monsterList [i].EpicPercent - monsterManager.monsterList [i].RarePercent, 100.0f - monsterManager.monsterList [i].EpicPercent - monsterManager.monsterList [i].RarePercent);
								monsterManager.monsterList [i].GoldMin = EditorGUILayout.FloatField ("Gold Min Drop: ", monsterManager.monsterList [i].GoldMin);
								monsterManager.monsterList [i].GoldMax = EditorGUILayout.FloatField ("Gold Max Drop: ", monsterManager.monsterList [i].GoldMax);
								monsterManager.monsterList [i].GoldDrop = EditorGUILayout.IntField ("Gold Drop : ", Mathf.CeilToInt (Random.Range (monsterManager.monsterList [i].GoldMin, monsterManager.monsterList [i].GoldMax)));
									if (GUILayout.Button ("Remove")) {
										monsterManager.monsterList.Remove (monsterManager.monsterList [i]);
									}
								EditorGUILayout.EndScrollView ();
						}
					}
				}

				if (GUILayout.Button ("Exit")) {
					this.Close ();
				}
				break;
			}
		}
	}
}
