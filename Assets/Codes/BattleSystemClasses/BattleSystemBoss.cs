using UnityEngine;

using System.Collections.Generic;
using BattleSystemClasses.Bosses.Leshii;

public class BattleSystemBoss : BattleSystem
{
    [SerializeField]
    private Leshii m_Leshii = null;

    public override void Awake()
    {
        base.Awake();

        InitBattle();
    }

    public override void InitBattle()
    {
        base.InitBattle();

        // Инициализация фона
        
        m_Leshii.Init();

        List<LeshiiOrgan> l_LeshiiOrgan = m_Leshii.GetOrgans();
        m_EnemyList = new List<BattleEnemy>();
        for (int i = 0; i < l_LeshiiOrgan.Count; i++)
        {
            m_EnemyList.Add(l_LeshiiOrgan[i]);
        }


    }

    public override void EndTurn()
    {
        base.EndTurn();

        //if (m_EnemyList.Count == 0)
        //{
        //    Win();
        //    return;
        //}

        //if (m_IsLose)
        //{
        //    return;
        //}

        SetVisibleAvatarPanel(true);
        BattlePlayer.GetInstance().RunTurn();
    }

    private void RunBossDialog()
    {
        
    }
}