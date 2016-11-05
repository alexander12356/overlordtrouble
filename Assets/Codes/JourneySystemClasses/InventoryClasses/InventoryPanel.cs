using System;
using System.Collections.Generic;
using UnityEngine;


public class InventoryPanel : Panel
{
    private static InventoryPanel m_Prefab;
    private InventoryTab m_CurrOpenedTab = null;
    [SerializeField]
    private ButtonList m_TabButtonsList = null;
    [SerializeField]
    private List<InventoryTab> m_InventoryTabs = null;

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

    public override void Awake()
    {
        base.Awake();

        InitTabs();
        m_TabButtonsList.isActive = true;
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_TabButtonsList.UpdateKey();
        m_CurrOpenedTab.UpdateKey();
    }

    private void InitTabs()
    {
        m_TabButtonsList.AddKeyArrowAction(ShowTab);
        m_TabButtonsList[0].AddAction(ConfirmTab);
        m_TabButtonsList[0].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Wear");
        m_TabButtonsList[1].AddAction(ConfirmTab);
        m_TabButtonsList[1].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Items");
        m_TabButtonsList[2].AddAction(CloseInventory);
        m_TabButtonsList[2].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Back");

        m_CurrOpenedTab = m_InventoryTabs[0];
    }

    private void ConfirmTab()
    {
        m_CurrOpenedTab.Confrim();
        m_TabButtonsList.isActive = false;
        m_TabButtonsList.currentButton.selected = true;
    }

    private void ShowTab()
    {
        if (m_TabButtonsList.currentButtonId < m_InventoryTabs.Count)
        {
            m_CurrOpenedTab.gameObject.SetActive(false);
            m_InventoryTabs[m_TabButtonsList.currentButtonId].gameObject.SetActive(true);
            m_CurrOpenedTab = m_InventoryTabs[m_TabButtonsList.currentButtonId];
        }
    }

    private void CloseInventory()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Player);
        PlayerInventory.GetInstance().SaveAll();
        Close();
    }
}
