using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InventoryView 
{
    #region Variables
    private bool m_Enabled;
    private ButtonList m_GroupButtonList = null;
    private ButtonList m_SlotButtonList = null;
    private ButtonList m_ItemButtonsList = null;
    private ButtonListScrolling m_ItemButtonListScrolling = null;
    private GameObject m_PlayerStat;
    private Text m_HealthStatText;
    private Text m_SpecialPointStatText;
    private Text m_AttackStatText;
    private Text m_DefenseStatText;
    private Text m_SpeedStatText;
    private static int m_EmptySlotsCount = 4;
    private Text m_DescriptionText;
    List<GroupMemberData> m_GroupMemberDataList = null;
    #endregion

    #region Properties
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
            if (m_ItemButtonsList == null)
            {
                m_ItemButtonsList = parent.transform.FindChild("ItemList").GetComponentInChildren<ButtonList>();
            }
            return m_ItemButtonsList;
        }
    }

    public ButtonListScrolling itemButtonListScrolling
    {
        get
        {
            if(m_ItemButtonListScrolling == null)
            {
                m_ItemButtonListScrolling = parent.transform.FindChild("ItemList").GetComponent<ButtonListScrolling>();
            }
            return m_ItemButtonListScrolling;
        }
    }

    public GameObject playerStat
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

    public int emptySlotsCount
    {
        get
        {
            return m_EmptySlotsCount;
        }
    }

    public Text descriptionText
    {
        get
        {
            if(m_DescriptionText == null)
            {
                m_DescriptionText = parent.transform.FindChild("Description").GetComponent<Text>();
            }
            return m_DescriptionText;
        }
    }

    public bool enabled
    {
        get
        {
            return m_Enabled;
        }
        set
        {
            m_Enabled = value;
        }
    }
    #endregion

    #region Methods
    public InventoryPanel parent { get; set; }
    public abstract void Init();
    public abstract bool Confirm();
    public virtual void Disable() { }
    public virtual void AddItem(InventoryItemData pInventoryItemData) { }
    public virtual void CancelAction() { }
    public virtual void SelectItem() { }
    public virtual void UpdateKey() { }
    public abstract bool isNull();

    public void InitGroupData()
    {
        m_GroupMemberDataList = new List<GroupMemberData>();
        float l_Health = PlayerData.GetInstance().GetStatValue("HealthPoints");
        float l_SpecialPoints = PlayerData.GetInstance().GetStatValue("MonstylePoints");
        float l_AttackStat = PlayerData.GetInstance().GetStatValue("Attack");
        float l_DefenseStat = PlayerData.GetInstance().GetStatValue("Defense");
        float l_SpeedStat = PlayerData.GetInstance().GetStatValue("Speed");
        GroupMemberData l_GroupMemberData = new GroupMemberData(l_Health, l_Health, l_SpecialPoints, l_SpecialPoints, l_AttackStat, l_DefenseStat, l_SpeedStat);
        l_GroupMemberData.avatatarSprite = PlayerData.GetInstance().GetBattleAvatar();
        m_GroupMemberDataList.Add(l_GroupMemberData);
    }

    // Шаблонный метод
    public void InitGroupButtonList()
    {
        groupButtonList.Clear();
        groupButtonList.ClearEvents();
        groupButtonList.AddCancelAction(GroupButtonListCancelAction);
        groupButtonList.AddKeyArrowAction(ShowGroupMemberInfo);
        groupButtonList.isActive = false;

        foreach (var groupMemberData in m_GroupMemberDataList)
        {
            AddGroupMember(groupMemberData);
        }
    }

    // Операция-зацепка для InitGroupButtonList
    public virtual void GroupButtonListCancelAction() { }

    // Шаблонный метод
    public void AddGroupMember(GroupMemberData groupMemberData)
    {
        InventoryGroupMemberButton l_Button = UnityEngine.Object.Instantiate(InventoryGroupMemberButton.prefab);
        l_Button.groupMemberData = groupMemberData;
        l_Button.SetAvatar(groupMemberData.avatatarSprite);
        l_Button.AddAction(GroupMemberButtonAction);
        groupButtonList.AddButton(l_Button);
    }

    // Операция-зацепка для AddGroupMember
    public virtual void GroupMemberButtonAction() { }

    public void ShowGroupMemberInfo()
    {
        InitSlots(PlayerInventory.GetInstance().GetInventorySlotData());
        InventoryGroupMemberButton l_GroupMemberButton = (InventoryGroupMemberButton)groupButtonList[groupButtonList.currentButtonId];
        playerStat.gameObject.SetActive(true);
        m_HealthStatText.text = LocalizationDataBase.GetInstance().GetText("Stat:HealthPoints") + " : " + l_GroupMemberButton.groupMemberData.m_Health;
        m_SpecialPointStatText.text = LocalizationDataBase.GetInstance().GetText("Stat:MonstylePoints") + " : " + l_GroupMemberButton.groupMemberData.m_SpecialPoints;
        m_AttackStatText.text = LocalizationDataBase.GetInstance().GetText("Stat:Attack") + " : " + l_GroupMemberButton.groupMemberData.m_AttackStat;
        m_DefenseStatText.text = LocalizationDataBase.GetInstance().GetText("Stat:Defense") + " : " + l_GroupMemberButton.groupMemberData.m_DefenseStat;
        m_SpeedStatText.text = LocalizationDataBase.GetInstance().GetText("Stat:Speed") + " : " + l_GroupMemberButton.groupMemberData.m_SpeedStat;
    }

    // Шаблонный метод
    private void InitSlots(Dictionary<string, InventorySlotData> p_SlotData)
    {
        slotButtonList.Clear();
        foreach (var lKey in p_SlotData.Keys)
        {
            AddSlot(p_SlotData[lKey]);
        }
    }

    // Операция-зацепка для InitSlots
    public virtual void AddSlot(InventorySlotData p_SlotData)
    {
        if (PlayerInventory.GetInstance().GetItemCount(p_SlotData.itemId) == 0)
            p_SlotData.itemId = string.Empty;
    }

    // Шаблонный метод
    public void InitItemButtonList()
    {
        m_ItemButtonsList = itemButtonList;
        m_ItemButtonsList.AddCancelAction(ItemButtonListCancelAction);
        m_ItemButtonsList.AddKeyArrowAction(ShowDescription);
        m_ItemButtonsList.isActive = false;
        m_ItemButtonListScrolling = itemButtonListScrolling;
        m_ItemButtonsList.AddKeyArrowAction(m_ItemButtonListScrolling.CheckScrolling);
    }

    // Операция-зацепка для InitItemButtonList
    public virtual void ItemButtonListCancelAction() { }

    public void ShowDescription()
    {
        if (itemButtonList != null && itemButtonList.count > 0)
        {
            InventoryItemButton lItemButton = (InventoryItemButton)itemButtonList.currentButton;
            int lCountInInventory = PlayerInventory.GetInstance().GetItemCount(lItemButton.itemId);
            string lInInventoryText = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:InInventory");
            descriptionText.text = lItemButton.title + "_Description" + lInInventoryText + lCountInInventory;
        }
    }

    public virtual void InitItemList()
    {
        itemButtonListScrolling.Init(42.0f, 12);
        itemButtonListScrolling.scrollRect.verticalNormalizedPosition = 1.0f;
    }

    public void ClearGroupMemberInfo()
    {
        playerStat.gameObject.SetActive(false);
    }

    public void ClearDescription()
    {
        descriptionText.text = "";
    }

    public void InitEmptySlots()
    {
        slotButtonList.Clear();
        for (int i = 0; i < emptySlotsCount; i++)
        {
            InventorySlotButton l_Button = UnityEngine.Object.Instantiate(InventorySlotButton.prefab);
            l_Button.title = "-------";
            slotButtonList.AddButton(l_Button);
        }
        slotButtonList.isActive = false;
    }
    #endregion
}
