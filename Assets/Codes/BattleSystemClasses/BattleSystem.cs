using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
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
    private List<BattleEnemy>  m_EnemyList = new List<BattleEnemy>();
    private bool m_IsPlayerTurn = true;
    private int m_CurrentEnemyNumber = 0;
    private BattleData m_BattleData;
    private int m_Experience = 0;
    private string m_LevelupNotificationText = string.Empty;
    private string m_ClassupNotificationText = string.Empty;
    private bool m_IsClassup = false;

    [SerializeField]
    private Transform m_MainPanelTransform = null;

    [SerializeField]
    private GameObject m_AvatarPanel = null;

    [SerializeField]
    private Transform m_EnemyTransform = null;

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

    public void Awake()
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

        InitBattle();
    }

    public void Start()
    {
        InitStartPanel();
    }

    public void Died(ActorID p_ActorID)
    {
        switch (p_ActorID)
        {
            case ActorID.Player:
                Lose();
                break;
            case ActorID.Enemy:
                Win();
                break;
        }
    }

    public void EndTurn()
    {
        m_IsPlayerTurn = !m_IsPlayerTurn;

        if (m_IsPlayerTurn)
        {
            SetVisibleAvatarPanel(true);
            BattlePlayer.GetInstance().RunTurn();
        }
        else
        {
            SetVisibleAvatarPanel(false);

            BattleEnemy l_NextEnemy = GetNextEnemy();
            l_NextEnemy.RunTurn();
            //  Запуск ИИ
            if (!l_NextEnemy.isDead)
            {
                BattlePlayer.GetInstance().RunTurn();
                l_NextEnemy.Run();
            }
        }
    }

    public void SetVisibleAvatarPanel(bool p_Value)
    {
        m_AvatarPanel.SetActive(p_Value);
    }

    public List<BattleEnemy> GetEnemyList()
    {
        return m_EnemyList;
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
    #endregion

    #region Private
    private void Win()
    {
        SetVisibleAvatarPanel(false);

        List<string> l_WinText = new List<string>();
        l_WinText.Add("Враги убиты.\nВы победили.\nГГ получает " + m_Experience + " exp");
        if (m_LevelupNotificationText != string.Empty)
        {
            l_WinText.Add(m_LevelupNotificationText);
        }
        if (m_ClassupNotificationText != string.Empty)
        {
            l_WinText.Add(m_ClassupNotificationText);
        }

        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(l_WinText);
        l_TextPanel.AddButtonAction(ReturnToJourney);
        
        ShowPanel(l_TextPanel);
        BattleStarter.GetInstance().BattleWin();
    }

    private void Lose()
    {
        SetVisibleAvatarPanel(false);
        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { "У ГГ больше нету сил сражаться далее, ГГ потерял сознание.\nНе осталось никого кто мог бы продолжить сражение...\nВы проиграли." });
        l_TextPanel.AddButtonAction(ReturnToMainMenu);

        ShowPanel(l_TextPanel);
    }

    private void ReturnToJourney()
    {
        if (m_BattleData.id == "TestBattle")
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
        if (m_BattleData.id == "TestBattle")
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

    private BattleEnemy GetNextEnemy()
    {
        if (m_CurrentEnemyNumber >= m_EnemyList.Count)
        {
            m_CurrentEnemyNumber = 0;
        }
        return m_EnemyList[m_CurrentEnemyNumber];
    }

    private void InitBattle()
    {
        m_BattleData = BattleStarter.GetInstance().GetBattle();
        if (m_BattleData.id == null)
        {
            BattleStarter.GetInstance().InitBattle(null, "TestBattle");
            m_BattleData = BattleStarter.GetInstance().GetBattle();
        }

        m_LocationBackground.sprite = Resources.Load<Sprite>("Sprites/BattleSystem/Background/" + m_BattleData.locationBackground);

        for (int i = 0; i < m_BattleData.enemyList.Count; i++)
        {
            BattleEnemy l_NewEnemy = Instantiate(BattleEnemy.prefab);
            l_NewEnemy.SetData(EnemyDataBase.GetInstance().GetEnemy(m_BattleData.enemyList[i]));
            l_NewEnemy.transform.SetParent(m_EnemyTransform);
            l_NewEnemy.transform.localPosition = Vector3.zero;
            m_EnemyList.Add(l_NewEnemy);
        }

        if (m_BattleData.playerSettings != null)
        {
            foreach (string l_Key in m_BattleData.playerSettings.Keys)
            {
                switch (l_Key)
                {
                    case "HP":
                        PlayerData.GetInstance().health = m_BattleData.playerSettings[l_Key];
                        break;
                }
            }
        }
    }

    private void LevelupNotification()
    {
        string l_LevelText = (PlayerData.GetInstance().GetLevel() + 1).ToString();
        m_LevelupNotificationText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Levelup", l_LevelText);
    }

    private void ClassupNotification()
    {
        m_IsClassup = true;
        m_ClassupNotificationText = PlayerData.GetInstance().GetPlayerName() + " " + LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Classup");
    }
    #endregion
}
