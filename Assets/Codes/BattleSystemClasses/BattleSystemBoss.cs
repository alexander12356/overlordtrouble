﻿using UnityEngine;

using System.Collections.Generic;
using BattleSystemClasses.Bosses.Leshii;

public class BattleSystemBoss : BattleSystem
{
    private bool m_IsPlayerTurn = true;

    [SerializeField]
    private Leshii m_Leshii = null;

    public override void Awake()
    {
        base.Awake();

        InitBattle();
    }

    public override void Start()
    {
        base.Start();

        RunBossIntro();
    }

    public override void InitBattle()
    {
        base.InitBattle();

        m_BattleData = BattleStarter.GetInstance().GetBattle();
        if (m_BattleData.id == null)
        {
            BattleStarter.GetInstance().InitBattle(null, "TestBattleBossLeshii");
            m_BattleData = BattleStarter.GetInstance().GetBattle();
        }

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

        //if (m_IsPlayerTurn)
        //{
            SetVisibleAvatarPanel(true);
            BattlePlayer.GetInstance().RunTurn();
            m_IsPlayerTurn = false;
        //}
        //else
        //{
        //    SetVisibleAvatarPanel(false);
        //    m_Leshii.Run();
        //    m_IsPlayerTurn = true;
        //}

        //if (m_EnemyList.Count == 0)
        //{
        //    Win();
        //    return;
        //}

        //if (m_IsLose)
        //{
        //    return;
        //}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_Leshii.bodyAnimator.SetTrigger("Attack");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_Leshii.bodyAnimator.SetTrigger("StartCharge");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_Leshii.bodyAnimator.SetTrigger("AttackCharge");
        }
    }

    private void RunBossIntro()
    {
        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { "Тебе меня не победить!" });
        l_TextPanel.SetTalkingAnimator(m_Leshii.headAnimator, "Talking");
        
        l_TextPanel.AddButtonAction(l_TextPanel.Close);
        l_TextPanel.AddButtonAction(EndTurn);
        ShowPanel(l_TextPanel);

        SetVisibleAvatarPanel(false);
    }
}