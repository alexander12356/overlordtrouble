#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class ItemDatabaseManager : EditorWindow  {
	[MenuItem("RPG Manager/Item Editor")]
	static void Init()
	{
		ItemDatabaseManager window = (ItemDatabaseManager)EditorWindow.CreateInstance (typeof(ItemDatabaseManager));
		window.Show ();
	}

	public ItemManager itemManager;
	int newID = 0;
	string newItemName = null;
	int newItemCost = 0;
	string newItemDescription = null;
	int newWeaponDamage = 0;
	int newArmorDefence = 0;
	int newItemCharges = 0;
	int deltaID = 0;
	bool findedID = false;

	public enum ItemCreateOrEdit
	{
		CreateNewItem,
		EditItem
	}
	public ItemCreateOrEdit currentChoose = ItemCreateOrEdit.CreateNewItem;
	public enum ItemTypeofCreate
	{
		Weapon,
		Armor,
		Consumable,
		KeyItem
	}



	public ItemTypeofCreate currentTypetoCreate = ItemTypeofCreate.Weapon;
	void OnGUI(){
		
		itemManager = (ItemManager)EditorGUILayout.ObjectField (itemManager, typeof(ItemManager)) as ItemManager;

		if (itemManager != null) {
			currentChoose = (ItemCreateOrEdit)EditorGUILayout.EnumPopup (currentChoose);
			switch (currentChoose) {
			case ItemCreateOrEdit.CreateNewItem:
				currentTypetoCreate = (ItemTypeofCreate)EditorGUILayout.EnumPopup (currentTypetoCreate);
				newID = itemManager.itemList.Count + 1;
				EditorGUILayout.LabelField ("ID: " + newID);
				newItemName = EditorGUILayout.TextField ("New Item Name: ", newItemName);
				newItemDescription = EditorGUILayout.TextField ("New Item Description: ", newItemDescription);
				newItemCost = EditorGUILayout.IntField ("New Item Cost: ", newItemCost);

				switch (currentTypetoCreate) {
				case ItemTypeofCreate.Weapon:
					newWeaponDamage = EditorGUILayout.IntField ("New Weapon Damage: ", newWeaponDamage);
					break;
				case ItemTypeofCreate.Armor:
					newArmorDefence = EditorGUILayout.IntField ("New Armor Defence: ", newArmorDefence);
					break;
				case ItemTypeofCreate.Consumable:
					newItemCharges = EditorGUILayout.IntField ("New Item Charges: ", newItemCharges);
					break;
				case ItemTypeofCreate.KeyItem:
					
					break;
				}


				if (GUILayout.Button ("Add New Item")) {
					switch (currentTypetoCreate) {
					case ItemTypeofCreate.Weapon:
						Weapon newweapon = (Weapon)ScriptableObject.CreateInstance<Weapon> ();
						newweapon.name = newItemName;
						newweapon.description = newItemDescription;
						newweapon.cost = newItemCost;
						newweapon.ID = newID;
						itemManager.itemList.Add (newweapon);
						break;
					case ItemTypeofCreate.Armor:
						Armor newArmor = (Armor)ScriptableObject.CreateInstance<Armor> ();
						newArmor.name = newItemName;
						newArmor.description = newItemDescription;
						newArmor.cost = newItemCost;
						newArmor.ID = newID;
						itemManager.itemList.Add (newArmor);
						break;
					case ItemTypeofCreate.Consumable:
						Consumable newConsumable = (Consumable)ScriptableObject.CreateInstance<Consumable> ();
						newConsumable.name = newItemName;
						newConsumable.description = newItemDescription;
						newConsumable.cost = newItemCost;
						newConsumable.ID = newID;
						itemManager.itemList.Add (newConsumable);
						break;
					case ItemTypeofCreate.KeyItem:
						Item newItem = (Item)ScriptableObject.CreateInstance<Item> ();
						newItem.name = newItemName;
						newItem.description = newItemDescription;
						newItem.cost = newItemCost;
						newItem.ID = newID;
						itemManager.itemList.Add (newItem);
						break;
					}

				}
				break;
			case ItemCreateOrEdit.EditItem:
				deltaID = EditorGUILayout.IntField ("Enter ID: ", deltaID);

				List<Weapon> Weapons = new List<Weapon> ();
				List<Armor> Armors = new List<Armor> ();
				List<Consumable> Consumables = new List<Consumable> ();
				for (int i = 0; i < itemManager.itemList.Count; i++) {
					if (itemManager.itemList [i].GetType () == typeof(Weapon)) {
						Weapons.Add ((Weapon)itemManager.itemList [i]);
					}
					if (itemManager.itemList [i].GetType () == typeof(Armor)) {
						Armors.Add ((Armor)itemManager.itemList [i]);
					}
					if (itemManager.itemList [i].GetType () == typeof(Consumable)) {
						Consumables.Add ((Consumable)itemManager.itemList [i]);
					}
				}
				for (int j = 0; j < itemManager.itemList.Count; j++) {
					if (itemManager.itemList [j].ID == deltaID) {
						currentTypetoCreate = (ItemTypeofCreate)EditorGUILayout.EnumPopup (currentTypetoCreate);
						switch (currentTypetoCreate) {
						case ItemTypeofCreate.Weapon:
							for (int i = 0; i < Weapons.Count; i++) {
								Weapons [i].name = EditorGUILayout.TextField ("Item: ", Weapons [i].name);
								Weapons [i].description = EditorGUILayout.TextField ("Description: ", Weapons [i].description);
								Weapons [i].cost = EditorGUILayout.IntField ("Cost: ", Weapons [i].cost);
								Weapons [i].Damage = EditorGUILayout.IntField ("Damage: ", Weapons [i].Damage);
							}
							break;
						case ItemTypeofCreate.Armor:
							break;
						case ItemTypeofCreate.Consumable:
							break;
						}
					}
				}
				break;
			}
		}
	}
}
#endif