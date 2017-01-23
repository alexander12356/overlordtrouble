using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class Slot
{
    public abstract string GetTitle(int p_SlotsCount);
    public abstract Dictionary<string, InventoryItemData> GetInventoryItemData();
    public abstract string GetType();
}

class NormalSlot : Slot
{
    public override Dictionary<string, InventoryItemData> GetInventoryItemData()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Bling).ToDictionary(obj => obj.Key, obj => obj.Value);
    }

    public override string GetTitle(int p_SlotsCount)
    {
        return LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Slot") + " " + p_SlotsCount.ToString();
    }

    public override string GetType()
    {
        return "normal";
    }
}

class WeaponSlot : Slot
{
    public override Dictionary<string, InventoryItemData> GetInventoryItemData()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Weapon).ToDictionary(obj => obj.Key, obj => obj.Value);
    }

    public override string GetTitle(int p_SlotsCount)
    {
        return LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:SlotWeapon");
    }

    public override string GetType()
    {
        return "weapon";
    }
}

class UniversalSlot : Slot
{
    public override Dictionary<string, InventoryItemData> GetInventoryItemData()
    {
        return PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Bling || ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Weapon).ToDictionary(obj => obj.Key, obj => obj.Value);
    }

    public override string GetTitle(int p_SlotsCount)
    {
        return LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Slot") + " " + (p_SlotsCount + 1).ToString();
    }

    public override string GetType()
    {
        return "universal";
    }
}
