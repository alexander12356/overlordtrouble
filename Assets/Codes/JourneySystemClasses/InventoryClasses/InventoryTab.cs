using System.Collections.Generic;
using System.Linq;

public interface IInventoryTab
{
    Dictionary<string, InventoryItemData> GetItems();
}

public class AllTab : IInventoryTab
{
    public Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems();
    }
}

public class WepsTab : IInventoryTab
{
    public Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Weapon).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}

public class BlingTab : IInventoryTab
{
    public Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Bling).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}

public class SingleUseTab : IInventoryTab
{
    public Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.SingleUse).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}

public class MultiUseTab : IInventoryTab
{
    public Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.MultipleUse).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}

public class KeyItemTab : IInventoryTab
{
    public Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Key).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}
