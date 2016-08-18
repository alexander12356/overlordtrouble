using UnityEngine;
using System.Collections;

public class StorePanel : Panel
{
    #region Variables
    private static StorePanel m_Prefab;
    private ButtonList m_ButtonList = null;
    #endregion

    #region Interface
    public static StorePanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<StorePanel>("Prefabs/Panels/StorePanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_ButtonList = GetComponentInChildren<ButtonList>();
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ButtonList.UpdateKey();
    }
    #endregion
}
