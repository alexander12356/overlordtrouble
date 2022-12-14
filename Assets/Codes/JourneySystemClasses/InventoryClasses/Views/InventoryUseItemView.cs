using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUseItemView : InventoryView
{
    PanelActionHandler m_UseAction;
    PanelActionHandler m_CancelAction;

    public InventoryUseItemView(InventoryPanel p_Parent)
    {
        parent = p_Parent;
    }

    public override void Init()
    {
        InitGroupData();
        InitGroupButtonList();
        enabled = true;
    }

    public override void AddSlot(InventorySlotData p_SlotData)
    {
        base.AddSlot(p_SlotData);

        InventorySlotButton l_Button = UnityEngine.Object.Instantiate(InventorySlotButton.prefab);
        l_Button.slotData = p_SlotData;
        l_Button.defaultTitle = p_SlotData.slotType.GetTitle(slotButtonList.count);
        slotButtonList.AddButton(l_Button);
    }

    public override void GroupMemberButtonAction()
    {
        if (m_UseAction != null)
        {
            m_UseAction();
        }
        GroupMemberButtonAnimation();
        RefreshGroupMemberInfo();
        Cancel();
    }

    private void GroupMemberButtonAnimation()
    {
        InventoryGroupMemberButton l_GroupMemberButton = (InventoryGroupMemberButton)groupButtonList[groupButtonList.currentButtonId];
        l_GroupMemberButton.Blink();
    }

    public override void GroupButtonListCancelAction()
    {      
        if (m_CancelAction != null)
        {
            m_CancelAction();
        }
        Cancel();
    }

    private void Cancel()
    {
        enabled = false;
        groupButtonList.isActive = false;
        itemButtonList.isActive = true;
        InitEmptySlots();
        ClearGroupMemberInfo();
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

    public override bool Confirm()
    {
        groupButtonList.isActive = true;
        ShowGroupMemberInfo();
        return true;
    }

    public override bool isNull()
    {
        return false;
    }
}
