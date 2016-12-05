using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

public class BattleSystem : MonoBehaviour
{
    public enum ActorID
    {
        Player,
        Enemy
    }

    #region Variables
    private static BattleSystem m_Instance;
    private BattlePlayer m_Player;
    private int m_Experience = 0;
    private bool m_IsClassup = false;
    private bool m_IsLevelup = false;

    protected List<BattleEnemy> m_EnemyList = new List<BattleEnemy>();
    protected int m_CurrentTurn = -1;
    protected bool m_IsLose = false;
    protected BattleData m_BattleData;

    [SerializeField]
    private Transform m_MainPanelTransform = null;

    [SerializeField]
    private GameObject m_AvatarPanel = null;

    [SerializeField]
    private PanelManager m_PanelManager = null;

    [SerializeField]
    private SpriteRenderer m_LocationBackground = null;
    #endregion

    #region Interface
    public static BattleSystem GetInstance()
    {
        return m_Instance;
    }
    public Transform mainPanelTransform
    {
        get { return m_MainPanelTransform; }
    }
    public BattleData battleData
    {
        get { return m_BattleData; }
    }

    public virtual void Awake()
    {
        m_Instance = this;
        m_Player = BattlePlayer.GetInstance();
        PlayerData.GetInstance().AddLevelupNotification(LevelupNotification);
        PlayerData.GetInstance().AddClassupNotification(ClassupNotification);

        if (GameManager.IsInstance() == false)
        {
            GameManager.GetInstance();
            PlayerData.GetInstance().InitTestStats();
        }
    }

    public virtual void Start()
    {
        InitStartPanel();
    }

    public void SetVisibleAvatarPanel(bool p_Value)
    {
        m_AvatarPanel.SetActive(p_Value);
    }

    public List<BattleEnemy> GetEnemyList()
    {
        return m_EnemyList;
    }

    public void EnemyDied(BattleEnemy p_BattleEnemy)
    {
        m_EnemyList.Remove(p_BattleEnemy);
    }

    public void PlayerDied()
    {
        Lose();
    }

    public void Retreat()
    {
        BattleStarter.GetInstance().BattleRetreat();
        ReturnToJourney();
    }

    public void ShowPanel(Panel p_Panel, bool p_WithOverlay = false, Transform p_Parent = null)
    {
        m_PanelManager.ShowPanel(p_Panel, p_WithOverlay, p_Parent);
    }

    public void StartLocation(string p_LocationId)
    {
        m_PanelManager.StartLocation(p_LocationId);
    }

    public string GetBattleId()
    {
        return m_BattleData.id;
    }

    public void AddExperience(int p_Experience)
    {
        m_Experience += p_Experience;
        PlayerData.GetInstance().AddExperience(m_Experience);
    }

    public void Win()
    {
        SetVisibleAvatarPanel(false);

        List<string> l_WinText = new List<string>();
        l_WinText.Add(LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Win", new string[] { m_Experience.ToString() }));
        if (m_IsLevelup)
        {
            string l_LevelText = (PlayerData.GetInstance().GetLevel() + 1).ToString();
            string l_LevelupNotificationText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Levelup", new string[] { l_LevelText });
            l_WinText.Add(l_LevelupNotificationText);
        }
        if (m_IsClassup)
        {
            string l_ClassupNotificationText = PlayerData.GetInstance().GetPlayerName() + " " + LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Classup");
            l_WinText.Add(l_ClassupNotificationText);
        }

        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(l_WinText);
        l_TextPanel.AddButtonAction(ReturnToJourney);

        ShowPanel(l_TextPanel);
        BattleStarter.GetInstance().BattleWin();
    }

    public virtual void InitBattle()
    {
    }

    public void InitLocationBackground(string p_LocationId)
    {
        m_LocationBackground.sprite = Resources.Load<Sprite>("Sprites/BattleSystem/Background/" + p_LocationId);
    }

    public bool CheckWin()
    {
        if (m_EnemyList.Count == 0)
        {
            Win();
            return true;
        }
        return false;
    }

    public bool IsLose()
    {
        return m_IsLose;
    }
    #endregion

    #region Private
    private void Lose()
    {
        m_IsLose = true;
        SetVisibleAvatarPanel(false);

        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Lose") });
        l_TextPanel.AddButtonAction(ReturnToMainMenu);

        ShowPanel(l_TextPanel);
    }

    private void ReturnToJourney()
    {
        if (m_BattleData.id.Contains("TestBattle"))
        {
            SceneManager.LoadScene("DemoMainScene");
            return;
        }

        if (m_IsClassup)
        {
            m_PanelManager.UnloadAndLoadScene("Improve");
            return;
        }

        m_PanelManager.UnloadScene();
    }

    private void ReturnToMainMenu()
    {
        if (m_BattleData.id.Contains("TestBattle"))
        {
            SceneManager.LoadScene("DemoMainScene");
        }
        else
        {
            StartLocation("MainMenu");
        }
    }

    private void InitStartPanel()
    {
        MainPanel l_MainPanel = Instantiate(MainPanel.prefab);
        ShowPanel(l_MainPanel);
    }

    private void LevelupNotification()
    {
        m_IsLevelup = true;
    }

    private void ClassupNotification()
    {
        m_IsClassup = true;
    }
    #endregion
}
