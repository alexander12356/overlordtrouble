using UnityEngine;
using UnityEngine.UI;

public class InventorySlotButton : PanelButton
{
    private static InventorySlotButton m_Prefab = null;
    private InventorySlotData m_SlotData;
    private Text m_CapacityDetectorText = null;
    private bool m_IsFull;

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

    public Text capacityDetectorText
    {
        get
        {
            if(m_CapacityDetectorText == null)
            {
                m_CapacityDetectorText = transform.FindChild("CapacityDetectorText").GetComponent<Text>();
            }
            return m_CapacityDetectorText;
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
                capacityDetectorText.gameObject.SetActive(false);
            }
            else
            {
                m_IsFull = false;
                capacityDetectorText.gameObject.SetActive(true);
            }
        }
    }

    public void SelectItem(string pItemId)
    {
        m_IsFull = true;
        m_SlotData.itemId = pItemId;
        capacityDetectorText.gameObject.SetActive(false);
    }

    public void DeselectItem()
    {
        m_IsFull = false;
        m_SlotData.itemId = string.Empty;
        capacityDetectorText.gameObject.SetActive(true);
    }
}
