using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public enum ControlType
{
    None,
    Player,
    Panel,
    Cutscene,
    StartBattle
}

public class JourneySystem : MonoBehaviour
{
    private static JourneySystem m_Instance;
    private ControlType m_CurrentControlType = ControlType.Player;
    private UnityEvent m_OnPauseEvent = null;
    private UnityEvent m_OnResumeEvent = null;
    private bool m_IsPause = false;

    [SerializeField]
    private JourneyPlayer m_Player = null;

    [SerializeField]
    private PanelManager m_PanelManager = null;

    [SerializeField]
    private EnemyGeneratorSystem m_EnemyGeneratorSystem = null;

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
    public bool isPause
    {
        get { return m_IsPause; }
    }

    public void Awake()
    {
        m_Instance = this;

        m_OnPauseEvent = new UnityEvent();
        m_OnResumeEvent = new UnityEvent();

#if UNITY_EDITOR
        if (GameManager.IsInstance() == false)
        {
            GameManager.GetInstance();
            PlayerData.GetInstance().NewGameDataInit();
            PlayerInventory.GetInstance().NewGameDataInit();
            EnemyGenerate(RoomSystem.GetInstance().currentRoomId);
            AudioSystem.GetInstance().SetTheme(GameManager.GetInstance().currentSceneName);
            AudioSystem.GetInstance().PlayTheme();
            AudioSystem.GetInstance().ResumeMainTheme();
            RoomSystem.GetInstance().ChangeRoom(RoomSystem.GetInstance().currentRoomId);
        }
#endif
    }

    public void Start()
    {
        AudioSystem.GetInstance().PlayTheme();

        SaveSystem.GetInstance().LoadFromMemory();
        
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
            case ControlType.StartBattle:
                m_PanelManager.enabled = false;
                m_Player.StopLogic();
                CutsceneSystem.GetInstance().enabled = false;
                break;
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

    public void StartBattle()
    {
        AudioSystem.GetInstance().StopTheme();
        AddScene("BattleSystem");
        SetControl(ControlType.StartBattle);
    }

    public void StartLocation(string p_LocationId)
    {
        SaveSystem.GetInstance().SaveToMemory();
        SaveSystem.GetInstance().ClearCache();
        SaveSystem.GetInstance().Init(p_LocationId);

        AudioSystem.GetInstance().StopTheme();
        AudioSystem.GetInstance().SetTheme(p_LocationId);

        m_PanelManager.StartLocation(p_LocationId);
    }

    public void ReturnToMainMenu()
    {
        PlayerPrefs.DeleteAll();
        SaveSystem.ShutDown();
        m_PanelManager.StartLocation("MainMenu");
    }

    public void OnEnable()
    {
        m_Player.LoadImprove();
    }

    public void EnemyGenerate(string p_RoomId)
    {
        if (m_EnemyGeneratorSystem != null)
        {
            m_EnemyGeneratorSystem.Generate(p_RoomId);
        }
    }

    public void RunPauseMenu()
    {
        PauseMenuPanel l_PauseMenuPanel = Instantiate(PauseMenuPanel.prefab);
        ShowPanel(l_PauseMenuPanel);

        AudioSystem.GetInstance().ChangeThemeVolume(0.1f);

        SetControl(ControlType.Panel);
    }

    public void OpenInventory()
    {
        SetControl(ControlType.Panel);
        InventoryPanel lInventoryPanel = Instantiate(InventoryPanel.prefab);
        ShowPanel(lInventoryPanel);
    }

    public void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }

    public void AddOnPauseListener(UnityAction p_Action)
    {
        m_OnPauseEvent.AddListener(p_Action);
    }

    public void RemoveOnPauseListener(UnityAction p_Action)
    {
        m_OnPauseEvent.RemoveListener(p_Action);
    }

    public void AddOnResumeListener(UnityAction p_Action)
    {
        m_OnResumeEvent.AddListener(p_Action);
    }

    public void RemoveOnResumeListener(UnityAction p_Action)
    {
        m_OnResumeEvent.RemoveListener(p_Action);
    }

    public void Pause()
    {
        m_IsPause = true;
        m_OnPauseEvent.Invoke();
    }

    public void Resume()
    {
        m_OnResumeEvent.Invoke();
        m_IsPause = false;
    }
}
