using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEquipmentTab : InventoryTab
{
    private ButtonList m_SlotButtonList;
    private ButtonList m_ItemButtonList;
    private InventoryPanel m_InventoryPanel;
    private Text m_DescriptionText;

    public ButtonList slotButtonList
    {
        get
        {
            if (m_SlotButtonList == null)
            {
                m_SlotButtonList = transform.FindChild("SlotList").GetComponentInChildren<ButtonList>();
            }
            return m_SlotButtonList;
        }
    }

    public ButtonList itemButtonList
    {
        get
        {
            if (m_ItemButtonList == null)
            {
                m_ItemButtonList = transform.FindChild("ItemList").GetComponentInChildren<ButtonList>();
            }
            return m_ItemButtonList;
        }
    }

    private Text descriptionText
    {
        get
        {
            if(m_DescriptionText == null)
            {
                m_DescriptionText = transform.FindChild("Description").GetComponent<Text>();
            }
            return m_DescriptionText;
        }
    }

    public void Awake()
    {
        m_SlotButtonList = slotButtonList;
        m_SlotButtonList.AddCancelAction(CancelAction);
        m_SlotButtonList.AddKeyArrowAction(InitItemsList);
        m_SlotButtonList.isActive = false;
        InitSlots();
        m_ItemButtonList = itemButtonList;
        m_ItemButtonList.AddCancelAction(DeselectItemList);
        m_ItemButtonList.AddKeyArrowAction(ShowItemDescription);
        m_ItemButtonList.isActive = false;
        InitItemsList();
        m_DescriptionText = descriptionText;
        m_DescriptionText.text = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Description"); // TODO : Добавить текст
        m_InventoryPanel = GetComponentInParent<InventoryPanel>();
    }

    // TODO : Сделать нормальное описание предметов
    private void ShowItemDescription()
    {
        if (m_ItemButtonList != null && m_ItemButtonList.count > 0)
        {
            InventoryEquipmentButton lEqButton = (InventoryEquipmentButton)m_ItemButtonList.currentButton;
            int lCountInInventory = PlayerInventory.GetInstance().GetItemCount(lEqButton.itemId);
            string lDescriptionText = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Description");
            string lInInventoryText = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:InInventory");
            m_DescriptionText.text = lDescriptionText + lEqButton.title + "_Description" + lInInventoryText + lCountInInventory;
        }
    }

    private void InitSlots()
    {
        Dictionary<string, InventorySlotData> lSlotData = PlayerInventory.GetInstance().GetInventorySlotData();
        foreach (var lKey in lSlotData.Keys)
        {
            AddSlot(lSlotData[lKey]);
        }
    }

    public void AddSlot(InventorySlotData pSlotData)
    {
        InventorySlotButton l_Button = Instantiate(InventorySlotButton.prefab);

        if (PlayerInventory.GetInstance().GetItemCount(pSlotData.itemId) == 0)
            pSlotData.itemId = string.Empty;

        l_Button.slotData = pSlotData;
        l_Button.AddAction(SelectItemList);

        switch (pSlotData.slotType)
        {
            case eSlotType.normal:
                l_Button.title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Slot") + " " + slotButtonList.count;
                break;
            case eSlotType.weapon:
                l_Button.title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:SlotWeapon");
                break;
            case eSlotType.universal:
                l_Button.title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Slot") + " " + slotButtonList.count + 1;
                break;
        }

        slotButtonList.AddButton(l_Button);
    }

    private void InitItemsList()
    {
        itemButtonList.Clear();
        InventorySlotButton l_SlotButton = (InventorySlotButton)m_SlotButtonList.currentButton;
        Dictionary<string, InventoryItemData> l_InventoryItems = new Dictionary<string, InventoryItemData>();
        switch (l_SlotButton.slotType)
        {
            case eSlotType.normal:
                l_InventoryItems = PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Equipment).ToDictionary(obj => obj.Key, obj => obj.Value);
                break;
            case eSlotType.weapon:
                l_InventoryItems = PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Weapon).ToDictionary(obj => obj.Key, obj => obj.Value);
                break;
            case eSlotType.universal:
                l_InventoryItems = PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Equipment || ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Weapon).ToDictionary(obj => obj.Key, obj => obj.Value);
                break;
        }
        foreach (var lKey in l_InventoryItems.Keys)
        {
            if (PlayerInventory.GetInstance().ItemAlreadyUsed(l_SlotButton.slotId, l_InventoryItems[lKey].id))
                continue;
            AddItem(l_InventoryItems[lKey]);
        }
    }

    private void AddItem(InventoryItemData pInventoryItemData)
    {
        InventoryEquipmentButton l_Button = Instantiate(InventoryEquipmentButton.prefab);
        l_Button.title = LocalizationDataBase.GetInstance().GetText("Item:" + pInventoryItemData.id);
        l_Button.itemId = pInventoryItemData.id;
        l_Button.AddAction(SelectItem);

        if (PlayerInventory.GetInstance().SlotsContainItem(l_Button.itemId))
            l_Button.equipped = true;

        itemButtonList.AddButton(l_Button);
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
        m_DescriptionText.text = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Description"); 
    }

    public override void CancelAction()
    {
        m_SlotButtonList.isActive = false;
        m_InventoryPanel.tabButtonList.isActive = true;
    }

    public override void Confrim()
    {
        m_SlotButtonList.isActive = true;
    }

    public override void SelectItem()
    {
        // TODO : Экипировать игрока выбранным предметом
        InventoryEquipmentButton l_EqButton = (InventoryEquipmentButton)m_ItemButtonList.currentButton;
        InventorySlotButton l_SlotButton = (InventorySlotButton)m_SlotButtonList.currentButton;
        
        if(l_SlotButton.IsFull)
        {           
            if (l_EqButton.equipped)
            {
                l_SlotButton.DeselectItem();
                l_EqButton.equipped = false;
                PlayerInventory.GetInstance().UpdateSlotData(l_SlotButton.slotId, l_SlotButton.slotData);
            }
            else
            {
                ClearEquipped();
                l_SlotButton.SelectItem(l_EqButton.itemId);
                l_EqButton.equipped = true;
                PlayerInventory.GetInstance().UpdateSlotData(l_SlotButton.slotId, l_SlotButton.slotData);
            }
        }
        else
        {
            l_EqButton.equipped = true;
            l_SlotButton.SelectItem(l_EqButton.itemId);
            PlayerInventory.GetInstance().UpdateSlotData(l_SlotButton.slotId, l_SlotButton.slotData);
        }
    }

    private void ClearEquipped()
    {
        for (int i = 0; i < m_ItemButtonList.count; i++)
        {
            (m_ItemButtonList[i] as InventoryEquipmentButton).equipped = false;
        }
    }

    public override void UpdateKey()
    {
        slotButtonList.UpdateKey();
        itemButtonList.UpdateKey();
    }

    public override void DeselectItem()
    {
        
    }
}
