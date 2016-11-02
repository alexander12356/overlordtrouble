﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum eItemTab
{
    TAB_ALL,
    TAB_EQUIPMENT,
    TAB_SINGLE,
    TAB_MULTIPLY,
    TAB_KEY
}

public class InventoryItemsTab : InventoryTab
{
    #region Variables
    private ButtonList m_ItemsButtonsList;
    private InventoryPanel m_InventoryPanel;
    private Text m_DescriptionText;
    private InventoryItemButton m_CurrentSelectedItem = null;

    [SerializeField]
    private ButtonList m_TabButtonsList = null;
    #endregion

    #region Interface
    public ButtonList itemsButtonList
    {
        get
        {
            if(m_ItemsButtonsList == null)
            {
                m_ItemsButtonsList = transform.FindChild("ItemList").GetComponentInChildren<ButtonList>();
            }
            return m_ItemsButtonsList;
        }
    }

    public ButtonList tabButtonList
    {
        get
        {
            return m_TabButtonsList;
        }
    }

    public InventoryItemButton currentSelectedItem
    {
        get
        {
            return m_CurrentSelectedItem;
        }
        set
        {
            m_CurrentSelectedItem = value;
        }
    }
    #endregion

    public void Awake()
    {
        InitTabs();

        m_ItemsButtonsList = itemsButtonList;
        m_ItemsButtonsList.AddCancelAction(ItemListCancelAction);
        m_ItemsButtonsList.AddKeyArrowAction(ShowItemDescription);
        m_ItemsButtonsList.isActive = false;

        m_DescriptionText = transform.FindChild("Description").GetComponent<Text>();
        m_DescriptionText.text = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Description"); // TODO: Добавить текст
        m_InventoryPanel = GetComponentInParent<InventoryPanel>();

        InitItemList();
        ShowItemDescription();
    }

    private void InitTabs()
    {
        m_TabButtonsList.AddKeyArrowAction(InitItemList);
        m_TabButtonsList[0].AddAction(SelectItemList);
        m_TabButtonsList[1].AddAction(SelectItemList);
        m_TabButtonsList[2].AddAction(SelectItemList);
        m_TabButtonsList[3].AddAction(SelectItemList);
        m_TabButtonsList[4].AddAction(SelectItemList);
        m_TabButtonsList[5].AddAction(CancelAction);
        m_TabButtonsList.isActive = false;
    }

    private void InitItemList()
    {
        itemsButtonList.Clear();
        Dictionary<string, InventoryItemData> l_InventoryItems = new Dictionary<string, InventoryItemData>();
        switch((eItemTab)m_TabButtonsList.currentButtonId)
        {
            case eItemTab.TAB_ALL:
                l_InventoryItems = PlayerInventory.GetInstance().GetInventoryItems();
                break;
            case eItemTab.TAB_EQUIPMENT:
                l_InventoryItems = PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Equipment).ToDictionary(obj => obj.Key, obj => obj.Value);
                break;
            case eItemTab.TAB_SINGLE:
                l_InventoryItems = PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.SingleUse).ToDictionary(obj => obj.Key, obj => obj.Value);
                break;
            case eItemTab.TAB_MULTIPLY:
                l_InventoryItems = PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.MultipleUse).ToDictionary(obj => obj.Key, obj => obj.Value);
                break;
            case eItemTab.TAB_KEY:
                l_InventoryItems = PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
                break;
        }
        foreach(var lKey in l_InventoryItems.Keys)
        {
            AddItem(l_InventoryItems[lKey]);
        }
    }

    private void AddItem(InventoryItemData p_InventoryItemData)
    {
        InventoryItemButton l_Button = Instantiate(InventoryItemButton.prefab);
        l_Button.title = LocalizationDataBase.GetInstance().GetText("Item:" + p_InventoryItemData.id);
        l_Button.itemId = p_InventoryItemData.id;
        l_Button.itemCount = p_InventoryItemData.count;
        l_Button.AddAction(SelectItem);
        l_Button.InitActionButtonList();

        itemsButtonList.AddButton(l_Button);
    }

    // TODO : Сделать нормальное описание
    private void ShowItemDescription()
    {
        if (m_ItemsButtonsList != null && m_ItemsButtonsList.count > 0)
        {
            InventoryItemButton l_InventoryItemButton = (InventoryItemButton)m_ItemsButtonsList.currentButton;
            string l_DescriptionText = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Description");
            m_DescriptionText.text = l_DescriptionText + l_InventoryItemButton.title;
        }
    }

    private void SelectItemList()
    {
        if (m_ItemsButtonsList != null && m_ItemsButtonsList.count > 0)
        {
            m_TabButtonsList.isActive = false;
            m_TabButtonsList.currentButton.selected = true;
            m_ItemsButtonsList.isActive = true;
        }
    }

    private void ItemListCancelAction()
    {
        m_TabButtonsList.isActive = true;
        m_ItemsButtonsList.isActive = false;
    }

    public override void CancelAction()
    {
        m_TabButtonsList.isActive = false;
        m_InventoryPanel.tabButtonList.isActive = true;
    }

    public override void Confrim()
    {
        m_TabButtonsList.isActive = true;
    }

    public override void SelectItem()
    {
        InventoryItemButton l_ItemButton = (InventoryItemButton)m_ItemsButtonsList.currentButton;
        l_ItemButton.Activate(true);
        l_ItemButton.AddCancelAction(DeselectItem);
        l_ItemButton.AddDestroyButtonAction(DestroyItemButton);
        currentSelectedItem = l_ItemButton;

        m_ItemsButtonsList.isActive = false;
    }

    public override void DeselectItem()
    {
        InventoryItemButton l_ItemButton = (InventoryItemButton)m_ItemsButtonsList.currentButton;
        l_ItemButton.Activate(false);
        l_ItemButton.RemoveCancelAction(DeselectItem);
        currentSelectedItem = null;

        m_ItemsButtonsList.isActive = true;
    }

    private void DestroyItemButton()
    {
        InventoryItemButton l_ItemButton = (InventoryItemButton)m_ItemsButtonsList.currentButton;
        l_ItemButton.RemoveCancelAction(DeselectItem);
        InitItemList();
    }

    public override void UpdateKey()
    {
        m_TabButtonsList.UpdateKey();
        m_ItemsButtonsList.UpdateKey();
        if(m_CurrentSelectedItem != null)
        {
            m_CurrentSelectedItem.UpdateKey();
        }
    }
}