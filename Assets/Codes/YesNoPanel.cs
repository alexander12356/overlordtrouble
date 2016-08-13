using UnityEngine;
using System.Collections;

public class YesNoPanel : Panel
{
    private static YesNoPanel m_Prefab = null;
    private ButtonList m_ButtonList = null;

    public static YesNoPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<YesNoPanel>("Prefabs/Panels/YesNoPanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_ButtonList = GetComponent<ButtonList>();
        m_ButtonList[0].AddAction(Cancel);
        m_ButtonList[1].AddAction(Cancel);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ButtonList.UpdateKey();
    }

    public void Cancel()
    {
        PanelManager.GetInstance().ClosePanel(this);
    }
}
