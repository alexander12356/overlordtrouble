using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenlightButton : PanelButton
{
    private static GreenlightButton m_Prefab;

    public static GreenlightButton prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<GreenlightButton>("Prefabs/Button/GreenlightButton");
            }
            return m_Prefab;
        }
    }
}
