using System;
using System.Collections.Generic;
using System.Linq;

public interface IItemsGetter
{
    Dictionary<string, InventoryItemData> GetInventoryItems();
    Dictionary<string, StoreItemData> GetStoreItems();
}

public class AllGetter : IItemsGetter
{
    public Dictionary<string, InventoryItemData> GetInventoryItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems();
    }

    public Dictionary<string, StoreItemData> GetStoreItems()
    {
        return StoreDataBase.GetInstance().GetStoreItem();
    }
}

public class AllGetterForStore : IItemsGetter
{
    public Dictionary<string, InventoryItemData> GetInventoryItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType != ItemType.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
    }

    public Dictionary<string, StoreItemData> GetStoreItems()
    {
        return StoreDataBase.GetInstance().GetStoreItem();
    }
}

public class WepsGetter : IItemsGetter
{
    public Dictionary<string, InventoryItemData> GetInventoryItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Weapon).ToDictionary(obj => obj.Key, obj => obj.Value);
    }

    public Dictionary<string, StoreItemData> GetStoreItems()
    {
        return StoreDataBase.GetInstance().GetStoreItem().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Weapon).ToDictionary(obj => obj.Key, obj => obj.Value);
    }
}

public class BlingGetter : IItemsGetter
{
    public Dictionary<string, InventoryItemData> GetInventoryItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Bling).ToDictionary(obj => obj.Key, obj => obj.Value); 
    }

    public Dictionary<string, StoreItemData> GetStoreItems()
    {
        return StoreDataBase.GetInstance().GetStoreItem().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Bling).ToDictionary(obj => obj.Key, obj => obj.Value);
    }
}

public class SingleUseGetter : IItemsGetter
{
    public Dictionary<string, InventoryItemData> GetInventoryItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.SingleUse).ToDictionary(obj => obj.Key, obj => obj.Value); 
    }

    public Dictionary<string, StoreItemData> GetStoreItems()
    {
        return StoreDataBase.GetInstance().GetStoreItem().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.SingleUse).ToDictionary(obj => obj.Key, obj => obj.Value);
    }
}

public class MultiUseGetter : IItemsGetter
{
    public Dictionary<string, InventoryItemData> GetInventoryItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.MultipleUse).ToDictionary(obj => obj.Key, obj => obj.Value); 
    }

    public Dictionary<string, StoreItemData> GetStoreItems()
    {
        return StoreDataBase.GetInstance().GetStoreItem().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.MultipleUse).ToDictionary(obj => obj.Key, obj => obj.Value);
    }
}

public class KeyItemGetter : IItemsGetter
{
    public Dictionary<string, InventoryItemData> GetInventoryItems()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Key).ToDictionary(obj => obj.Key, obj => obj.Value); 
    }

    public Dictionary<string, StoreItemData> GetStoreItems()
    {
        return null;
    }
}
