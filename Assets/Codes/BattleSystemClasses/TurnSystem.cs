using System.Collections.Generic;

using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private static TurnSystem m_Instance = null;
    private List<BattleActor> m_ActorList = new List<BattleActor>();
    private int m_CurrentTurn = 0;
    private int m_CurrentActor = 0;

    public int currentTurn
    {
        get { return m_CurrentTurn; }
    }

    public void Awake()
    {
        m_Instance = this;
    }

    public void Start()
    {
        m_ActorList.Insert(0, BattlePlayer.GetInstance());
    }

    public static TurnSystem GetInstance()
    {
        return m_Instance;
    }

    public void AddEnemy(BattleEnemy p_Enemy)
    {
        m_ActorList.Add(p_Enemy);
    }

    public void RemoveEnemy(BattleEnemy p_Enemy)
    {
        m_ActorList.Remove(p_Enemy);
    }

    public void EnemyRunned(BattleEnemy p_Enemy)
    {
        RemoveEnemy(p_Enemy);
        m_CurrentActor--;
    }

    private void RunTurn()
    {
        if (m_CurrentActor >= m_ActorList.Count)
        {
            m_CurrentActor = 0;
            NextTurn();
        }
        else
        {
            m_ActorList[m_CurrentActor].RunTurn();

            if (m_CurrentActor == 0)
            {
                BattleSystem.GetInstance().SetVisibleAvatarPanel(true);
                BattleSystem.GetInstance().ShowMainPanel();
            }
            else
            {
                BattleSystem.GetInstance().SetVisibleAvatarPanel(false);
            }

            m_CurrentActor++;
        }
    }

    public void NextActorRun()
    {
        RunTurn();
    }

    private void NextTurn()
    {
        if (BattleSystem.GetInstance().CheckWin() || BattleSystem.GetInstance().IsLose())
        {
            return;
        }

        m_CurrentTurn++;
        EffectSystem.GetInstance().CheckEffects();
        ResultSystem.GetInstance().ShowResult();
        BattlePlayer.GetInstance().RestoreSpecialPoints();
    }
}