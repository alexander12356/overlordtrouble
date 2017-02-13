﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class ItemsPanel : Panel
{
    private static ItemsPanel m_Prefab = null;
    private ItemPanelButton m_ChoosedItemButton = null;

    [SerializeField]
    private ButtonList m_ItemsButtonList = null;

    [SerializeField]
    private ButtonList m_ConfirmButtonList = null;

    [SerializeField]
    private ButtonList m_TeamButtonList = null;

    [SerializeField]
    private ButtonListScrolling m_ButtonListScrolling = null;

    public static ItemsPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<ItemsPanel>("Prefabs/Panels/BattleSystem/ItemsPanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        PlayerInventory.GetInstance().NewGameDataInit();

        m_ItemsButtonList = GetComponentInChildren<ButtonList>();
    }

    public void Start()
    {
        InitItems();
        InitTeam();

        m_ButtonListScrolling.Init(120.0f, 3);
        m_ItemsButtonList.AddKeyArrowAction(m_ButtonListScrolling.CheckScrolling);

        m_TeamButtonList.isActive = false;
        m_ConfirmButtonList.isActive = false;

        m_ConfirmButtonList[0].AddAction(UseItem);
        m_ConfirmButtonList[0].title = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:UseSpecials");
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (m_ItemsButtonList.isActive && Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_ItemsButtonList.isActive = false;
            m_ConfirmButtonList.isActive = true;
        }
        if (m_ConfirmButtonList && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_ItemsButtonList.isActive = true;
            m_ConfirmButtonList.isActive = false;
        }

        m_ItemsButtonList.UpdateKey();
        m_ConfirmButtonList.UpdateKey();

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (m_ConfirmButtonList.isActive)
            {
                m_ItemsButtonList.isActive = true;
                m_ConfirmButtonList.isActive = false;
            }
            else
            {
                ReturnToMain();
            }
        }
    }

    private void InitItems()
    {
        Dictionary<string, InventoryItemData> l_ItemsDictionary = PlayerInventory.GetInstance().GetInventoryItems().Where(obj => ItemDataBase.GetInstance().GetItem(obj.Key).itemType == ItemType.SingleUse).ToDictionary(obj => obj.Key, obj => obj.Value);

        foreach (InventoryItemData l_ItemData in l_ItemsDictionary.Values)
        {
            ItemPanelButton l_Button = Instantiate(ItemPanelButton.prefab);
            l_Button.itemId = l_ItemData.id;
            l_Button.title = LocalizationDataBase.GetInstance().GetText("Item:" + l_ItemData.id);
            l_Button.description = LocalizationDataBase.GetInstance().GetText("Item:" + l_ItemData.id + ":Description");
            l_Button.AddAction(ChooseItem);

            m_ItemsButtonList.AddButton(l_Button);
        }
    }

    private void InitTeam()
    {
        PanelButtonChosenSpecial l_TeamMember = Instantiate(PanelButtonChosenSpecial.prefab);
        l_TeamMember.title = BattlePlayer.GetInstance().actorName;

        m_TeamButtonList.AddButton(l_TeamMember);
    }

    private void ReturnToMain()
    {
        Close();
    }

    private void ChooseItem()
    {
        if (m_ChoosedItemButton != null)
        {
            m_ChoosedItemButton.Choose(false);
        }
        m_ChoosedItemButton = m_ItemsButtonList.currentButton as ItemPanelButton;
        m_ChoosedItemButton.Choose(true);
    }

    private void UseItem()
    {
        Close();

        string l_ItemId = m_ChoosedItemButton.itemId;
        Item l_Item = ItemDataBase.GetInstance().GetItem(l_ItemId).CreateItem();

        BattlePlayer.GetInstance().UseItem(BattlePlayer.GetInstance(), l_Item);
    }
}