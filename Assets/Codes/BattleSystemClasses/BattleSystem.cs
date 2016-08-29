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

    private static BattleSystem m_Instance;
    private Player m_Player;
    private List<Enemy>  m_EnemyList = new List<Enemy>();
    private bool m_IsPlayerTurn = true;
    private int m_CurrentEnemyNumber = 0;
    private bool m_FromMap = false;

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

    public void Awake()
    {
        m_Instance = this;

        m_Player = Player.GetInstance();

        List<string> l_EnemyIds = BattleStarter.GetInstance().GetEnemy();

        if (l_EnemyIds.Count == 0)
        {
            Enemy l_NewEnemy = Instantiate(Enemy.prefab);
            l_NewEnemy.SetData(EnemyDataBase.GetInstance().GetEnemy("Skwatwolf"));
            l_NewEnemy.transform.SetParent(m_EnemyTransform);
            l_NewEnemy.transform.localPosition = Vector3.zero;
            m_EnemyList.Add(l_NewEnemy);
        }
        else
        {
            m_FromMap = true;
            for (int i = 0; i < l_EnemyIds.Count; i++)
            {
                Enemy l_NewEnemy = Instantiate(Enemy.prefab);
                l_NewEnemy.SetData(EnemyDataBase.GetInstance().GetEnemy(l_EnemyIds[i]));
                l_NewEnemy.transform.SetParent(m_EnemyTransform);
                l_NewEnemy.transform.localPosition = Vector3.zero;
                m_EnemyList.Add(l_NewEnemy);
            }
        }
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
            Player.GetInstance().RunTurn();
        }
        else
        {
            SetVisibleAvatarPanel(false);

            Enemy l_NextEnemy = GetNextEnemy();
            l_NextEnemy.RunTurn();
            //  Запуск ИИ
            if (!l_NextEnemy.isDead)
            {
                Player.GetInstance().RunTurn();
                l_NextEnemy.Run();
            }
        }
    }

    public void SetVisibleAvatarPanel(bool p_Value)
    {
        m_AvatarPanel.SetActive(p_Value);
    }

    public List<Enemy> GetEnemyList()
    {
        return m_EnemyList;
    }

    private void Win()
    {
        SetVisibleAvatarPanel(false);
        TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { "Враги убиты.\nВы победили.\nГГ получает десять очков опыта и семки, шерсть 5 золотых, которые вы не увидите. Даже я сам не знаю для чего они. Кароч маладец. На шоколадка, воон лежит обернись... Шучу :)" });
        l_TextPanel.AddButtonAction(RestartGame);
        PanelManager.GetInstance().ShowPanel(l_TextPanel);
    }

    private void Lose()
    {
        SetVisibleAvatarPanel(false);
        TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { "У ГГ больше нету сил сражаться далее, ГГ потерял сознание.\nНе осталось никого кто мог бы продолжить сражение...\nВы проиграли." });
        l_TextPanel.AddButtonAction(RestartGame);
        PanelManager.GetInstance().ShowPanel(l_TextPanel);
    }

    private void RestartGame()
    {
        if (m_FromMap)
        {
            SceneManager.LoadScene("Town");
        }
        else
        {
            SceneManager.LoadScene("BattleSystem");
        }
    }

    private void InitStartPanel()
    {
        MainPanel l_MainPanel = Instantiate(MainPanel.prefab);
        PanelManager.GetInstance().ShowPanel(l_MainPanel);
    }

    private Enemy GetNextEnemy()
    {
        if (m_CurrentEnemyNumber >= m_EnemyList.Count)
        {
            m_CurrentEnemyNumber = 0;
        }
        return m_EnemyList[m_CurrentEnemyNumber];
    }
}
