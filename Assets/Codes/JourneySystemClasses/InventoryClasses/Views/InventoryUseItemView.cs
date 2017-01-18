using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUseItemView : InventoryView
{
    PanelActionHandler m_UseAction;
    PanelActionHandler m_CancelAction;
    private ButtonList m_SlotButtonList;
    private ButtonList m_ItemButtonList;
    private ButtonList m_GroupButtonList;
    private GameObject m_PlayerStat;
    private Text m_HealthStatText;
    private Text m_SpecialPointStatText;
    private Text m_AttackStatText;
    private Text m_DefenseStatText;
    private Text m_SpeedStatText;
    private int m_EmptySlotsCount = 4;
    // test data
    List<TestGroupMemberData> m_TestGroupMemberDataList = null;

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
            if (m_GroupButtonList == null)
            {
                m_GroupButtonList = parent.transform.FindChild("GroupList").GetComponent<ButtonList>();
            }
            return m_GroupButtonList;
        }
    }

    private GameObject playerStat
    {
        get
        {
            if (m_PlayerStat == null)
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

    public InventoryUseItemView(InventoryPanel p_Parent)
    {
        parent = p_Parent;
    }

    public override void Init()
    {
        InitGroupData();
        InitGroupButtonList();
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
        groupButtonList.Clear();
        groupButtonList.ClearEvents();
        groupButtonList.AddCancelAction(CancelAction);
        groupButtonList.AddKeyArrowAction(ShowGroupMemberInfo);
        groupButtonList.isActive = false;

        foreach (var groupMemberData in m_TestGroupMemberDataList)
        {
            AddGroupMember(groupMemberData);
        }
    }

    private void ShowGroupMemberInfo()
    {
        InitSlots(PlayerInventory.GetInstance().GetInventorySlotData());
        InventoryGroupMemberButton l_GroupMemberButton = (InventoryGroupMemberButton)groupButtonList[groupButtonList.currentButtonId];
        playerStat.gameObject.SetActive(true);
        m_HealthStatText.text = LocalizationDataBase.GetInstance().GetText("Stat:HealthPoints") + " : " + l_GroupMemberButton.testMemberData.m_Health;
        m_SpecialPointStatText.text = LocalizationDataBase.GetInstance().GetText("Stat:MonstylePoints") + " : " + l_GroupMemberButton.testMemberData.m_SpecialPoints;
        m_AttackStatText.text = LocalizationDataBase.GetInstance().GetText("Stat:Attack") + " : " + l_GroupMemberButton.testMemberData.m_AttackStat;
        m_DefenseStatText.text = LocalizationDataBase.GetInstance().GetText("Stat:Defense") + " : " + l_GroupMemberButton.testMemberData.m_DefenseStat;
        m_SpeedStatText.text = LocalizationDataBase.GetInstance().GetText("Stat:Speed") + " : " + l_GroupMemberButton.testMemberData.m_SpeedStat;
    }

    private void InitSlots(Dictionary<string, InventorySlotData> p_SlotData)
    {
        slotButtonList.Clear();
        foreach (var lKey in p_SlotData.Keys)
        {
            AddSlot(p_SlotData[lKey]);
        }
    }

    private void AddSlot(InventorySlotData pSlotData)
    {
        InventorySlotButton l_Button = UnityEngine.Object.Instantiate(InventorySlotButton.prefab);

        if (PlayerInventory.GetInstance().GetItemCount(pSlotData.itemId) == 0)
            pSlotData.itemId = string.Empty;

        l_Button.slotData = pSlotData;
        l_Button.defaultTitle = pSlotData.slotType.GetTitle(slotButtonList.count);

        slotButtonList.AddButton(l_Button);
    }

    private void AddGroupMember(TestGroupMemberData groupMemberData)
    {
        InventoryGroupMemberButton l_Button = UnityEngine.Object.Instantiate(InventoryGroupMemberButton.prefab);
        l_Button.testMemberData = groupMemberData;
        l_Button.AddAction(UseForThisMember);
        groupButtonList.AddButton(l_Button);
    }

    private void UseForThisMember()
    {
        if(m_UseAction != null)
        {
            m_UseAction();
        }
        CancelAction();
    }

    public override void CancelAction()
    {
        groupButtonList.isActive = false;
        itemButtonList.isActive = true;
        InitEmptySlots();
        ClearGroupMemberInfo();
        if (m_CancelAction != null)
        {
            m_CancelAction();
        }
    }

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

    private void ClearGroupMemberInfo()
    {
        playerStat.gameObject.SetActive(false);
    }

    public override void UpdateKey()
    {
        groupButtonList.UpdateKey();
    }

    public void AddUseAction(PanelActionHandler p_Action)
    {
        m_UseAction += p_Action;
    }

    public void RemoveUseAction(PanelActionHandler p_Action)
    {
        m_UseAction -= p_Action;
    }

    public void AddCancelAction(PanelActionHandler p_Action)
    {
        m_CancelAction += p_Action;
    }

    public void RemoveCancelAction(PanelActionHandler p_Action)
    {
        m_CancelAction -= p_Action;
    }

    public override bool Confrim()
    {
        groupButtonList.isActive = true;
        ShowGroupMemberInfo();
        return true;
    }
}
