using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : PanelButton {

    private static DeleteButton m_Prefab = null;

    public static DeleteButton prefab
    {
        get
        {
            if(m_Prefab == null)
            {
                m_Prefab = Resources.Load<DeleteButton>("Prefabs/Button/DeleteButton");
            }
            return m_Prefab;
        }
    }
}
