using System.Collections.Generic;
using System.Linq;

public interface IInventoryItemsGetter
{
    Dictionary<string, InventoryItemData> GetItems();
}

public class AllGetter : IInventoryItemsGetter
{
    public Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems();
    }
}

public class WepsGetter : IInventoryItemsGetter
{
    public Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Weapon).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}

public class BlingGetter : IInventoryItemsGetter
{
    public Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Bling).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}

public class SingleUseGetter : IInventoryItemsGetter
{
    public Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.SingleUse).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}

public class MultiUseGetter : IInventoryItemsGetter
{
    public Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.MultipleUse).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}

public class KeyItemGetter : IInventoryItemsGetter
{
    public Dictionary<string, InventoryItemData> GetItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Key).ToDictionary(obj => obj.Key, obj => obj.Value); ;
    }
}
