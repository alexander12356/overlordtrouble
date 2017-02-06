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
        itemsButtonList.AddCancelAction(CancelAction);
        itemsButtonList.AddKeyArrowAction(ShowItemDescription);

        m_DescriptionText = GetComponentInChildren<Text>();
        m_DescriptionText.text = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Description");
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
        itemsButtonList.isActive = true;
    }

    public void CheckCountInInventory()
    {
        for (int i = 0; i < itemsButtonList.count; i++)
        {
            StoreItemButton l_StoreItemButton = (StoreItemButton)itemsButtonList[i];
            l_StoreItemButton.itemCost = PlayerInventory.GetInstance().GetItemCount(l_StoreItemButton.itemId);
        }
    }
    #endregion

    #region Private
    private void CancelAction()
    {
        itemsButtonList.isActive = false;
        m_StorePanel.tabButtonList.isActive = true;
        m_DescriptionText.text = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Description");
    }

    private void ShowItemDescription()
    {
        StoreItemButton m_StoreItemButton = (StoreItemButton)itemsButtonList.currentButton;
        int l_CountInInventory = PlayerInventory.GetInstance().GetItemCount(m_StoreItemButton.itemId);
        string l_DescriptionText = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Description");
        string l_InInventoryText = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:InInventory");
        m_DescriptionText.text = l_DescriptionText + m_StoreItemButton.title + "_Description" + l_InInventoryText + l_CountInInventory;
    }

    private void SelectItem()
    {
        StoreItemButton l_StoreItemButton = (StoreItemButton)itemsButtonList.currentButton;
        l_StoreItemButton.Activate(true);
        l_StoreItemButton.AddCancelAction(DeselectItem);
        l_StoreItemButton.AddBuyAction(Buy);
        itemsButtonList.isActive = false;

        m_StorePanel.currentSelectedItem = l_StoreItemButton;
    }

    private void DeselectItem()
    {
        StoreItemButton l_StoreItemButton = (StoreItemButton)itemsButtonList.currentButton;
        l_StoreItemButton.Activate(false);
        l_StoreItemButton.RemoveCancelAction(DeselectItem);
        l_StoreItemButton.RemoveBuyAction(Buy);

        m_StorePanel.currentSelectedItem = null;
        itemsButtonList.isActive = true;
    }

    private void Buy()
    {
        StoreItemButton l_StoreItemButton = (StoreItemButton)itemsButtonList.currentButton;
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
