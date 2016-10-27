using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEquipmentTab : InventoryTab
{
    private ButtonList mSlotButtonList;
    private ButtonList mItemButtonList;
    private InventoryPanel mInventoryPanel;
    private Dictionary<string, InventorySlotData> mChoosedItems = new Dictionary<string, InventorySlotData>();
    private Text mDescriptionText;

    public ButtonList slotButtonList
    {
        get
        {
            if (mSlotButtonList == null)
            {
                mSlotButtonList = transform.FindChild("SlotList").GetComponentInChildren<ButtonList>();
            }
            return mSlotButtonList;
        }
    }

    public ButtonList itemButtonList
    {
        get
        {
            if (mItemButtonList == null)
            {
                mItemButtonList = transform.FindChild("ItemList").GetComponentInChildren<ButtonList>();
            }
            return mItemButtonList;
        }
    }

    private Text descriptionText
    {
        get
        {
            if(mDescriptionText == null)
            {
                mDescriptionText = transform.FindChild("Description").GetComponent<Text>();
            }
            return mDescriptionText;
        }
    }

    public void Awake()
    {
        mSlotButtonList = slotButtonList;
        mSlotButtonList.AddCancelAction(CancelAction);
        mSlotButtonList.AddKeyArrowAction(InitItemsList);
        mSlotButtonList.isActive = false;
        InitItemsList();
        mItemButtonList = itemButtonList;
        mItemButtonList.AddCancelAction(DeselectItemList);
        mItemButtonList.AddKeyArrowAction(ShowItemDescription);
        mItemButtonList.isActive = false;
        mDescriptionText = descriptionText;
        mDescriptionText.text = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Description"); // TODO : Добавить текст
        mInventoryPanel = GetComponentInParent<InventoryPanel>();
    }

    // TODO : Сделать нормальное описание предметов
    private void ShowItemDescription()
    {
        InventoryEquipmentButton lEqButton = (InventoryEquipmentButton)mItemButtonList.currentButton;
        int lCountInInventory = PlayerInventory.GetInstance().GetItemCount(lEqButton.itemId);
        string lDescriptionText = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Description");
        string lInInventoryText = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:InInventory");
        mDescriptionText.text = lDescriptionText + lEqButton.title + "_Description" + lInInventoryText + lCountInInventory;
    }

    public void AddSlot(eSlotType pSlotType, string pSlotTitle)
    {
        InventorySlotButton lButton = Instantiate(InventorySlotButton.prefab);
        lButton.slotType = pSlotType;
        lButton.title = pSlotTitle;
        lButton.AddAction(SelectItemList);

        slotButtonList.AddButton(lButton);
    }

    private void AddItem(InventoryItemData pInventoryItemData)
    {
        InventoryEquipmentButton lButton = Instantiate(InventoryEquipmentButton.prefab);
        lButton.title = LocalizationDataBase.GetInstance().GetText("Item:" + pInventoryItemData.id);
        lButton.itemId = pInventoryItemData.id;
        lButton.AddAction(SelectItem);

        if (mChoosedItems.ContainsKey(lButton.itemId))
            lButton.equipped = true;

        itemButtonList.AddButton(lButton);
    }

    private void InitItemsList()
    {
        itemButtonList.Clear();
        InventorySlotButton lSlotButton = (InventorySlotButton)mSlotButtonList.currentButton;
        Dictionary<string, InventoryItemData> lInventoryItems = new Dictionary<string, InventoryItemData>();
        switch (lSlotButton.slotType)
        {
            case eSlotType.normal:
                lInventoryItems = PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Equipment).ToDictionary(obj => obj.Key, obj => obj.Value);
                break;
            case eSlotType.weapon:
                lInventoryItems = PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Weapon).ToDictionary(obj => obj.Key, obj => obj.Value);
                break;
            case eSlotType.universal:
                lInventoryItems = PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Equipment || ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Weapon).ToDictionary(obj => obj.Key, obj => obj.Value);
                break;
        }
        foreach(var lKey in lInventoryItems.Keys)
        {
            if (mChoosedItems.ContainsKey(lKey) && mChoosedItems[lKey].itemId != lSlotButton.itemId)
                continue;       
            AddItem(lInventoryItems[lKey]);
        }
    }

    private void SelectItemList()
    {
        itemButtonList.isActive = true;
        slotButtonList.isActive = false;
        slotButtonList.currentButton.selected = true;
        ShowItemDescription();
    }

    private void DeselectItemList()
    {
        itemButtonList.isActive = false;
        slotButtonList.isActive = true;
        mDescriptionText.text = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Description"); 
    }

    public override void CancelAction()
    {
        mSlotButtonList.isActive = false;
        mInventoryPanel.tabButtonList.isActive = true;
    }

    public override void Confrim()
    {
        mSlotButtonList.isActive = true;
    }

    public override void DeselectItem()
    {
        // TODO : Отменить экипировку этим предметом
    }

    public override void SelectItem()
    {
        // TODO : Экипировать игрока выбранным предметом
        InventoryEquipmentButton lEqButton = (InventoryEquipmentButton)mItemButtonList.currentButton;
        InventorySlotButton lSlotButton = (InventorySlotButton)mSlotButtonList.currentButton;
        
        if(lSlotButton.IsFull)
        {           
            if (lEqButton.equipped)
            {
                mChoosedItems.Remove(lSlotButton.itemId);
                lSlotButton.DeselectItem();
                lEqButton.equipped = false;
            }
            else
            {
                ClearEquipped();
                mChoosedItems.Remove(lSlotButton.itemId);
                lSlotButton.SelectItem(lEqButton.itemId);
                lEqButton.equipped = true;
                mChoosedItems.Add(lEqButton.itemId, lSlotButton.slotData);
            }
        }
        else
        {
            lEqButton.equipped = true;
            lSlotButton.SelectItem(lEqButton.itemId);
            mChoosedItems.Add(lEqButton.itemId, lSlotButton.slotData);
        }
    }

    private void ClearEquipped()
    {
        for (int i = 0; i < mItemButtonList.count; i++)
        {
            (mItemButtonList[i] as InventoryEquipmentButton).equipped = false;
        }
    }

    public override void UpdateKey()
    {
        if(slotButtonList.isActive)
            slotButtonList.UpdateKey();
        if (itemButtonList.isActive)
            itemButtonList.UpdateKey();
    }
}
