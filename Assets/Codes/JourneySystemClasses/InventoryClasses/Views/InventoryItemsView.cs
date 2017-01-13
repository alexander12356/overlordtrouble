using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemsView : InventoryView
{
    #region Variables
    private IInventoryItemsGetter m_InventoryItemsGetter = null;
    private ButtonList m_ItemsButtonsList;
    private Text m_DescriptionText;
    #endregion

    #region Interface
    public ButtonList itemsButtonList
    {
        get
        {
            if (m_ItemsButtonsList == null)
            {
                m_ItemsButtonsList = parent.transform.FindChild("ItemList").GetComponentInChildren<ButtonList>();
            }
            return m_ItemsButtonsList;
        }
    }

    public IInventoryItemsGetter inventoryItemsGetter
    {
        get
        {
            return m_InventoryItemsGetter;
        }
        set
        {
            m_InventoryItemsGetter = value;
        }
    }
    #endregion

    public InventoryItemsView(InventoryPanel p_Parent, IInventoryItemsGetter p_Getter)
    {
        parent = p_Parent;
        inventoryItemsGetter = p_Getter;
    }

    public override void Init()
    {
        m_ItemsButtonsList = itemsButtonList;
        m_ItemsButtonsList.AddCancelAction(CancelAction);
        m_ItemsButtonsList.AddKeyArrowAction(ShowDescription);
        m_ItemsButtonsList.isActive = false;

        m_DescriptionText = parent.transform.FindChild("Description").GetComponent<Text>();

        InitItemList();
        ShowDescription();
    }

    public override void Disable()
    {
        m_ItemsButtonsList.RemoveCancelAction(CancelAction);
        m_ItemsButtonsList.RemoveKeyArrowAction(ShowDescription);
        itemsButtonList.Clear();
    }

    public void InitItemList()
    {
        itemsButtonList.Clear();
        ClearDescription();
        if (inventoryItemsGetter != null)
        {
            Dictionary<string, InventoryItemData> l_InventoryItems = inventoryItemsGetter.GetItems();
            foreach (var lKey in l_InventoryItems.Keys)
            {
                AddItem(l_InventoryItems[lKey]);
            }
        }
    }

    public override void AddItem(InventoryItemData p_InventoryItemData)
    {
        InventoryItemButton l_Button = Object.Instantiate(InventoryItemButton.prefab);
        l_Button.title = LocalizationDataBase.GetInstance().GetText("Item:" + p_InventoryItemData.id);
        l_Button.itemId = p_InventoryItemData.id;
        l_Button.itemCount = p_InventoryItemData.count;
        l_Button.AddAction(SelectItem);

        itemsButtonList.AddButton(l_Button);
    }

    // TODO : Сделать нормальное описание
    public override void ShowDescription()
    {
        if (m_ItemsButtonsList != null && m_ItemsButtonsList.count > 0)
        {
            InventoryItemButton l_InventoryItemButton = (InventoryItemButton)m_ItemsButtonsList.currentButton;
            m_DescriptionText.text = l_InventoryItemButton.title;
        }
    }

    public override void ClearDescription()
    {
        m_DescriptionText.text = "";
    }

    public override void CancelAction()
    {
        m_ItemsButtonsList.isActive = false;
        parent.tabButtonList.isActive = true;
    }

    public override bool Confrim()
    {
        if (m_ItemsButtonsList != null && m_ItemsButtonsList.count > 0)
        {
            m_ItemsButtonsList.isActive = true;
            return true;
        }
        return false;
    }

    public override void SelectItem()
    {
        InventoryItemButton l_ItemButton = (InventoryItemButton)m_ItemsButtonsList.currentButton;
        l_ItemButton.CreateItemActionPanel();
    }

    public override void UpdateKey()
    {
        m_ItemsButtonsList.UpdateKey();
    }
}
