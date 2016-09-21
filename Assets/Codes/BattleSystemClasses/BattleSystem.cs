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

    private static BattleSystem m_Instance;
    private BattlePlayer m_Player;
    private List<BattleEnemy>  m_EnemyList = new List<BattleEnemy>();
    private bool m_IsPlayerTurn = true;
    private int m_CurrentEnemyNumber = 0;
    private BattleData m_BattleData;

    [SerializeField]
    private Transform m_MainPanelTransform = null;

    [SerializeField]
    private GameObject m_AvatarPanel = null;

    [SerializeField]
    private Transform m_EnemyTransform = null;

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

        DataLoader.GetInstance();

        m_Player = BattlePlayer.GetInstance();

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
        ReturnToJourney();
    }

    private void Win()
    {
        SetVisibleAvatarPanel(false);
        TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { "Враги убиты.\nВы победили.\nГГ получает десять очков опыта и семки, шерсть 5 золотых, которые вы не увидите. Даже я сам не знаю для чего они. Кароч маладец. На шоколадка, воон лежит обернись... Шучу :)" });
        l_TextPanel.AddButtonAction(ReturnToJourney);
        PanelManager.GetInstance().ShowPanel(l_TextPanel);
    }

    private void Lose()
    {
        SetVisibleAvatarPanel(false);
        TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { "У ГГ больше нету сил сражаться далее, ГГ потерял сознание.\nНе осталось никого кто мог бы продолжить сражение...\nВы проиграли." });
        l_TextPanel.AddButtonAction(ReturnToMainMenu);

        PlayerData.GetInstance().health = 20;

        PanelManager.GetInstance().ShowPanel(l_TextPanel);
    }

    private void ReturnToJourney()
    {
        if (m_BattleData.id == "TestBattle")
        {
            PanelManager.GetInstance().ChangeScene("DemoMainScene");
        }
        else
        {
            PanelManager.GetInstance().ChangeScene("Town");
        }
    }

    private void ReturnToMainMenu()
    {
        if (m_BattleData.id == "TestBattle")
        {
            PanelManager.GetInstance().ChangeScene("BattleSystem");
        }
        else
        {
            PanelManager.GetInstance().ChangeScene("DemoMainScene");
        }
    }

    private void InitStartPanel()
    {
        MainPanel l_MainPanel = Instantiate(MainPanel.prefab);
        PanelManager.GetInstance().ShowPanel(l_MainPanel);
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
            m_BattleData = BattleDataBase.GetInstance().GetBattle("TestBattle");
        }

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
}
