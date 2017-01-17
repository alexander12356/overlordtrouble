using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private ControlType m_CurrentControlType = ControlType.Player;

    [SerializeField]
    private JourneyPlayer m_Player = null;

    [SerializeField]
    private PanelManager m_PanelManager = null;

    public PanelManager panelManager
    {
        get { return m_PanelManager; }
    }
    public JourneyPlayer player
    {
        get { return m_Player; }
    }
    public static JourneySystem GetInstance()
    {
        return m_Instance;
    }
    public static bool IsInstance()
    {
        if (m_Instance == null)
        {
            return false;
        }
        return true;
    }
    public ControlType currentControlType
    {
        get { return m_CurrentControlType; }
    }

    public void Awake()
    {
        m_Instance = this;

        if (GameManager.IsInstance() == false)
        {
            GameManager.GetInstance();
            PlayerData.GetInstance().ResetData();
        }
    }

    public void Start()
    {
        LocationWarpSystem.GetInstance().SetPlayerPos();

        SetControl(ControlType.Player);
        m_Player.LoadImprove();
    }

    public DialogPanel StartDialog(string p_DialogId, List<ActionStruct> p_AnswerActionList = null)
    {
        if (m_CurrentControlType != ControlType.Cutscene)
        {
            SetControl(ControlType.Panel);
        }

        return DialogManager.GetInstance().StartDialog(p_DialogId, p_AnswerActionList);
    }

    public void SetControl(ControlType p_Type)
    {
        m_CurrentControlType = p_Type;
        switch (p_Type)
        {
            case ControlType.Panel:
                m_PanelManager.enabled = true;
                m_Player.StopLogic();
                CutsceneSystem.GetInstance().enabled = false;
                break;
            case ControlType.Player:
                if (m_PanelManager.panelCount > 0)
                {
                    break;
                }
                m_PanelManager.enabled = false;
                m_Player.StartLogic();
                CutsceneSystem.GetInstance().enabled = false;
                break;
            case ControlType.Cutscene:
                if (m_PanelManager.panelCount > 0)
                {
                    break;
                }
                m_PanelManager.enabled = true;
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

    public void ReturnToMainMenu()
    {
        m_PanelManager.StartLocation("MainMenu");
    }

    public void OpenProfile()
    {
        m_PanelManager.OpenProfile();
    }

    public void OnEnable()
    {
        m_Player.LoadImprove();
    }
}
