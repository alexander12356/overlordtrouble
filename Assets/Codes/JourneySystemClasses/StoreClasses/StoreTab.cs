using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using System;

public class StoreTab : MonoBehaviour
{
    private ButtonList m_ItemsButtonList;
    private StorePanel m_StorePanel;
    private Text m_DescriptionText;

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

    public void SetItems(Dictionary<string, StoreItemData> m_Items)
    {
        foreach(string l_ItemId in m_Items.Keys)
        {
            PanelButton l_Button = Instantiate(PanelButton.prefab);
            l_Button.title = m_Items[l_ItemId].id;
            itemsButtonList.AddButton(l_Button);
        }
    }

    public void CancelAction()
    {
        m_ItemsButtonList.isActive = false;
        m_StorePanel.tabButtonList.isActive = true;
        m_DescriptionText.text = "Описание:";
    }

    public void Confirm()
    {
        ShowItemDescription();
        m_ItemsButtonList.isActive = true;
    }

    private void ShowItemDescription()
    {
        m_DescriptionText.text = "Описание:\n" + m_ItemsButtonList.currentButton.title + "_Description";
    }
}
