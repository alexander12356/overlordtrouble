using UnityEngine;
using System.Collections;
using System;

public enum ControlType
{
    None,
    Player,
    Panel,
    Cutscene
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

    public void Awake()
    {
        m_Instance = this;
        LoadDataBases();
        CutsceneSystem.GetInstance().StartCutscene("Intro");
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
                CutsceneSystem.GetInstance().enabled = false;
                break;
            case ControlType.Player:
                PanelManager.GetInstance().enabled = false;
                m_Player.StartLogic();
                CutsceneSystem.GetInstance().enabled = false;
                break;
            case ControlType.Cutscene:
                PanelManager.GetInstance().enabled = false;
                m_Player.StopLogic();
                CutsceneSystem.GetInstance().enabled = true;
                break;
            case ControlType.None:
                PanelManager.GetInstance().enabled = false;
                m_Player.StopLogic();
                CutsceneSystem.GetInstance().enabled = false;
                break;
        }
    }

    private void LoadDataBases()
    {
        DataLoader.GetInstance();
    }
}
