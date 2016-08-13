using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Win()
    {
        TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText("Враги убиты.\nВы победили.\nГГ получает десять очков опыта и семки, шерсть 5 золотых, которые вы не увидите. Даже я сам не знаю для чего они. Кароч маладец. На шоколадка, воон лежит обернись... Шучу :)");
        l_TextPanel.AddButtonAction(RestartGame);
        PanelManager.GetInstance().ShowPanel(l_TextPanel);
    }

    private void Lose()
    {
        TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText("У ГГ больше нету сил сражаться далее, ГГ потерял сознание.\nНе осталось никого кто мог бы продолжить сражение...\nВы проиграли.");
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
