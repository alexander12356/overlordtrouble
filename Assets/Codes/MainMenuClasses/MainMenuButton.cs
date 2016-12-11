using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : PanelButton {
    private static MainMenuButton m_Prefab;
    
    public static MainMenuButton prefab
    {
        get
        {
            if(m_Prefab == null)
            {
                m_Prefab = Resources.Load<MainMenuButton>("Prefabs/Button/MainMenuButton");
            }
            return m_Prefab;
        }
    }
}
