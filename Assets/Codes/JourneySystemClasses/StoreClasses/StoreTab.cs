using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class StoreTab : MonoBehaviour
{
    #region Variables
    private ButtonList m_ItemsButtonList;
    private StorePanel m_StorePanel;
    private Text m_DescriptionText;
    #endregion

    #region Interface
    public ButtonList itemsButtonList
    {
        get
        {
            if (m_ItemsButtonList == null)
            {
                m_ItemsButtonList = GetComponentInChildren<ButtonList>();
            }
            return m_ItemsButtonList;
        }
    }

    public void Awake()
    {
        m_ItemsButtonList = itemsButtonList;
        m_ItemsButtonList.AddCancelAction(CancelAction);
        m_ItemsButtonList.AddKeyArrowAction(ShowItemDescription);

        m_DescriptionText = GetComponentInChildren<Text>();
        m_StorePanel = GetComponentInParent<StorePanel>();
    }

    public void AddItem(StoreItemData p_StoreItemData)
    {
        StoreItemButton l_Button = Instantiate(StoreItemButton.prefab);
        l_Button.title = LocalizationDataBase.GetInstance().GetText("Item:" + p_StoreItemData.id);
        l_Button.itemId = p_StoreItemData.id;
        l_Button.AddAction(SelectItem);

        itemsButtonList.AddButton(l_Button);

        Vector3 l_ButtonPosition = l_Button.transform.localPosition;
        l_ButtonPosition.x = 0.0f;
        l_Button.transform.localPosition = l_ButtonPosition;
    }

    public void Confirm()
    {
        ShowItemDescription();
        m_ItemsButtonList.isActive = true;
    }

    public void CheckCountInInventory()
    {
        for (int i = 0; i < m_ItemsButtonList.count; i++)
        {
            StoreItemButton l_StoreItemButton = (StoreItemButton)m_ItemsButtonList[i];
            l_StoreItemButton.itemCost = PlayerInventory.GetInstance().GetItemCount(l_StoreItemButton.itemId);
        }
    }
    #endregion

    #region Private
    private void CancelAction()
    {
        m_ItemsButtonList.isActive = false;
        m_StorePanel.tabButtonList.isActive = true;
        m_DescriptionText.text = "Описание:";
    }

    private void ShowItemDescription()
    {
        StoreItemButton m_StoreItemButton = (StoreItemButton)m_ItemsButtonList.currentButton;
        int l_CountInInventory = PlayerInventory.GetInstance().GetItemCount(m_StoreItemButton.itemId);
        m_DescriptionText.text = "Описание:\n" + m_StoreItemButton.title + "_Description" + "\nУ тебя в инвентаре: " + l_CountInInventory;
    }

    private void SelectItem()
    {
        StoreItemButton l_StoreItemButton = (StoreItemButton)m_ItemsButtonList.currentButton;
        l_StoreItemButton.Activate(true);
        l_StoreItemButton.AddCancelAction(DeselectItem);
        l_StoreItemButton.AddBuyAction(Buy);
        m_ItemsButtonList.isActive = false;

        m_StorePanel.currentSelectedItem = l_StoreItemButton;
    }

    private void DeselectItem()
    {
        StoreItemButton l_StoreItemButton = (StoreItemButton)m_ItemsButtonList.currentButton;
        l_StoreItemButton.Activate(false);
        l_StoreItemButton.RemoveCancelAction(DeselectItem);
        l_StoreItemButton.RemoveBuyAction(Buy);

        m_StorePanel.currentSelectedItem = null;
        m_ItemsButtonList.isActive = true;
    }

    private void Buy()
    {
        StoreItemButton l_StoreItemButton = (StoreItemButton)m_ItemsButtonList.currentButton;
        int    l_CountToBuy = l_StoreItemButton.countToBuy;
        string l_ItemId     = l_StoreItemButton.itemId;
        int    l_ItemCost = l_StoreItemButton.itemCost;
        
        if (l_ItemCost * l_CountToBuy <= m_StorePanel.playerCoins)
        {
            m_StorePanel.playerCoins -= l_ItemCost * l_CountToBuy;
            PlayerInventory.GetInstance().AddItem(l_ItemId, l_CountToBuy);
            ShowItemDescription();
        }
    }
    #endregion
}
