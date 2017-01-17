using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUseItemView : InventoryEquipmentView
{
    PanelActionHandler m_UseAction;
    PanelActionHandler m_CancelAction;
    // test data
    List<TestGroupMemberData> m_TestGroupMemberDataList = null;

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
        groupButtonList.AddCancelAction(Cancel);
        groupButtonList.AddKeyArrowAction(ShowGroupMemberInfo);
        groupButtonList.isActive = false;

        foreach (var groupMemberData in m_TestGroupMemberDataList)
        {
            AddGroupMember(groupMemberData);
        }
    }

    public override void AddGroupMember(TestGroupMemberData groupMemberData)
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
        Cancel();
    }

    public void Cancel()
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
}
