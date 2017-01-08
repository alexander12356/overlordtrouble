using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class InventoryTabNew : MonoBehaviour
{
    public abstract Dictionary<string, InventoryItemData> GetItems();
}

public class AllTab : InventoryTabNew
{
    public override Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems();
    }
}

public class WepsTab : InventoryTabNew
{
    public override Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Weapon).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}

public class BlingTab : InventoryTabNew
{
    public override Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Bling).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}

public class SingleUseTab : InventoryTabNew
{
    public override Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.SingleUse).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}

public class MultiUseTab : InventoryTabNew
{
    public override Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.MultipleUse).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}

public class KeyItemTab : InventoryTabNew
{
    public override Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Key).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}
