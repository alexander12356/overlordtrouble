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
    }

    private void InitTabs()
    {
        //m_TabButtonsList.AddKeyArrowAction(ShowTab);
        //m_TabButtonsList[0].AddAction(ConfirmTab);
        //m_TabButtonsList[0].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Wear");
        //m_TabButtonsList[1].AddAction(ConfirmTab);
        //m_TabButtonsList[1].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Items");
        //m_TabButtonsList[2].AddAction(CloseInventory);
        //m_TabButtonsList[2].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Back");

        //m_CurrOpenedTab = m_InventoryTabs[0];
        m_TabButtonsList.isActive = true;
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
        m_DescriptionText.text = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Description");
        ShowItemDescription();
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
