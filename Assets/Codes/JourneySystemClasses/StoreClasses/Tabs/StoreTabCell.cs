
using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreTabCell : StoreTab
{
    public StoreTabCell(StorePanel p_Parent, IItemsGetter p_Getter)
    {
        parent = p_Parent;
        storeItemsGetter = p_Getter;
    }

    public override void InitItemList()
    {
        itemsButtonList.Clear();
        ClearDescription();
        if (storeItemsGetter != null)
        {
            Dictionary<string, InventoryItemData> l_StoreItems = storeItemsGetter.GetInventoryItems();
            foreach (var lKey in l_StoreItems.Keys)
            {
                AddItem(l_StoreItems[lKey]);
            }
        }
    }

    public void AddItem(InventoryItemData p_StoreItemData)
    {
        StoreCellButton l_Button = (StoreCellButton)Instantiate(StoreCellButton.prefab);
        l_Button.title = LocalizationDataBase.GetInstance().GetText("Item:" + p_StoreItemData.id);
        l_Button.itemId = p_StoreItemData.id;
        l_Button.AddAction((PanelButtonActionHandler)this.SelectItem);

        itemsButtonList.AddButton(l_Button);

        Vector3 l_ButtonPosition = l_Button.transform.localPosition;
        l_ButtonPosition.x = 0.0f;
        l_Button.transform.localPosition = l_ButtonPosition;
    }

    public override void SelectItem()
    {
        StoreCellButton l_StoreItemButton = (StoreCellButton)itemsButtonList.currentButton;
        l_StoreItemButton.Activate(true);
        l_StoreItemButton.AddCancelAction(DeselectItem);
        l_StoreItemButton.AddAction(Cell);
        itemsButtonList.isActive = false;

        parent.currentSelectedItem = l_StoreItemButton;
    }

    public override void DeselectItem()
    {
        if (itemsButtonList != null && itemsButtonList.count > 0)
        {
            StoreCellButton l_StoreItemButton = (StoreCellButton)itemsButtonList.currentButton;
            l_StoreItemButton.Activate(false);
            l_StoreItemButton.RemoveCancelAction(DeselectItem);
            l_StoreItemButton.RemoveAction(Cell);

            parent.currentSelectedItem = null;
            itemsButtonList.isActive = true;
        }
        else
        {
            Disable();
            CancelAction();
        }
    }

    private void Cell()
    {
        StoreCellButton l_StoreItemButton = (StoreCellButton)itemsButtonList.currentButton;
        int l_CountToCell = l_StoreItemButton.countToAction;
        string l_ItemId = l_StoreItemButton.itemId;
        int l_ItemCost = l_StoreItemButton.itemCost;
        int l_ItemCount = PlayerInventory.GetInstance().GetItemCount(l_ItemId);

        parent.playerCoins += l_ItemCost * l_CountToCell;
        PlayerInventory.GetInstance().SetItemCount(l_ItemId, l_ItemCount - l_CountToCell);
        if ((l_ItemCount - l_CountToCell) <= 0)
            itemsButtonList.RemoveButton(l_StoreItemButton);
        ShowItemDescription();
    }

    public override void ShowItemDescription()
    {
        if (itemsButtonList != null && itemsButtonList.count > 0)
        {
            StoreCellButton m_StoreItemButton = (StoreCellButton)itemsButtonList.currentButton;
            int l_CountInInventory = PlayerInventory.GetInstance().GetItemCount(m_StoreItemButton.itemId);
            string l_DescriptionText = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Description");
            string l_InInventoryText = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:InInventory");
            descriptionText.text = l_DescriptionText + m_StoreItemButton.title + "_Description" + l_InInventoryText + l_CountInInventory;
        }
    }

    public override void Disable()
    {
        itemsButtonList.RemoveCancelAction(CancelAction);
        itemsButtonList.RemoveKeyArrowAction(ShowItemDescription);
        itemsButtonList.Clear();
    }
}
