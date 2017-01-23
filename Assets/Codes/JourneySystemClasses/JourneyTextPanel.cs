using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JourneyTextPanel : TextPanel
{
    private static JourneyTextPanel m_Prefab = null;

    public static JourneyTextPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<JourneyTextPanel>("Prefabs/Panels/JourneyTextPanel");
            }
            return m_Prefab;
        }
    }
}
