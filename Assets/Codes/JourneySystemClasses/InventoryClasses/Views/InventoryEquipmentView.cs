using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEquipmentView : InventoryView
{
    private ButtonList m_SlotButtonList;
    private ButtonList m_ItemButtonList;
    private Text m_DescriptionText;

    public ButtonList slotButtonList
    {
        get
        {
            if (m_SlotButtonList == null)
            {
                m_SlotButtonList = parent.transform.FindChild("SlotList").GetComponentInChildren<ButtonList>();
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
                m_ItemButtonList = parent.transform.FindChild("ItemList").GetComponentInChildren<ButtonList>();
            }
            return m_ItemButtonList;
        }
    }

    private Text descriptionText
    {
        get
        {
            if (m_DescriptionText == null)
            {
                m_DescriptionText = parent.transform.FindChild("Description").GetComponent<Text>();
            }
            return m_DescriptionText;
        }
    }

    public InventoryEquipmentView(InventoryPanel p_Parent)
    {
        parent = p_Parent;
    }

    public override void Init()
    {
        m_SlotButtonList = slotButtonList;
        m_SlotButtonList.AddCancelAction(CancelAction);
        m_SlotButtonList.AddKeyArrowAction(InitItemsList);
        m_SlotButtonList.isActive = false;
        InitSlots();
        m_ItemButtonList = itemButtonList;
        m_ItemButtonList.AddCancelAction(DeselectItemList);
        m_ItemButtonList.AddKeyArrowAction(ShowDescription);
        m_ItemButtonList.isActive = false;
        InitItemsList();
        m_DescriptionText = descriptionText;
    }

    public override void Disable()
    {
        m_SlotButtonList.RemoveCancelAction(CancelAction);
        m_SlotButtonList.RemoveKeyArrowAction(InitItemsList);
        m_ItemButtonList.RemoveCancelAction(DeselectItemList);
        m_ItemButtonList.RemoveKeyArrowAction(ShowDescription);
    }

    // TODO : Сделать нормальное описание предметов
    public override void ShowDescription()
    {
        if (m_ItemButtonList != null && m_ItemButtonList.count > 0)
        {
            InventoryEquipmentButton lEqButton = (InventoryEquipmentButton)m_ItemButtonList.currentButton;
            int lCountInInventory = PlayerInventory.GetInstance().GetItemCount(lEqButton.itemId);
            string lInInventoryText = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:InInventory");
            m_DescriptionText.text = lEqButton.title + "_Description" + lInInventoryText + lCountInInventory;
        }
    }

    public override void ClearDescription()
    {
        m_DescriptionText.text = "";
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
        InventorySlotButton l_Button = Object.Instantiate(InventorySlotButton.prefab);

        if (PlayerInventory.GetInstance().GetItemCount(pSlotData.itemId) == 0)
            pSlotData.itemId = string.Empty;

        l_Button.slotData = pSlotData;
        l_Button.AddAction(SelectItemList);
        l_Button.title = pSlotData.slotType.GetTitle(slotButtonList.count);

        slotButtonList.AddButton(l_Button);
    }

    private void InitItemsList()
    {
        itemButtonList.Clear();
        InventorySlotButton l_SlotButton = (InventorySlotButton)m_SlotButtonList.currentButton;
        Dictionary<string, InventoryItemData> l_InventoryItems = new Dictionary<string, InventoryItemData>();
        l_InventoryItems = l_SlotButton.slotType.GetInventoryItemData();
        foreach (var lKey in l_InventoryItems.Keys)
        {
            if (PlayerInventory.GetInstance().ItemAlreadyUsed(l_SlotButton.slotId, l_InventoryItems[lKey].id))
                continue;
            AddItem(l_InventoryItems[lKey]);
        }
    }

    public override void AddItem(InventoryItemData pInventoryItemData)
    {
        InventoryEquipmentButton l_Button = Object.Instantiate(InventoryEquipmentButton.prefab);
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
        ShowDescription();
    }

    private void DeselectItemList()
    {
        itemButtonList.isActive = false;
        slotButtonList.isActive = true;
        m_DescriptionText.text = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Description");
    }

    public override void CancelAction()
    {
        m_SlotButtonList.isActive = false;
        parent.tabButtonList.isActive = true;
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

        if (l_SlotButton.IsFull)
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
}
