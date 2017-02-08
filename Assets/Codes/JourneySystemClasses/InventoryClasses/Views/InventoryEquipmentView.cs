using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEquipmentView : InventoryView
{
    public InventoryEquipmentView(InventoryPanel p_Parent)
    {
        parent = p_Parent;
    }

    public override void Init()
    {
        InitGroupData();
        InitGroupButtonList();
        InitSlotButtonList();
        InitItemButtonList();
    }

    private void InitSlotButtonList()
    {
        slotButtonList.AddCancelAction(DeselectSlotList);
        slotButtonList.AddKeyArrowAction(InitItemList);
        slotButtonList.isActive = false;
    }

    public override void InitItemList()
    {
        itemButtonList.Clear();
        InventorySlotButton l_SlotButton = (InventorySlotButton)slotButtonList.currentButton;
        Dictionary<string, InventoryItemData> l_InventoryItems = new Dictionary<string, InventoryItemData>();
        l_InventoryItems = l_SlotButton.slotType.GetInventoryItemData();
        foreach (var lKey in l_InventoryItems.Keys)
        {
            AddItem(l_InventoryItems[lKey]);
        }
        // Add deselect button
        AddItem(DeselectItem, "----------");
    }

    public override void GroupMemberButtonAction()
    {
        groupButtonList.isActive = false;
        slotButtonList.isActive = true;
        InitItemList();
    }

    private void DeselectSlotList()
    {
        groupButtonList.isActive = true;
        slotButtonList.isActive = false;
        itemButtonList.Clear();
    }

    public override void Disable()
    {
        slotButtonList.RemoveCancelAction(DeselectSlotList);
        slotButtonList.RemoveKeyArrowAction(InitItemList);
        itemButtonList.RemoveCancelAction(ItemButtonListCancelAction);
        itemButtonList.RemoveKeyArrowAction(ShowDescription);
    }

    public override void AddSlot(InventorySlotData p_SlotData)
    {
        base.AddSlot(p_SlotData);

        InventorySlotButton l_Button = UnityEngine.Object.Instantiate(InventorySlotButton.prefab);
        l_Button.slotData = p_SlotData;
        l_Button.AddAction(SelectItemList);
        l_Button.defaultTitle = p_SlotData.slotType.GetTitle(slotButtonList.count);
        slotButtonList.AddButton(l_Button);
    }

    public override void AddItem(InventoryItemData pInventoryItemData)
    {
        InventoryItemButton l_ItemButton = UnityEngine.Object.Instantiate(InventoryItemButton.prefab);
        l_ItemButton.title = LocalizationDataBase.GetInstance().GetText("Item:" + pInventoryItemData.id);
        l_ItemButton.itemId = pInventoryItemData.id;
        l_ItemButton.AddAction(SelectItem);

        itemButtonList.AddButton(l_ItemButton);
    }

    public void AddItem(PanelButtonActionHandler p_Action, string p_Title)
    {
        InventoryItemButton l_ItemButton = UnityEngine.Object.Instantiate(InventoryItemButton.prefab);
        l_ItemButton.title = p_Title;
        l_ItemButton.AddAction(p_Action);
        itemButtonList.AddButton(l_ItemButton);
    }

    private void SelectItemList()
    {
        itemButtonList.isActive = true;
        slotButtonList.isActive = false;
        slotButtonList.currentButton.selected = true;
        ShowDescription();
    }

    public override void ItemButtonListCancelAction()
    {
        itemButtonList.isActive = false;
        slotButtonList.isActive = true;
        descriptionText.text = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Description");
    }

    public override void GroupButtonListCancelAction()
    {
        groupButtonList.isActive = false;
        parent.tabButtonList.isActive = true;
        InitEmptySlots();
        ClearGroupMemberInfo();
    }

    public override bool Confirm()
    {
        groupButtonList.isActive = true;
        ShowGroupMemberInfo();
        return true;
    }

    public override void SelectItem()
    {
        // TODO : Экипировать игрока выбранным предметом
        InventoryItemButton l_ItemButton = (InventoryItemButton)itemButtonList.currentButton;
        InventorySlotButton l_SlotButton = (InventorySlotButton)slotButtonList.currentButton;    
        ChangeItemCount(l_SlotButton.itemId, l_ItemButton.itemId);
        l_SlotButton.SelectItem(l_ItemButton.itemId);
        // TODO: Переделать под группу
        PlayerInventory.GetInstance().UpdateSlotData(l_SlotButton.slotId, l_SlotButton.slotData);
    }

    public void DeselectItem()
    {
        InventoryItemButton l_ItemButton = (InventoryItemButton)itemButtonList.currentButton;
        InventorySlotButton l_SlotButton = (InventorySlotButton)slotButtonList.currentButton;
        ChangeItemCount(l_SlotButton.itemId, l_ItemButton.itemId);
        l_SlotButton.DeselectItem();
        PlayerInventory.GetInstance().UpdateSlotData(l_SlotButton.slotId, l_SlotButton.slotData);
    }

    private void ChangeItemCount(string p_SlotItemId, string p_ItemId)
    {
        if (p_ItemId == p_SlotItemId)
            return;
        if (IsSelectAnotherItem(p_SlotItemId, p_ItemId))
        {
            PlayerInventory.GetInstance().SetItemCount(p_SlotItemId, PlayerInventory.GetInstance().GetItemCount(p_SlotItemId) + 1);
            PlayerInventory.GetInstance().SetItemCount(p_ItemId, PlayerInventory.GetInstance().GetItemCount(p_ItemId) - 1);
            CheckItemButtonList(p_SlotItemId, PlayerInventory.GetInstance().GetItemCount(p_SlotItemId));
            CheckItemButtonList(p_ItemId, PlayerInventory.GetInstance().GetItemCount(p_ItemId));
        }
        else if (IsSlotItemDeselect(p_SlotItemId, p_ItemId))
        {
            PlayerInventory.GetInstance().SetItemCount(p_SlotItemId, PlayerInventory.GetInstance().GetItemCount(p_SlotItemId) + 1);
            CheckItemButtonList(p_SlotItemId, PlayerInventory.GetInstance().GetItemCount(p_SlotItemId));
        }
        else if (IsSelectNewSlotItem(p_SlotItemId, p_ItemId))
        {
            PlayerInventory.GetInstance().SetItemCount(p_ItemId, PlayerInventory.GetInstance().GetItemCount(p_ItemId) - 1);
            CheckItemButtonList(p_ItemId, PlayerInventory.GetInstance().GetItemCount(p_ItemId));
        }
    }

    private void CheckItemButtonList(string p_ItemId, int p_ItemCount)
    {
        InventoryItemButton l_ItemButton = null;
        for(int i = 0; i < itemButtonList.count; i++)
        {
            if((itemButtonList[i] as InventoryItemButton).itemId == p_ItemId)
            {
                l_ItemButton = itemButtonList[i] as InventoryItemButton;
                break;
            }
        }
        if (p_ItemCount <= 0)
        {
            itemButtonList.RemoveButton(l_ItemButton);
            if(itemButtonList.count <= 0)
            {
                ItemButtonListCancelAction();
            }
        }
        else
        {
            l_ItemButton.itemCount = p_ItemCount;
            ShowDescription();
        }
    }

    private bool IsSelectNewSlotItem(string p_SlotItemId, string p_ItemId)
    {
        return p_SlotItemId == String.Empty && p_ItemId != String.Empty;
    }

    private bool IsSlotItemDeselect(string p_SlotItemId, string p_ItemId)
    {
        return p_SlotItemId != String.Empty && p_ItemId == String.Empty;
    }

    private bool IsSelectAnotherItem(string p_SlotItemId, string p_ItemId)
    {
        return p_SlotItemId != String.Empty && p_ItemId != String.Empty;
    }

    public override void UpdateKey()
    {
        slotButtonList.UpdateKey();
        itemButtonList.UpdateKey();
        groupButtonList.UpdateKey();
    }

    public override bool isNull()
    {
        return false;
    }
}
