using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEquipmentView : InventoryView
{
    #region Variables

    private ButtonList m_SlotButtonList;
    private ButtonList m_ItemButtonList;
    private ButtonList m_GroupButtonList;
    private Text m_DescriptionText;
    private GameObject m_PlayerStat;
    private Text m_HealthStatText;
    private Text m_SpecialPointStatText;
    private Text m_AttackStatText;
    private Text m_DefenseStatText;
    private Text m_SpeedStatText;
    private int m_EmptySlotsCount = 4;

    // test data
    List<TestGroupMemberData> m_TestGroupMemberDataList = null;

    #endregion

    #region Properties

    public ButtonList slotButtonList
    {
        get
        {
            if (m_SlotButtonList == null)
            {
                m_SlotButtonList = parent.transform.FindChild("SlotList").GetComponentInChildren<ButtonList>();
            }
            return m_SlotButtonList;
        }
    }

    public ButtonList itemButtonList
    {
        get
        {
            if (m_ItemButtonList == null)
            {
                m_ItemButtonList = parent.transform.FindChild("ItemList").GetComponentInChildren<ButtonList>();
            }
            return m_ItemButtonList;
        }
    }

    public ButtonList groupButtonList
    {
        get
        {
            if(m_GroupButtonList == null)
            {
                m_GroupButtonList = parent.transform.FindChild("GroupList").GetComponent<ButtonList>();
            }
            return m_GroupButtonList;
        }
    }

    private Text descriptionText
    {
        get
        {
            if (m_DescriptionText == null)
            {
                m_DescriptionText = parent.transform.FindChild("Description").GetComponent<Text>();
            }
            return m_DescriptionText;
        }
    }

    private GameObject playerStat
    {
        get
        {
            if(m_PlayerStat == null)
            {
                m_PlayerStat = parent.transform.FindChild("PlayerStat").gameObject;
                m_HealthStatText = m_PlayerStat.transform.FindChild("HealthStat").GetComponent<Text>();
                m_SpecialPointStatText = m_PlayerStat.transform.FindChild("SpecialPointsStat").GetComponent<Text>();
                m_AttackStatText = m_PlayerStat.transform.FindChild("AttackStat").GetComponent<Text>();
                m_DefenseStatText = m_PlayerStat.transform.FindChild("DefenseStat").GetComponent<Text>();
                m_SpeedStatText = m_PlayerStat.transform.FindChild("SpeedStat").GetComponent<Text>();
            }
            return m_PlayerStat;
        }
    }

    #endregion

    #region Methods

    public InventoryEquipmentView(InventoryPanel p_Parent)
    {
        parent = p_Parent;
        InitGroupData();
        InitGroupButtonList();
        InitEmptySlots();
    }

    public override void Init()
    {
        InitSlotButtonList();
        InitItemButtonList();
        m_DescriptionText = descriptionText;
    }

    private void InitGroupData()
    {
        m_TestGroupMemberDataList = new List<TestGroupMemberData>();
        TestGroupMemberData l_GroupMemberData = new TestGroupMemberData(10.0f, 10.0f, 5.0f, 20.0f, 5.0f, 5.0f, 1.0f);
        m_TestGroupMemberDataList.Add(l_GroupMemberData);

        l_GroupMemberData = new TestGroupMemberData(85.0f, 100.0f, 77.0f, 100.0f, 23.0f, 17.0f, 3.3f);
        m_TestGroupMemberDataList.Add(l_GroupMemberData);
    }

    private void InitGroupButtonList()
    {
        m_GroupButtonList = groupButtonList;
        m_GroupButtonList.AddCancelAction(CancelAction);
        m_GroupButtonList.AddKeyArrowAction(ShowGroupMemberInfo);
        m_GroupButtonList.isActive = false;

        foreach(var groupMemberData in m_TestGroupMemberDataList)
        {
            AddGroupMember(groupMemberData);
        }
    }

    private void AddGroupMember(TestGroupMemberData groupMemberData)
    {
        InventoryGroupMemberButton l_Button = UnityEngine.Object.Instantiate(InventoryGroupMemberButton.prefab);
        l_Button.testMemberData = groupMemberData;
        l_Button.AddAction(SelectSlotList);
        m_GroupButtonList.AddButton(l_Button);
    }

    private void SelectSlotList()
    {
        m_GroupButtonList.isActive = false;
        m_SlotButtonList.isActive = true;
    }

    private void InitItemButtonList()
    {
        m_ItemButtonList = itemButtonList;
        m_ItemButtonList.AddCancelAction(DeselectItemList);
        m_ItemButtonList.AddKeyArrowAction(ShowDescription);
        m_ItemButtonList.isActive = false;
    }

    private void InitSlotButtonList()
    {
        m_SlotButtonList = slotButtonList;
        m_SlotButtonList.AddCancelAction(DeselectSlotList);
        m_SlotButtonList.AddKeyArrowAction(InitItemsList);
        m_SlotButtonList.isActive = false;
    }

    private void DeselectSlotList()
    {
        m_GroupButtonList.isActive = true;
        m_SlotButtonList.isActive = false;
    }

    public override void Disable()
    {
        m_SlotButtonList.RemoveCancelAction(DeselectSlotList);
        m_SlotButtonList.RemoveKeyArrowAction(InitItemsList);
        m_ItemButtonList.RemoveCancelAction(DeselectItemList);
        m_ItemButtonList.RemoveKeyArrowAction(ShowDescription);
    }

    private void ShowGroupMemberInfo()
    {
        InitSlots(PlayerInventory.GetInstance().GetInventorySlotData());
        InventoryGroupMemberButton l_GroupMemberButton = (InventoryGroupMemberButton) m_GroupButtonList[m_GroupButtonList.currentButtonId];
        playerStat.gameObject.SetActive(true);
        m_HealthStatText.text = "HP : " + l_GroupMemberButton.testMemberData.m_Health;
        m_SpecialPointStatText.text = "SP : " + l_GroupMemberButton.testMemberData.m_SpecialPoints;
        m_AttackStatText.text = "Atk : " + l_GroupMemberButton.testMemberData.m_AttackStat;
        m_DefenseStatText.text = "Def : " + l_GroupMemberButton.testMemberData.m_DefenseStat;
        m_SpeedStatText.text = "Speed : " + l_GroupMemberButton.testMemberData.m_SpeedStat; 
    }

    private void ClearGroupMemberInfo()
    {
        playerStat.gameObject.SetActive(false);
    }

    // TODO : Сделать нормальное описание предметов
    public override void ShowDescription()
    {
        if (m_ItemButtonList != null && m_ItemButtonList.count > 0)
        {
            InventoryEquipmentButton lEqButton = (InventoryEquipmentButton)m_ItemButtonList.currentButton;
            int lCountInInventory = PlayerInventory.GetInstance().GetItemCount(lEqButton.itemId);
            string lInInventoryText = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:InInventory");
            m_DescriptionText.text = lEqButton.title + "_Description" + lInInventoryText + lCountInInventory;
        }
    }

    public override void ClearDescription()
    {
        m_DescriptionText.text = "";
    }

    /// <summary>
    /// Создание пустого списка слотов
    /// </summary>
    private void InitEmptySlots()
    {
        slotButtonList.Clear();
        for (int i = 0; i < m_EmptySlotsCount; i++)
        {
            InventorySlotButton l_Button = UnityEngine.Object.Instantiate(InventorySlotButton.prefab);
            l_Button.title = "-------";
            slotButtonList.AddButton(l_Button);
        }
        slotButtonList.isActive = false;
    }

    private void InitSlots(Dictionary<string, InventorySlotData> p_SlotData)
    {
        slotButtonList.Clear();
        foreach (var lKey in p_SlotData.Keys)
        {
            AddSlot(p_SlotData[lKey]);
        }
    }

    public void AddSlot(InventorySlotData pSlotData)
    {
        InventorySlotButton l_Button = UnityEngine.Object.Instantiate(InventorySlotButton.prefab);

        if (PlayerInventory.GetInstance().GetItemCount(pSlotData.itemId) == 0)
            pSlotData.itemId = string.Empty;

        l_Button.slotData = pSlotData;
        l_Button.AddAction(SelectItemList);
        l_Button.title = pSlotData.slotType.GetTitle(slotButtonList.count);

        slotButtonList.AddButton(l_Button);
    }

    private void InitItemsList()
    {
        itemButtonList.Clear();
        InventorySlotButton l_SlotButton = (InventorySlotButton)m_SlotButtonList.currentButton;
        Dictionary<string, InventoryItemData> l_InventoryItems = new Dictionary<string, InventoryItemData>();
        l_InventoryItems = l_SlotButton.slotType.GetInventoryItemData();
        foreach (var lKey in l_InventoryItems.Keys)
        {
            if (PlayerInventory.GetInstance().ItemAlreadyUsed(l_SlotButton.slotId, l_InventoryItems[lKey].id))
                continue;
            AddItem(l_InventoryItems[lKey]);
        }
    }

    public override void AddItem(InventoryItemData pInventoryItemData)
    {
        InventoryEquipmentButton l_Button = UnityEngine.Object.Instantiate(InventoryEquipmentButton.prefab);
        l_Button.title = LocalizationDataBase.GetInstance().GetText("Item:" + pInventoryItemData.id);
        l_Button.itemId = pInventoryItemData.id;
        l_Button.AddAction(SelectItem);

        if (PlayerInventory.GetInstance().SlotsContainItem(l_Button.itemId))
            l_Button.equipped = true;

        itemButtonList.AddButton(l_Button);
    }

    private void SelectItemList()
    {
        itemButtonList.isActive = true;
        slotButtonList.isActive = false;
        slotButtonList.currentButton.selected = true;
        ShowDescription();
    }

    private void DeselectItemList()
    {
        itemButtonList.isActive = false;
        slotButtonList.isActive = true;
        m_DescriptionText.text = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Description");
    }

    public override void CancelAction()
    {
        m_GroupButtonList.isActive = false;
        parent.tabButtonList.isActive = true;
        InitEmptySlots();
        ClearGroupMemberInfo();
    }

    public override bool Confrim()
    {
        m_GroupButtonList.isActive = true;
        ShowGroupMemberInfo();
        return true;
    }

    public override void SelectItem()
    {
        // TODO : Экипировать игрока выбранным предметом
        InventoryEquipmentButton l_EqButton = (InventoryEquipmentButton)m_ItemButtonList.currentButton;
        InventorySlotButton l_SlotButton = (InventorySlotButton)m_SlotButtonList.currentButton;

        if (l_SlotButton.IsFull)
        {
            if (l_EqButton.equipped)
            {
                l_SlotButton.DeselectItem();
                l_EqButton.equipped = false;
                PlayerInventory.GetInstance().UpdateSlotData(l_SlotButton.slotId, l_SlotButton.slotData);
            }
            else
            {
                ClearEquipped();
                l_SlotButton.SelectItem(l_EqButton.itemId);
                l_EqButton.equipped = true;
                PlayerInventory.GetInstance().UpdateSlotData(l_SlotButton.slotId, l_SlotButton.slotData);
            }
        }
        else
        {
            l_EqButton.equipped = true;
            l_SlotButton.SelectItem(l_EqButton.itemId);
            PlayerInventory.GetInstance().UpdateSlotData(l_SlotButton.slotId, l_SlotButton.slotData);
        }
    }

    private void ClearEquipped()
    {
        for (int i = 0; i < m_ItemButtonList.count; i++)
        {
            (m_ItemButtonList[i] as InventoryEquipmentButton).equipped = false;
        }
    }

    public override void UpdateKey()
    {
        slotButtonList.UpdateKey();
        itemButtonList.UpdateKey();
        groupButtonList.UpdateKey();
    }

    #endregion
}
