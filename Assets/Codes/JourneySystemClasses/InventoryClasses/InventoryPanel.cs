using System;
using System.Collections.Generic;
using UnityEngine;


public class InventoryPanel : Panel
{
    private static InventoryPanel m_Prefab;
    private InventoryTab mCurrOpenedTab = null;
    [SerializeField]
    private ButtonList mTabButtonsList = null;
    [SerializeField]
    private List<InventoryTab> mInventoryTabs = null;

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
            return mTabButtonsList;
        }
    }

    public override void Awake()
    {
        base.Awake();

        InitTabs();
        InitSlots();

        mTabButtonsList.isActive = true;
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        mTabButtonsList.UpdateKey();
        mCurrOpenedTab.UpdateKey();
    }

    private void InitTabs()
    {
        mTabButtonsList.AddKeyArrowAction(ShowTab);
        mTabButtonsList[0].AddAction(ConfirmTab);
        mTabButtonsList[0].title = "Надеть"; // TODO: Add localization
        mTabButtonsList[1].AddAction(ConfirmTab);
        mTabButtonsList[1].title = "Предметы";
        mTabButtonsList[2].AddAction(CloseInventory);
        mTabButtonsList[2].title = "Назад";

        mCurrOpenedTab = mInventoryTabs[0];
    }

    private void InitSlots()
    {
        InventoryEquipmentTab lEquipmentTab = (InventoryEquipmentTab)mInventoryTabs[0];
        Dictionary<string, InventorySlotData> lSlotData = PlayerInventory.GetInstance().GetInventorySlotData();
        foreach (var lKey in lSlotData.Keys)
        {
            lEquipmentTab.AddSlot(lSlotData[lKey]);
        }
    }

    private void ConfirmTab()
    {
        mCurrOpenedTab.Confrim();
        mTabButtonsList.isActive = false;
        mTabButtonsList.currentButton.selected = true;
    }

    private void ShowTab()
    {
        if (mTabButtonsList.currentButtonId < mInventoryTabs.Count)
        {
            mCurrOpenedTab.gameObject.SetActive(false);
            mInventoryTabs[mTabButtonsList.currentButtonId].gameObject.SetActive(true);
            mCurrOpenedTab = mInventoryTabs[mTabButtonsList.currentButtonId];
        }
    }

    private void CloseInventory()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Player);
        PlayerInventory.GetInstance().Save();
        Close();
    }
}
