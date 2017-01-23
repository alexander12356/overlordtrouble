using UnityEngine;

public class InventoryTextPanel : TextPanel
{
    private static InventoryTextPanel m_Prefab;

    public static InventoryTextPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<InventoryTextPanel>("Prefabs/Panels/InventoryTextPanel");
            }
            return m_Prefab;
        }
    }
}
