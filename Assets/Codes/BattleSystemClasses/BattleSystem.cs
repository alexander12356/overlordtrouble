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
    private Enemy  m_Enemy;
    private bool m_IsPlayerTurn = true;

    [SerializeField]
    private Transform m_MainPanelTransform;

    [SerializeField]
    private GameObject m_AvatarPanel = null;

    public Transform mainPanelTransform
    {
        get { return m_MainPanelTransform; }
    }

    public static BattleSystem GetInstance()
    {
        return m_Instance;
    }

    public void Awake()
    {
        m_Instance = this;

        m_Player = Player.GetInstance();
        m_Enemy = Enemy.GetInstance();
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
            Enemy.GetInstance().RunTurn();
            //  Запуск ИИ
            if (!Enemy.GetInstance().isDead)
            {
                Player.GetInstance().RunTurn();
                Enemy.GetInstance().Run();
            }
        }
    }

    public void SetVisibleAvatarPanel(bool p_Value)
    {
        m_AvatarPanel.SetActive(p_Value);
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
        SceneManager.LoadScene("BattleSystem");
    }

    private void InitStartPanel()
    {
        MainPanel l_MainPanel = Instantiate(MainPanel.prefab);
        PanelManager.GetInstance().ShowPanel(l_MainPanel);
    }
}
