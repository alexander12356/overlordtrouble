using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemsView : InventoryView
{
    #region Variables
    private IInventoryItemsGetter m_InventoryItemsGetter = null;
    #endregion

    #region Interface

    public IInventoryItemsGetter inventoryItemsGetter
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

    public InventoryItemsView(InventoryPanel p_Parent, IInventoryItemsGetter p_Getter)
    {
        parent = p_Parent;
        inventoryItemsGetter = p_Getter;
    }

    public override void Init()
    {
        InitItemButtonList();     
        InitItemList();
        ShowDescription();
    }

    public override void Disable()
    {
        itemButtonList.RemoveCancelAction(ItemButtonListCancelAction);
        itemButtonList.RemoveKeyArrowAction(ShowDescription);
        itemButtonList.Clear();
    }

    public override void InitItemList()
    {
        itemButtonList.Clear();
        ClearDescription();
        if (inventoryItemsGetter != null)
        {
            Dictionary<string, InventoryItemData> l_InventoryItems = inventoryItemsGetter.GetItems();
            foreach (var lKey in l_InventoryItems.Keys)
            {
                AddItem(l_InventoryItems[lKey]);
            }
        }
    }

    public override void AddItem(InventoryItemData p_InventoryItemData)
    {
        InventoryItemButton l_Button = UnityEngine.Object.Instantiate(InventoryItemButton.prefab);
        l_Button.title = LocalizationDataBase.GetInstance().GetText("Item:" + p_InventoryItemData.id);
        l_Button.itemId = p_InventoryItemData.id;
        l_Button.itemCount = p_InventoryItemData.count;
        l_Button.AddAction(SelectItem);
        l_Button.AddRemovingAction(ChangeItemButtonList);

        itemButtonList.AddButton(l_Button);
    }

    private void ChangeItemButtonList()
    {
        InventoryItemButton l_ItemButton = (InventoryItemButton) itemButtonList.currentButton;
        itemButtonList.RemoveButton(l_ItemButton);
        ShowDescription();
    }

    public override void ItemButtonListCancelAction()
    {
        itemButtonList.isActive = false;
        parent.tabButtonList.isActive = true;
    }

    public override bool Confrim()
    {
        if (itemButtonList != null && itemButtonList.count > 0)
        {
            itemButtonList.isActive = true;
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
