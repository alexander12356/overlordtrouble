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
    private MainPanel m_MainPanel = null;
    private Dictionary<string, int> m_LootList = new Dictionary<string, int>();

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
    public static bool IsInstance()
    {
        return m_Instance == null ? false : true;
    }
    public Transform mainPanelTransform
    {
        get { return m_MainPanelTransform; }
    }
    public BattleData battleData
    {
        get { return m_BattleData; }
    }
    public MainPanel mainPanel
    {
        get { return m_MainPanel; }
    }

    public virtual void Awake()
    {
        m_Instance = this;

        if (GameManager.IsInstance() == false)
        {
            GameManager.GetInstance();
            PlayerData.GetInstance().InitTestStats();
        }
    }

    public virtual void Start()
    {
        m_Player = BattlePlayer.GetInstance();
        PlayerData.GetInstance().AddLevelupNotification(LevelupNotification);
        PlayerData.GetInstance().AddClassupNotification(ClassupNotification);

        AudioSystem.GetInstance().PlayMusic("Battle");
    }

    public void SetVisibleAvatarPanel(bool p_Value)
    {
        //m_AvatarPanel.SetActive(p_Value);
    }

    public List<BattleEnemy> GetEnemyList()
    {
        return m_EnemyList;
    }

    public void EnemyDied(BattleEnemy p_BattleEnemy)
    {
        m_EnemyList.Remove(p_BattleEnemy);
        TurnSystem.GetInstance().RemoveEnemy(p_BattleEnemy);
    }

    public void EnemyRun(BattleEnemy p_BattleEnemy)
    {
        m_EnemyList.Remove(p_BattleEnemy);
        TurnSystem.GetInstance().EnemyRunned(p_BattleEnemy);
    }

    public void PlayerDied()
    {
        Lose();
    }

    public virtual void Retreat()
    {
        if (m_BattleData.isEvent)
        {
            string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:CannotRun");

            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
            l_TextPanel.AddButtonAction(l_TextPanel.Close);
            l_TextPanel.SetText(new List<string>() { l_Text });

            ShowPanel(l_TextPanel);
            return;
        }

        System.Random l_SystemRandom = new System.Random();
        int l_RetreatChance = l_SystemRandom.Next(0, 100);

        int l_PlayerSpeed = PlayerData.GetInstance().GetStatValue("Speed");
        int l_EnemySpeed = GetEnemyMaxSpeed();
        int l_EnemyLevel = GetEnemyMaxLevel();

        int l_DeltaSpeed = l_PlayerSpeed - l_EnemySpeed;

        if (l_DeltaSpeed > 0)
        {
            l_RetreatChance += l_DeltaSpeed * 5;
        }

        if ((m_Player.level - l_EnemyLevel) >= 10)
        {
            l_RetreatChance = 100;
        }

        if (l_RetreatChance >= 50)
        {
            BattleStarter.GetInstance().BattleRetreat();
            ReturnToJourney();
        }
        else
        {
            string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:RunFail");

            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
            l_TextPanel.AddButtonAction(l_TextPanel.Close);
            l_TextPanel.SetText(new List<string>() { l_Text });

            BattleShowPanelStep l_ShowText = new BattleShowPanelStep(l_TextPanel);

            ResultSystem.GetInstance().AddStep(l_ShowText);
            ResultSystem.GetInstance().ShowResult();
        }
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
    }

    public void Win()
    {
        SetVisibleAvatarPanel(false);

        PlayerData.GetInstance().AddExperience(m_Experience);

        List<string> l_WinText = new List<string>();

        string l_RewardText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Win", new string[] { BattlePlayer.GetInstance().actorName, m_Experience.ToString() });
        l_RewardText += GiveLoot();

        l_WinText.Add(l_RewardText);

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

    public void ShowMainPanel()
    {
        InitStartPanel();
    }

    public void CloseMainPanel()
    {
        if (m_MainPanel && !m_MainPanel.moving)
        {
            m_MainPanel.Close();
        }
    }

    public virtual void Lose()
    {
        m_IsLose = true;
        SetVisibleAvatarPanel(false);

        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Lose", new string[] { BattlePlayer.GetInstance().actorName, BattlePlayer.GetInstance().actorName } ) });
        l_TextPanel.AddButtonAction(ReturnToMainMenu);

        BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_Step);
    }

    public void AddLoot(EnemyLootData p_LootData)
    {
        System.Random l_Random = new System.Random();
        int l_Chance = l_Random.Next(0, 100);

        if (p_LootData.chance < l_Chance)
        {
            return;
        }

        int p_Count = l_Random.Next(p_LootData.count[0], p_LootData.count[1]);
        string p_Id = p_LootData.id;

        if (p_Id != "Monett")
        {
            PlayerInventory.GetInstance().AddItem(p_Id, p_Count);
        }
        else
        {
            PlayerInventory.GetInstance().coins += p_Count;
        }

        if (!m_LootList.ContainsKey(p_Id))
        {
            m_LootList.Add(p_Id, p_Count);
        }
        else
        {
            m_LootList[p_Id] += p_Count;
        }
    }

    public string GiveLoot()
    {
        string l_Text = string.Empty;
        int l_MonettCount = 0;

        if (m_LootList.ContainsKey("Monett"))
        {
            l_MonettCount = m_LootList["Monett"];
            m_LootList.Remove("Monett");
        }

        string l_ItemsText = "";
        int l_Index = 0;
        foreach (string l_ItemId in m_LootList.Keys)
        {
            string l_ItemName = LocalizationDataBase.GetInstance().GetText("Item:" + l_ItemId);
            int l_ItemCount = m_LootList[l_ItemId];

            if (l_ItemCount > 1)
            {
                l_ItemName += " x" + l_ItemCount;
            }

            if ((l_Index == m_LootList.Count - 2 && l_MonettCount == 0) || (m_LootList.Count == 1 && l_MonettCount == 0))
            {
                l_ItemsText += " " + LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:And") + " ";
            }
            else if (l_Index == m_LootList.Count - 1 && l_MonettCount == 0)
            {
                l_ItemsText += "";
            }
            else
            {
                l_ItemsText += ", ";
            }

            l_ItemsText += l_ItemName;

            l_Index++;
        }

        if (l_MonettCount > 0)
        {
            l_ItemsText += " " + LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:And") + " " + l_MonettCount + " " + LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Monet");
        }

        return l_ItemsText;
    }
    #endregion

    #region Private
    protected void ReturnToJourney()
    {
        AudioSystem.GetInstance().StopMusic("Battle");
        AudioSystem.GetInstance().PlayTheme();
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
        //TODO создать единый выгружатель при смерти или при выходе из локаций
        SaveSystem.ShutDown();
        PlayerPrefs.DeleteAll();
        AudioSystem.GetInstance().StopMusic("Battle");
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
        m_MainPanel = Instantiate(MainPanel.prefab);
        ShowPanel(m_MainPanel);
    }

    private void LevelupNotification()
    {
        m_IsLevelup = true;
    }

    private void ClassupNotification()
    {
        m_IsClassup = true;
    }

    private int GetEnemyMaxSpeed()
    {
        int l_EnemySpeed = (int)m_EnemyList[0].speedStat;

        for (int i = 0; i < m_EnemyList.Count; i++)
        {
            if (l_EnemySpeed < m_EnemyList[i].speedStat)
            {
                l_EnemySpeed = (int)m_EnemyList[i].speedStat;
            }
        }

        return l_EnemySpeed;
    }

    private int GetEnemyMaxLevel()
    {
        int l_EnemyLevel = m_EnemyList[0].level;

        for (int i = 0; i < m_EnemyList.Count; i++)
        {
            if (l_EnemyLevel < m_EnemyList[i].level)
            {
                l_EnemyLevel = m_EnemyList[i].level;
            }
        }

        return l_EnemyLevel;
    }
    #endregion
}
