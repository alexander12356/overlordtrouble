using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionPanel : Panel
{
    private static ItemActionPanel m_Prefab = null;
    private ButtonList m_ActionsButtonList;

    private InventoryUseItemView m_UseItemView = null;
    private PanelActionHandlerWithParameter m_ItemListRefreshAction;
    private Animator m_Animator = null;
    private string m_ItemId;

    private int m_CountToRemove = 0;
    private const int m_MinValue = 0;
    private const int m_MaxValue = 99;

    [SerializeField]
    private GameObject m_RemovingArrows = null;
    [SerializeField]
    private Text m_CountToRemoveText = null;

    public static ItemActionPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<ItemActionPanel>("Prefabs/Panels/ItemActionPanel");
            }
            return m_Prefab;
        }
    }

    private InventoryUseItemView useItemView
    {
        get
        {
            if(m_UseItemView == null)
            {
                InventoryPanel l_InventoryPanel = FindObjectOfType<InventoryPanel>();
                m_UseItemView = new InventoryUseItemView(l_InventoryPanel);
            }
            return m_UseItemView;
        }
    }

    public ButtonList actionButtonList
    {
        get
        {
            if (m_ActionsButtonList == null)
            {
                m_ActionsButtonList = transform.FindChild("ActionList").GetComponentInChildren<ButtonList>();
            }
            return m_ActionsButtonList;
        }
    }

    public int countToRemove
    {
        get
        {
            return m_CountToRemove;
        }
        set
        {
            m_CountToRemove = Mathf.Clamp(value, m_MinValue, m_MaxValue);
            m_CountToRemoveText.text = m_CountToRemove.ToString();
        }
    }

    public string itemId
    {
        get
        {
            return m_ItemId;
        }
        set
        {
            m_ItemId = value;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_ActionsButtonList = actionButtonList;
        m_Animator = GetComponent<Animator>();
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
            return;

        if (m_RemovingArrows.activeInHierarchy)
        {
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                countToRemove += 1;
                m_Animator.SetTrigger("RightArrow");
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                countToRemove -= 1;
                m_Animator.SetTrigger("LeftArrow");
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                countToRemove += 10;
                m_Animator.SetTrigger("UpArrow");
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                countToRemove -= 10;
                m_Animator.SetTrigger("DownArrow");
            }
            else if (Input.GetKeyUp(KeyCode.Z))
            {
                TryToRemove();
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                CancelRemove();
            }
        }
        else if (useItemView.enabled)
        {
            useItemView.UpdateKey();
        }
        else
        {
            m_ActionsButtonList.UpdateKey();
        }
    }

    private void CancelRemove()
    {
        m_RemovingArrows.SetActive(false);
        countToRemove = 0;
    }

    public void InitActionButtonList(string p_ItemId)
    {
        itemId = p_ItemId;
        if (ItemDataBase.GetInstance().GetItem(itemId).itemType == ItemType.SingleUse)
        {
            string l_UseStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Use");
            AddActionButton(l_UseStr, TryUseItem);

            string l_ThrowStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Throw");
            AddActionButton(l_ThrowStr, ActivateArrows);

            string l_BackStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Back");
            AddActionButton(l_BackStr, CancelAction);
        }
        else if (ItemDataBase.GetInstance().GetItem(itemId).itemType == ItemType.Bling || ItemDataBase.GetInstance().GetItem(p_ItemId).itemType == ItemType.Weapon || ItemDataBase.GetInstance().GetItem(p_ItemId).itemType == ItemType.MultipleUse)
        {
            string l_ThrowStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Throw");
            AddActionButton(l_ThrowStr, ActivateArrows);

            string l_BackStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Back");
            AddActionButton(l_BackStr, CancelAction);
        }
        else if (ItemDataBase.GetInstance().GetItem(itemId).itemType == ItemType.Key)
        {
            string l_UseStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Use");
            AddActionButton(l_UseStr, UseKeyItem);

            string l_BackStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Back");
            AddActionButton(l_BackStr, CancelAction);
        }
    }


    private void AddActionButton(string p_ButtonTitle, PanelButtonActionHandler p_Action)
    {
        ItemActionButton l_ItemActionButton = Instantiate(ItemActionButton.prefab);
        l_ItemActionButton.title = p_ButtonTitle;
        l_ItemActionButton.AddAction(p_Action);

        actionButtonList.AddButton(l_ItemActionButton);
    }

    public void AddItemListRefreshAction(PanelActionHandlerWithParameter p_Action)
    {
        m_ItemListRefreshAction += p_Action;
    }

    private void TryUseItem()
    {
        useItemView.Init();
        useItemView.Confirm();
        useItemView.AddCancelAction(CancelAction);
        useItemView.AddUseAction(UseItem);
    }

    private void UseKeyItem()
    {
        ShowMessage(LocalizationDataBase.GetInstance().GetText("Item:" + itemId + ":Effect"), CancelAction);
        // TODO : Использование предмета
        Item l_Item = ItemDataBase.GetInstance().GetItem(itemId).CreateItem();
        l_Item.Run(JourneySystem.GetInstance().player.statistics);
    }

    private void UseItem()
    {
        if (m_ItemListRefreshAction != null)
        {
            m_ItemListRefreshAction(1);
        }
        ShowMessage(LocalizationDataBase.GetInstance().GetText("Item:" + itemId + ":Effect"), CancelAction);
        // TODO : Использование предмета
        Item l_Item = ItemDataBase.GetInstance().GetItem(itemId).CreateItem();
        l_Item.Run(JourneySystem.GetInstance().player.statistics);
    }

    private void ShowMessage(string p_Message, PanelButtonActionHandler p_CancelAction)
    {
        InventoryTextPanel l_TextPanel = Instantiate(InventoryTextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { p_Message });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);
        l_TextPanel.AddButtonAction(p_CancelAction);
        JourneySystem.GetInstance().ShowPanel(l_TextPanel, true);
    }

    private void ActivateArrows()
    {
        m_RemovingArrows.SetActive(true);
    }

    private void TryToRemove()
    {
        YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
        // TODO : Заменить локализованным текстом
        l_YesNoPanel.SetText(string.Format("Вы действительно хотите выбросить {0} ?", LocalizationDataBase.GetInstance().GetText("Item:" + itemId)));
        l_YesNoPanel.AddYesAction(RemovingAction);
        l_YesNoPanel.AddPopAction(CancelAction);
        JourneySystem.GetInstance().ShowPanel(l_YesNoPanel, true);
    }

    private void RemovingAction()
    {
        if (m_ItemListRefreshAction != null)
        {
            m_ItemListRefreshAction(countToRemove);
        }
    }

    private void CancelAction()
    {
        RemovingAction();
        Close();
    }
}
