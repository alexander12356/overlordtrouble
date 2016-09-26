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

    public PanelManager panelManager
    {
        get { return m_PanelManager; }
    }

    public static JourneySystem GetInstance()
    {
        return m_Instance;
    }

    public void Awake()
    {
        m_Instance = this;
        CutsceneSystem.GetInstance().StartCutscene("Intro");

        if (GameManager.IsInstance() == false)
        {
            GameManager.GetInstance();
            PlayerData.GetInstance().ResetData();
            m_Player.LoadImprove();
        }
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
                m_PanelManager.enabled = true;
                m_Player.StopLogic();
                CutsceneSystem.GetInstance().enabled = false;
                break;
            case ControlType.Player:
                m_PanelManager.enabled = false;
                m_Player.StartLogic();
                CutsceneSystem.GetInstance().enabled = false;
                break;
            case ControlType.Cutscene:
                m_PanelManager.enabled = false;
                m_Player.StopLogic();
                CutsceneSystem.GetInstance().enabled = true;
                break;
            case ControlType.None:
                m_PanelManager.enabled = false;
                m_Player.StopLogic();
                CutsceneSystem.GetInstance().enabled = false;
                break;
        }
    }

    public void ShowPanel(Panel p_Panel, bool p_WithOverlay = false, Transform m_Parent = null)
    {
        m_PanelManager.ShowPanel(p_Panel, p_WithOverlay, m_Parent);
    }

    public void AddScene(string p_SceneId)
    {
        m_PanelManager.AddScene(p_SceneId);
        SetControl(ControlType.None);
    }

    public void StartLocation(string p_LocationId)
    {
        m_PanelManager.StartLocation(p_LocationId);
    }

    public void OpenProfile()
    {
        m_PanelManager.OpenProfile();
    }
}
