using UnityEngine;

public class BattleSystem : Singleton<BattleSystem>
{
    public enum ActorID
    {
        Player,
        Enemy
    }

    private Player m_Player;
    private Enemy  m_Enemy;

    public BattleSystem()
    {
        m_Player = Player.GetInstance();
        m_Enemy = Enemy.GetInstance();
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
        PanelManager.GetInstance().GetComponent<CanvasGroup>().interactable = false;
        //PanelManager.GetInstance().Show(PanelEnum.WinPanel);
    }

    private void Lose()
    {
        PanelManager.GetInstance().GetComponent<CanvasGroup>().interactable = false;
        //PanelManager.GetInstance().Show(PanelEnum.LosePanel);
    }
}
