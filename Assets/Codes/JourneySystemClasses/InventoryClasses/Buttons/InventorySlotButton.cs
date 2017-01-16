using UnityEngine;
using UnityEngine.UI;

public class InventorySlotButton : PanelButton
{
    private static InventorySlotButton m_Prefab = null;
    private InventorySlotData m_SlotData;
    private bool m_IsFull;
    private string m_DefaultTitle;

    public static InventorySlotButton prefab
    {
        get
        {
            if(m_Prefab == null)
            {
                m_Prefab = Resources.Load<InventorySlotButton>("Prefabs/Button/InventorySlotButton");
            }
            return m_Prefab;
        }
    }

    public string itemId
    {
        get
        {
            return m_SlotData.itemId;
        }
    }

    public Slot slotType
    {
        get
        {
            return m_SlotData.slotType;
        }
    }

    public string slotId
    {
        get
        {
            return m_SlotData.slotId;
        }
    }

    public bool IsFull
    {
        get
        {
            return m_IsFull;
        }
    }

    public string defaultTitle
    {
        get
        {
            return m_DefaultTitle;
        }
        set
        {
            m_DefaultTitle = value;
            if (m_IsFull)
            {
                title = LocalizationDataBase.GetInstance().GetText("Item:" + m_SlotData.itemId);
            }
            else
            {
                title = m_DefaultTitle;
            }
        }
    }

    public InventorySlotData slotData
    {
        get
        {
            return m_SlotData;
        }
        set
        {
            m_SlotData = value;
            if(m_SlotData.itemId != string.Empty)
            {
                m_IsFull = true;
            }
            else
            {
                m_IsFull = false;
            }
        }
    }

    public void SelectItem(string pItemId)
    {
        m_IsFull = true;
        m_SlotData.itemId = pItemId;
        title = LocalizationDataBase.GetInstance().GetText("Item:" + pItemId);
    }

    public void DeselectItem()
    {
        m_IsFull = false;
        m_SlotData.itemId = string.Empty;
        title = defaultTitle;
    }
}
