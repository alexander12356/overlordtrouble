using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private static TurnSystem m_Instance = null;
    private List<BattleActor> m_ActorList = new List<BattleActor>();
    private int m_CurrentTurn = 0;
    private int m_CurrentActor = 0;
    private bool m_EndTurn = false;

    public int currentTurn
    {
        get { return m_CurrentTurn; }
    }

    public void Awake()
    {
        m_Instance = this;
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

    public void RunGame()
    {
        m_ActorList.Insert(0, BattlePlayer.GetInstance());
        StartCoroutine(Running());
    }

    private IEnumerator Running()
    {
        while (true)
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

            while (!m_EndTurn)
            {
                yield return null;
            }
            m_EndTurn = false;
        }
    }

    public void NextActorRun()
    {
        if (BattleSystem.GetInstance().CheckWin() || BattleSystem.GetInstance().IsLose())
        {
            return;
        }
        m_EndTurn = true;
    }

    private void NextTurn()
    {
        m_CurrentTurn++;
        EffectSystem.GetInstance().CheckEffects();
        ResultSystem.GetInstance().ShowResult();
    }
}