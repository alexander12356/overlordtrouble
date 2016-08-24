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

    public static JourneySystem GetInstance()
    {
        return m_Instance;
    }

	public void Awake ()
    {
        m_Instance = this;
        LoadDataBases();
        SetControl(ControlType.Player);
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
                PanelManager.GetInstance().enabled = true;
                m_Player.StopLogic();
                break;
            case ControlType.Player:
                PanelManager.GetInstance().enabled = false;
                m_Player.StartLogic();
                break;
        }
    }

    private void LoadDataBases()
    {
        SkillDataBase.GetInstance();
        ImproveDataBase.GetInstance();
        ItemDataBase.GetInstance();
        StoreDataBase.GetInstance();
    }
}
