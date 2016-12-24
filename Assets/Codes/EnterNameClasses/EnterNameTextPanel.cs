using UnityEngine;

public class EnterNameTextPanel : TextPanel
{
    private static EnterNameTextPanel m_Prefab;

    public static EnterNameTextPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<EnterNameTextPanel>("Prefabs/Panels/EnterNameTextPanel");
            }
            return m_Prefab;
        }
    }
}
