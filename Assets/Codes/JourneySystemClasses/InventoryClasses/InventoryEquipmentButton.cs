using UnityEngine;
using UnityEngine.UI;

public class InventoryEquipmentButton : PanelButton
{
    private static InventoryEquipmentButton m_Prefab = null;
    private string m_ItemId = string.Empty;
    private bool m_Equipped = false;
    [SerializeField]
    private GameObject m_EquipmentText = null;

    public static InventoryEquipmentButton prefab
    {
        get
        {
            if(m_Prefab == null)
            {
                m_Prefab = Resources.Load<InventoryEquipmentButton>("Prefabs/Button/InventoryEquipmentButton");
            }
            return m_Prefab;
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

    public bool equipped
    {
        get
        {
            return m_Equipped;
        }
        set
        {
            m_Equipped = value;
            m_EquipmentText.SetActive(m_Equipped);
        }
    }
}
