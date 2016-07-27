using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MonsterDataBaseManager : EditorWindow {
	[MenuItem("Monster Manager/MonsterDB")]
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


	float NewMonsterAttackMIN = 0f;
	float NewMonsterAttackMax = 0f;
	float NewMonsterAttack = 0f;
	int NewMonsterHp = 0;
	int NewMonsterDefence = 0;


	bool Elemental = false;
	bool Dark = false; 
	bool Light = false;
	bool Fire = false;
	bool Water = false;
	bool Earth = false;
	bool Air = false;

	float NewPercentEpic = 0f;
	float NewPercentRare = 0f;
	float NewPercentSimple = 0f;

	Animator newAnim;

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
				curMonsterID = EditorGUILayout.IntField ("Monster ID: ", curMonsterID);
				newMonsterName = EditorGUILayout.TextField ("Monster name: ", newMonsterName);
				newMonsterDescription = EditorGUILayout.TextField ("Monster description: ", newMonsterDescription);
				NewMonsterAttackMIN = EditorGUILayout.FloatField ("Attack Min: ", NewMonsterAttackMIN);
				NewMonsterAttackMax = EditorGUILayout.FloatField ("Attack Max: ", NewMonsterAttackMax);
				NewMonsterAttack = EditorGUILayout.FloatField ("Monster Attack: ", Random.Range (NewMonsterAttackMIN, NewMonsterAttackMax));
				NewMonsterHp = EditorGUILayout.IntField ("Monster HP: ", NewMonsterHp);
				NewMonsterDefence = EditorGUILayout.IntField ("Defence: ", NewMonsterDefence);
				#region Стихии
				Elemental = EditorGUILayout.Foldout (Elemental,"Monster Elemental? :" );
				if(Elemental){
				Dark = EditorGUILayout.Toggle ("Dark : ", Dark);
				Light = EditorGUILayout.Toggle ("Light : ", Light);
				Fire = EditorGUILayout.Toggle ("Fire : ", Fire);
				Water = EditorGUILayout.Toggle ("Water : ", Water);
				Earth = EditorGUILayout.Toggle ("Earth : ", Earth);
				Air = EditorGUILayout.Toggle ("Air : ", Air);
				}
				#endregion
				GUILayout.Space (10f);
				newAnim = (Animator)EditorGUILayout.ObjectField ("Animator: ", newAnim, typeof(Animator));
				newSpriteMonster = (Sprite)EditorGUILayout.ObjectField ("Sprite", newSpriteMonster, typeof(Sprite));
				NewPercentEpic = EditorGUILayout.Slider ("Epic Percent Drop: ", NewPercentEpic, 0.0f, 10.0f);
				NewPercentRare = EditorGUILayout.Slider ("Rare Percent Drop: ", NewPercentRare, NewPercentEpic, 100.0f - NewPercentEpic * 4);
				NewPercentSimple = EditorGUILayout.Slider ("Simple Percent Drop: ", NewPercentSimple, 100.0f - NewPercentRare - NewPercentEpic, 100.0f - NewPercentRare - NewPercentEpic);
				GUILayout.Space (10f);
				minGoldDrop = EditorGUILayout.FloatField ("Gold Min Drop: ", minGoldDrop);
				maxGoldDrop = EditorGUILayout.FloatField ("Gold Max Drop: ", maxGoldDrop);
				if (maxGoldDrop != null) {
					Gold = EditorGUILayout.IntField ("Gold Drop :", Mathf.CeilToInt (Random.Range (minGoldDrop, maxGoldDrop)));
				}

				if (GUILayout.Button ("Add New Item")) {
					Monster newMonster = (Monster)ScriptableObject.CreateInstance<Monster> ();
					GameObject newMonsterGO = new GameObject(" ");
					newMonsterGO.AddComponent <MonsterScript>();
					newMonsterGO.name = newMonsterName;
					MonsterScript ms = newMonsterGO.GetComponent<MonsterScript> ();

					ms.ID = newMonster.ID = curMonsterID;
					ms.name = newMonster.name = newMonsterName;
					ms.description = newMonster.description = newMonsterDescription;
					ms.attackMin = newMonster.attackMin = NewMonsterAttackMIN;
					ms.attackMax = newMonster.attackMax = NewMonsterAttackMax;
					ms.attack = newMonster.attack = NewMonsterAttack;
					ms.hp = newMonster.hp = NewMonsterHp;
					ms.Defence = newMonster.Defence = NewMonsterDefence;
					ms.Elemental = newMonster.Elemental = Elemental;
					ms.Dark = newMonster.Dark = Dark;
					ms.Light = newMonster.Light = Light;
					ms.Fire = newMonster.Fire = Fire;
					ms.Water = newMonster.Water = Water;
					ms.Earth = newMonster.Earth = Earth;
					ms.Air = newMonster.Air = Air;
					ms.EpicPercent = newMonster.EpicPercent = NewPercentEpic;
					ms.RarePercent = newMonster.RarePercent = NewPercentRare;
					ms.SimplePercent = newMonster.SimplePercent = NewPercentSimple;
					ms.GoldMin = newMonster.GoldMin = minGoldDrop;
					ms.GoldMax = newMonster.GoldMax = maxGoldDrop;
					ms.GoldDrop = newMonster.GoldDrop = Gold;
					ms.monsterSprite =newMonster.monsterSprite = newSpriteMonster;
					monsterManager.monsterList.Add (newMonster);
					newMonsterGO.AddComponent<BoxCollider> ();
					newMonsterGO.AddComponent<MeshFilter> ();
					newMonsterGO.AddComponent<Animation> ();
					newMonsterGO.AddComponent<MeshRenderer> ();
					if (newMonsterName == null)
						newMonsterGO.name = newMonsterName = curMonsterID.ToString();
					PrefabUtility.CreatePrefab ("Assets/Resources/Prefabs/Monsters Prefab/" + newMonsterName + ".prefab",newMonsterGO);
					AssetDatabase.Refresh ();
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
							GameObject MonsterGO = GameObject.Find (monsterManager.monsterList [i].name.ToString ());
							EditorGUILayout.BeginScrollView (Vector2.zero);
							MonsterGO.name = MonsterGO.GetComponent<MonsterScript>().name = monsterManager.monsterList [i].name = EditorGUILayout.TextField ("Name: ", monsterManager.monsterList [i].name);
							#region Основные характеристики
							MonsterGO.GetComponent<MonsterScript>().attackMin =monsterManager.monsterList [i].attackMin = EditorGUILayout.FloatField ("Minimal Attack: ", monsterManager.monsterList [i].attackMin);
							MonsterGO.GetComponent<MonsterScript>().attackMax  =monsterManager.monsterList [i].attackMax = EditorGUILayout.FloatField ("Maximal Attack: ", monsterManager.monsterList [i].attackMax);
							MonsterGO.GetComponent<MonsterScript>().attack  =monsterManager.monsterList [i].attack = EditorGUILayout.FloatField ("Attack: ", Random.Range(monsterManager.monsterList [i].attackMin, monsterManager.monsterList [i].attackMax));
							MonsterGO.GetComponent<MonsterScript>().hp = monsterManager.monsterList [i].hp = EditorGUILayout.IntField ("HP: ", monsterManager.monsterList [i].hp);
							MonsterGO.GetComponent<MonsterScript>().Defence  =monsterManager.monsterList [i].Defence = EditorGUILayout.IntField ("Defence : ", monsterManager.monsterList [i].Defence);
							#endregion
							#region Стихия
							MonsterGO.GetComponent<MonsterScript>().Elemental = monsterManager.monsterList [i].Elemental = EditorGUILayout.BeginToggleGroup("Monster Elemental? :", monsterManager.monsterList [i].Elemental);
							MonsterGO.GetComponent<MonsterScript>().Dark = monsterManager.monsterList [i].Dark = EditorGUILayout.Toggle ("Dark : ", monsterManager.monsterList [i].Dark);
							MonsterGO.GetComponent<MonsterScript>().Light = monsterManager.monsterList [i].Light = EditorGUILayout.Toggle ("Light : ", monsterManager.monsterList [i].Light);
							MonsterGO.GetComponent<MonsterScript>().Fire = monsterManager.monsterList [i].Fire = EditorGUILayout.Toggle ("Fire : ", monsterManager.monsterList [i].Fire);
							MonsterGO.GetComponent<MonsterScript>().Water = monsterManager.monsterList [i].Water = EditorGUILayout.Toggle ("Water : ", monsterManager.monsterList [i].Water);
							MonsterGO.GetComponent<MonsterScript>().Earth = monsterManager.monsterList [i].Earth = EditorGUILayout.Toggle ("Earth : ", monsterManager.monsterList [i].Earth);
							MonsterGO.GetComponent<MonsterScript>().Air = monsterManager.monsterList [i].Air = EditorGUILayout.Toggle ("Air : ", monsterManager.monsterList [i].Air);
							EditorGUILayout.EndToggleGroup();
							#endregion
							MonsterGO.GetComponent<MonsterScript>().description = monsterManager.monsterList [i].description = EditorGUILayout.TextField ("Description: ", monsterManager.monsterList [i].description);
							MonsterGO.GetComponent<MonsterScript>().monsterSprite = monsterManager.monsterList [i].monsterSprite = (Sprite)EditorGUILayout.ObjectField ("Sprite: ", monsterManager.monsterList [i].monsterSprite, typeof(Sprite));
							MonsterGO.GetComponent<MonsterScript>().EpicPercent = monsterManager.monsterList [i].EpicPercent = EditorGUILayout.Slider ("Epic Percent Drop: ", monsterManager.monsterList [i].EpicPercent, 0.0f, 10.0f);
							MonsterGO.GetComponent<MonsterScript>().RarePercent = monsterManager.monsterList [i].RarePercent = EditorGUILayout.Slider ("Epic Percent Drop: ", monsterManager.monsterList [i].RarePercent, monsterManager.monsterList [i].EpicPercent, 100.0f - monsterManager.monsterList [i].EpicPercent * 4);
							MonsterGO.GetComponent<MonsterScript>().SimplePercent = monsterManager.monsterList [i].SimplePercent = EditorGUILayout.Slider ("Epic Percent Drop: ", monsterManager.monsterList [i].SimplePercent, 100.0f - monsterManager.monsterList [i].EpicPercent - monsterManager.monsterList [i].RarePercent, 100.0f - monsterManager.monsterList [i].EpicPercent - monsterManager.monsterList [i].RarePercent);
							MonsterGO.GetComponent<MonsterScript>().GoldMin = monsterManager.monsterList [i].GoldMin = EditorGUILayout.FloatField ("Gold Min Drop: ", monsterManager.monsterList [i].GoldMin);
							MonsterGO.GetComponent<MonsterScript>().GoldMax = monsterManager.monsterList [i].GoldMax = EditorGUILayout.FloatField ("Gold Max Drop: ", monsterManager.monsterList [i].GoldMax);
							MonsterGO.GetComponent<MonsterScript>().GoldDrop = monsterManager.monsterList [i].GoldDrop = EditorGUILayout.IntField ("Gold Drop : ", Mathf.CeilToInt (Random.Range (monsterManager.monsterList [i].GoldMin, monsterManager.monsterList [i].GoldMax)));
							EditorGUILayout.BeginHorizontal ();
							if (GUILayout.Button ("Remove")) {
									monsterManager.monsterList.Remove (monsterManager.monsterList [i]);
									string path = ("Assets/Resources/Prefabs/Monsters Prefab/"+monsterManager.monsterList [i].name+".prefab");
									FileUtil.DeleteFileOrDirectory (path);
									string path2 = ("Assets/Resources/Prefabs/Monsters Prefab/" + monsterManager.monsterList [i].ID + " EmptyMonster" + ".prefab");
									FileUtil.DeleteFileOrDirectory (path2);
									Object b = GameObject.Find (monsterManager.monsterList [i].ID + " EmptyMonster");
									Object.DestroyImmediate(b);
									AssetDatabase.Refresh ();
									}
							if (GUILayout.Button ("OK")) {
				
								PrefabUtility.prefabInstanceUpdated (MonsterGO);
							}
							EditorGUILayout.EndHorizontal ();
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
