using UnityEngine;
using System.Collections;
using System;

public enum ControlType
{
    Player,
    Panel
}

public class JourneySystem : MonoBehaviour
{
    private static JourneySystem m_Instance;

    [SerializeField]
    private JourneyPlayer m_Player = null;

    [SerializeField]
    private PanelManager m_PanelManager = null;

    public static JourneySystem GetInstance()
    {
        return m_Instance;
    }

	public void Awake ()
    {
        m_Instance = this;
    }

    public void StartDialog(string p_DialogId)
    {
        SetControl(ControlType.Panel);

        DialogManager.GetInstance().StartDialog(p_DialogId);
    }

    public void SetControl(ControlType p_Type)
    {
        switch (p_Type)
        {
            case ControlType.Panel:
                m_Player.StopLogic();
                break;
            case ControlType.Player:
                m_Player.StartLogic();
                break;
        }
    }
}
