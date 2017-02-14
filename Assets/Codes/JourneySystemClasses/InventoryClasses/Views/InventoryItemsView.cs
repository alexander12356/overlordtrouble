using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemsView : InventoryView
{
    #region Variables
    private IItemsGetter m_InventoryItemsGetter = null;
    #endregion

    #region Interface

    public IItemsGetter inventoryItemsGetter
    {
        get
        {
            return m_InventoryItemsGetter;
        }
        set
        {
            m_InventoryItemsGetter = value;
        }
    }
    #endregion

    public InventoryItemsView(InventoryPanel p_Parent, IItemsGetter p_Getter)
    {
        parent = p_Parent;
        inventoryItemsGetter = p_Getter;
    }

    public override void Init()
    {
        InitItemButtonList();     
        InitItemList();
    }

    public override void Disable()
    {
        itemButtonList.RemoveCancelAction(ItemButtonListCancelAction);
        itemButtonList.RemoveKeyArrowAction(ShowDescription);
        itemButtonList.RemoveKeyArrowAction(itemButtonListScrolling.CheckScrolling);
        itemButtonList.Clear();
    }

    public override void InitItemList()
    {
        itemButtonList.Clear();
        ClearDescription();
        if (inventoryItemsGetter != null)
        {
            Dictionary<string, InventoryItemData> l_InventoryItems = inventoryItemsGetter.GetInventoryItems();
            foreach (var lKey in l_InventoryItems.Keys)
            {
                AddItem(l_InventoryItems[lKey]);
            }
        }
        base.InitItemList();
    }

    public override void AddItem(InventoryItemData p_InventoryItemData)
    {
        InventoryItemButton l_Button = UnityEngine.Object.Instantiate(InventoryItemButton.prefab);
        l_Button.title = LocalizationDataBase.GetInstance().GetText("Item:" + p_InventoryItemData.id);
        l_Button.itemId = p_InventoryItemData.id;
        l_Button.itemCount = p_InventoryItemData.count;
        l_Button.AddAction(SelectItem);
        l_Button.AddItemListRefreshAction(RefreshItemButtonList);

        itemButtonList.AddButton(l_Button);
    }

    private void RefreshItemButtonList()
    {
        if (itemButtonList != null)
        {
            InventoryItemButton l_ItemButton = (InventoryItemButton)itemButtonList.currentButton;
            if (PlayerInventory.GetInstance().GetItemCount(l_ItemButton.itemId) <= 0)
                itemButtonList.RemoveButton(l_ItemButton);
            if (itemButtonList.count > 0)
            {
                itemButtonList.isActive = true;
                ShowDescription();
            }
            else
            {
                Disable();
                ItemButtonListCancelAction();
            }
        }
    }

    public override void ItemButtonListCancelAction()
    {
        itemButtonList.isActive = false;
        parent.tabButtonList.isActive = true;
        ClearDescription();
    }

    public override bool Confirm()
    {
        if (itemButtonList != null && itemButtonList.count > 0)
        {
            itemButtonList.isActive = true;
            ShowDescription();
            return true;
        }
        return false;
    }

    public override void SelectItem()
    {
        InventoryItemButton l_ItemButton = (InventoryItemButton)itemButtonList.currentButton;
        l_ItemButton.CreateItemActionPanel();
    }

    public override void UpdateKey()
    {
        itemButtonList.UpdateKey();
    }

    public override bool isNull()
    {
        return false;
    }
}
