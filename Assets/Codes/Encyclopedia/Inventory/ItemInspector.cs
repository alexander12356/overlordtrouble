using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ItemManager))]
internal class ItemInspector : Editor {

	bool ShowingWeapons = false;
	bool ShowingArmors = false;
	bool ShowingConsumables = false;
	public int WeaponsCount = 0;
	public int ArmorsCount = 0;
	public int ConsumablesCount = 0;
	public override void OnInspectorGUI(){
		ItemManager im = target as ItemManager;

		List<Weapon> Weapons = new List<Weapon> ();
		List<Armor> Armors = new List<Armor> ();
		List<Consumable> Consumables = new List<Consumable> ();
			for (int i = 0; i < im.itemList.Count; i++) {
				if (im.itemList [i].GetType () == typeof(Weapon)) {
				Weapons.Add ((Weapon)im.itemList [i]);
				}
				if (im.itemList [i].GetType () == typeof(Armor)) {
				Armors.Add ((Armor)im.itemList [i]);
				}
				
				if (im.itemList [i].GetType () == typeof(Consumable)) {
				Consumables.Add ((Consumable)im.itemList [i]);
				}
			}
		WeaponsCount = Weapons.Count;
		ShowingWeapons = EditorGUILayout.Foldout (ShowingWeapons, "Weapons: ");
		if(ShowingWeapons == true){
			EditorGUI.indentLevel = 1;
			for (int i = 0; i < Weapons.Count; i++) {
				Weapons[i].name = EditorGUILayout.TextField ("Name: ",Weapons [i].name);
				Weapons[i].description = EditorGUILayout.TextField ("Description: ",Weapons [i].description);
				Weapons[i].cost = EditorGUILayout.IntField ("Cost: ",Weapons [i].cost);
				Weapons [i].Damage = EditorGUILayout.IntField ("Damage: ", Weapons [i].Damage);
				EditorGUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Advanced")) {
					ItemDatabaseManager window = (ItemDatabaseManager)EditorWindow.CreateInstance (typeof(ItemDatabaseManager));
					window.Show ();
				}
				if (GUILayout.Button ("Remove")) {
					im.itemList.Remove (Weapons [i]);
				}
				EditorGUILayout.EndHorizontal ();
				GUILayout.Space (10);
			}
			if (GUILayout.Button ("Add New Weapon")) {
				Weapon newweapon = (Weapon)ScriptableObject.CreateInstance<Weapon> ();
				newweapon.name = null;
				newweapon.description = null;
				newweapon.cost = 0;
				newweapon.Damage = 0;
				im.itemList.Add (newweapon);
			}

			EditorGUI.indentLevel = 0;
		}
		ShowingArmors = EditorGUILayout.Foldout (ShowingArmors, "Armors: ");
		if (ShowingArmors == true) {
			EditorGUI.indentLevel = 1;
			for (int i = 0; i < Armors.Count; i++) {
				EditorGUILayout.LabelField (Armors [i].name);
			}
			EditorGUI.indentLevel = 0;
		}
		ShowingConsumables = EditorGUILayout.Foldout (ShowingConsumables, "Consumables: ");
		if (ShowingConsumables == true) {
			EditorGUI.indentLevel = 1;
			for (int i = 0; i < Consumables.Count; i++) {
				EditorGUILayout.LabelField (Consumables [i].name);
			}
			EditorGUI.indentLevel = 0;
		}
	}

}
