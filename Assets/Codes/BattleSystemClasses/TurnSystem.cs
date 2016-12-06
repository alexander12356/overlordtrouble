using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private static TurnSystem m_Instance = null;
    private List<BattleEnemy> m_EnemyList = new List<BattleEnemy>();
    private int m_CurrentTurn = -2;
    private bool m_EndTurn = false;

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
        m_EnemyList.Add(p_Enemy);
    }

    public void RemoveEnemy(BattleEnemy p_Enemy)
    {
        m_EnemyList.Remove(p_Enemy);
    }

    public void RunGame()
    {
        StartCoroutine(Running());
    }

    private IEnumerator Running()
    {
        while (true)
        {
            m_CurrentTurn++;
            if (m_CurrentTurn >= m_EnemyList.Count)
            {
                m_CurrentTurn = -1;
            }

            if (m_CurrentTurn == -1)
            {
                BattlePlayer.GetInstance().RunTurn();
                BattleSystem.GetInstance().SetVisibleAvatarPanel(true);
                BattleSystem.GetInstance().ShowMainPanel();
            }
            else
            {
                m_EnemyList[m_CurrentTurn].RunTurn();
                BattleSystem.GetInstance().SetVisibleAvatarPanel(false);
            }

            while (!m_EndTurn)
            {
                yield return null;
            }
            m_EndTurn = false;
        }
    }

    public void EndTurn()
    {
        if (BattleSystem.GetInstance().CheckWin() || BattleSystem.GetInstance().IsLose())
        {
            return;
        }
        m_EndTurn = true;
    }
}