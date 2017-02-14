using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public abstract class StoreTab : MonoBehaviour
{
    #region Variables
    private ButtonList m_ItemsButtonList;
    private ButtonListScrolling m_ButtonListScrolling;
    public StorePanel parent { get; set; }
    private Text m_DescriptionText;
    private IItemsGetter m_StoreItemsGetter = null;
    #endregion

    #region Interface

    public IItemsGetter storeItemsGetter
    {
        get
        {
            return m_StoreItemsGetter;
        }
        set
        {
            m_StoreItemsGetter = value;
        }
    }

    public ButtonList itemsButtonList
    {
        get
        {
            if (m_ItemsButtonList == null)
            {
                m_ItemsButtonList = parent.transform.FindChild("Window").FindChild("ItemList").GetComponentInChildren<ButtonList>();
            }
            return m_ItemsButtonList;
        }
    }

    public Text descriptionText
    {
        get
        {
            if(m_DescriptionText == null)
            {
                m_DescriptionText = parent.transform.FindChild("Window").FindChild("Description").GetComponent<Text>();
            }
            return m_DescriptionText;
        }
    }

    public ButtonListScrolling buttonListScrolling
    {
        get
        {
            if(m_ButtonListScrolling == null)
            {
                m_ButtonListScrolling = parent.transform.FindChild("Window").FindChild("ItemList").GetComponent<ButtonListScrolling>();
            }
            return m_ButtonListScrolling;
        }
    }

    #endregion

    public abstract void SelectItem();
    public abstract void DeselectItem();
    public virtual void InitItemList()
    {
        buttonListScrolling.Init(70.0f, 8);
        buttonListScrolling.scrollRect.verticalNormalizedPosition = 1.0f;
    }

    public void Init()
    {
        InitItemsButtonList();
        InitItemList();
    }

    private void InitItemsButtonList()
    {
        itemsButtonList.AddCancelAction(CancelAction);
        itemsButtonList.AddKeyArrowAction(ShowItemDescription);
        itemsButtonList.isActive = false;
        itemsButtonList.AddKeyArrowAction(buttonListScrolling.CheckScrolling);
    }

    public virtual bool Confirm()
    {
        if (itemsButtonList != null && itemsButtonList.count > 0)
        {
            itemsButtonList.isActive = true;
            ShowItemDescription();
            return true;
        }
        return false;
    }

    public void CancelAction()
    {
        itemsButtonList.isActive = false;
        parent.tabButtonList.isActive = true;
        ClearDescription();
    }

    public virtual void ShowItemDescription()
    {
    }

    public void ClearDescription()
    {
        descriptionText.text = "";
    }

    public virtual void Disable()
    {
    }

    public virtual void UpdateKey()
    {
        itemsButtonList.UpdateKey();
    }
}
