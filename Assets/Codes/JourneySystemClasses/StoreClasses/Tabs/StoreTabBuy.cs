
using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreTabBuy : StoreTab
{
    public StoreTabBuy(StorePanel p_Parent, IItemsGetter p_Getter)
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
            Dictionary<string, StoreItemData> l_StoreItems = storeItemsGetter.GetStoreItems();
            foreach (var lKey in l_StoreItems.Keys)
            {
                AddItem(l_StoreItems[lKey]);
            }
        }
    }

    public void AddItem(StoreItemData p_StoreItemData)
    {
        StoreBuyButton l_Button = (StoreBuyButton)Instantiate(StoreBuyButton.prefab);
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
        StoreBuyButton l_StoreItemButton = (StoreBuyButton)itemsButtonList.currentButton;
        l_StoreItemButton.Activate(true);
        l_StoreItemButton.AddCancelAction(DeselectItem);
        l_StoreItemButton.AddAction(Buy);
        itemsButtonList.isActive = false;

        parent.currentSelectedItem = l_StoreItemButton;
    }

    public override void DeselectItem()
    {
        StoreBuyButton l_StoreItemButton = (StoreBuyButton)itemsButtonList.currentButton;
        l_StoreItemButton.Activate(false);
        l_StoreItemButton.RemoveCancelAction(DeselectItem);
        l_StoreItemButton.RemoveAction(Buy);

        parent.currentSelectedItem = null;
        itemsButtonList.isActive = true;
    }

    private void Buy()
    {
        StoreBuyButton l_StoreItemButton = (StoreBuyButton)itemsButtonList.currentButton;
        int l_CountToBuy = l_StoreItemButton.countToAction;
        string l_ItemId = l_StoreItemButton.itemId;
        int l_ItemCost = l_StoreItemButton.itemCost;

        if (l_ItemCost * l_CountToBuy <= parent.playerCoins)
        {
            parent.playerCoins -= l_ItemCost * l_CountToBuy;
            PlayerInventory.GetInstance().AddItem(l_ItemId, l_CountToBuy);
            ShowItemDescription();
        }
    }

    public override void ShowItemDescription()
    {
        if (itemsButtonList != null && itemsButtonList.count > 0)
        {
            StoreBuyButton m_StoreItemButton = (StoreBuyButton)itemsButtonList.currentButton;
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
