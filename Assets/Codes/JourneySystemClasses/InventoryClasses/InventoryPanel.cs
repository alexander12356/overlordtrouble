using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : Panel
{
    #region Variables

    private static InventoryPanel m_Prefab;
    [SerializeField]
    private ButtonList m_TabButtonsList = null;
    [SerializeField]
    private Text m_PlayerCoinsText = null;

    private ButtonList m_ItemsButtonsList = null;
    private Text m_DescriptionText = null;

    #endregion

    #region Properties
    public static InventoryPanel prefab
    {
        get
        {
            if(m_Prefab == null)
            {
                m_Prefab = Resources.Load<InventoryPanel>("Prefabs/Panels/InventoryPanel");
            }
            return m_Prefab;
        }
    }

    public ButtonList tabButtonList
    {
        get
        {
            return m_TabButtonsList;
        }
    }

    public ButtonList itemsButtonList
    {
        get
        {
            if (m_ItemsButtonsList == null)
            {
                m_ItemsButtonsList = transform.FindChild("ItemList").GetComponentInChildren<ButtonList>();
            }
            return m_ItemsButtonsList;
        }
    }

    public int playerCoins
    {
        get
        {
            return PlayerInventory.GetInstance().coins;
        }
        set
        {
            PlayerInventory.GetInstance().coins = value;
            m_PlayerCoinsText.text = PlayerInventory.GetInstance().coins + " " + LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Monet");
        }
    }

    #endregion

    #region Methods

    public override void Awake()
    {
        base.Awake();

        InitTabs();
        InitItemList();
        InitDescriptionText();
        InitPlayerCoinsText();
        InitTabView();
    }

    private void InitTabs()
    {
        m_TabButtonsList.AddKeyArrowAction(InitTabView);
        m_TabButtonsList[0].AddAction(SelectEquipmentTab);
        m_TabButtonsList[1].AddAction(SelectItemList);
        m_TabButtonsList[2].AddAction(SelectItemList);
        m_TabButtonsList[3].AddAction(SelectItemList);
        m_TabButtonsList[4].AddAction(SelectItemList);
        m_TabButtonsList[5].AddAction(SelectItemList);
        m_TabButtonsList[6].AddAction(SelectItemList);
        m_TabButtonsList[7].AddAction(CloseInventory);

        InventoryTabButton l_TabButton;

        l_TabButton = m_TabButtonsList[1] as InventoryTabButton;
        l_TabButton.inventoryTab = new AllTab();

        l_TabButton = m_TabButtonsList[2] as InventoryTabButton;
        l_TabButton.inventoryTab = new WepsTab();

        l_TabButton = m_TabButtonsList[3] as InventoryTabButton;
        l_TabButton.inventoryTab = new BlingTab();

        l_TabButton = m_TabButtonsList[4] as InventoryTabButton;
        l_TabButton.inventoryTab = new SingleUseTab();

        l_TabButton = m_TabButtonsList[5] as InventoryTabButton;
        l_TabButton.inventoryTab = new MultiUseTab();

        l_TabButton = m_TabButtonsList[6] as InventoryTabButton;
        l_TabButton.inventoryTab = new KeyItemTab();

        m_TabButtonsList.SelectMoveDown();
        m_TabButtonsList.isActive = true;
    }

    private void SelectItemList()
    {
        if (m_ItemsButtonsList != null && m_ItemsButtonsList.count > 0)
        {
            m_TabButtonsList.isActive = false;
            m_TabButtonsList.currentButton.selected = true;
            m_ItemsButtonsList.isActive = true;
            ShowItemDescription();
        }
    }

    private void SelectEquipmentTab()
    {
        throw new NotImplementedException();
    }

    public void InitTabView()
    {
        InventoryTabButton l_TabButton = m_TabButtonsList[m_TabButtonsList.currentButtonId] as InventoryTabButton;
        itemsButtonList.Clear();
        ClearItemDescription();
        if (l_TabButton.inventoryTab != null)
        {
            Dictionary<string, InventoryItemData> l_InventoryItems = l_TabButton.inventoryTab.GetItems();
            foreach (var lKey in l_InventoryItems.Keys)
            {
                AddItem(l_InventoryItems[lKey]);
            }
        }
    }

    private void AddItem(InventoryItemData p_InventoryItemData)
    {
        InventoryItemButton l_Button = Instantiate(InventoryItemButton.prefab);
        l_Button.title = LocalizationDataBase.GetInstance().GetText("Item:" + p_InventoryItemData.id);
        l_Button.itemId = p_InventoryItemData.id;
        l_Button.itemCount = p_InventoryItemData.count;
        l_Button.AddAction(SelectItem);

        itemsButtonList.AddButton(l_Button);
    }

    private void SelectItem()
    {
        InventoryItemButton l_ItemButton = (InventoryItemButton)m_ItemsButtonsList.currentButton;
        l_ItemButton.CreateItemActionPanel();
    }

    private void InitItemList()
    {
        m_ItemsButtonsList = itemsButtonList;
        m_ItemsButtonsList.AddCancelAction(ItemListCancelAction);
        m_ItemsButtonsList.AddKeyArrowAction(ShowItemDescription);
        m_ItemsButtonsList.isActive = false;
    }

    private void InitDescriptionText()
    {
        m_DescriptionText = transform.FindChild("Description").GetComponent<Text>();
    }

    private void InitPlayerCoinsText()
    {
        playerCoins = PlayerInventory.GetInstance().coins;
    }

    private void ShowItemDescription()
    {
        if (m_ItemsButtonsList != null && m_ItemsButtonsList.count > 0)
        {
            InventoryItemButton l_InventoryItemButton = (InventoryItemButton)m_ItemsButtonsList.currentButton;
            string l_DescriptionText = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Description");
            m_DescriptionText.text = l_DescriptionText + l_InventoryItemButton.title;
        }
    }

    private void ClearItemDescription()
    {
        m_DescriptionText.text = String.Empty;
    }

    private void ItemListCancelAction()
    {
        tabButtonList.isActive = true;
        itemsButtonList.isActive = false;
    }

    //private void ConfirmTab()
    //{
    //    m_CurrOpenedTab.Confrim();
    //    m_TabButtonsList.isActive = false;
    //    m_TabButtonsList.currentButton.selected = true;
    //}

    //private void ShowTab()
    //{
    //    if (m_TabButtonsList.currentButtonId < m_InventoryTabs.Count)
    //    {
    //        m_CurrOpenedTab.gameObject.SetActive(false);
    //        m_InventoryTabs[m_TabButtonsList.currentButtonId].gameObject.SetActive(true);
    //        m_CurrOpenedTab = m_InventoryTabs[m_TabButtonsList.currentButtonId];
    //    }
    //}

    private void CloseInventory()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Player);
        PlayerInventory.GetInstance().SaveAll();
        Close();
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_TabButtonsList.UpdateKey();
        m_ItemsButtonsList.UpdateKey();
    }

    #endregion
}
