using UnityEngine;

using System.Collections.Generic;
using BattleSystemClasses.Bosses.Leshii;

public class BattleSystemBoss : BattleSystem
{
    private bool m_IsPlayerTurn = true;

    [SerializeField]
    protected Leshii m_Leshii = null;

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
        m_Leshii.InitStats();

        InitLeshiiOrgans();

        TurnSystem.GetInstance().AddEnemy(m_Leshii);
    }

    public void InitLeshiiOrgans()
    {
        m_EnemyList = new List<BattleEnemy>();
        List<LeshiiOrgan> l_LeshiiOrgan = m_Leshii.GetOrgans();
        for (int i = 0; i < l_LeshiiOrgan.Count; i++)
        {
            m_EnemyList.Add(l_LeshiiOrgan[i]);
        }
    }

    public override void Retreat()
    {
        string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:CannotRun");

        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.AddButtonAction(l_TextPanel.Close);
        l_TextPanel.SetText(new List<string>() { l_Text });

        ShowPanel(l_TextPanel);
    }

    public virtual void RunBossIntro()
    {
        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetTalkingAnimator(m_Leshii.headAnimator, "Talking");

        string l_Text = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:Intro");
        l_TextPanel.SetText(new List<string>() { l_Text });

        l_TextPanel.AddButtonAction(l_TextPanel.Close);
        l_TextPanel.AddButtonAction(StartGame);
        ShowPanel(l_TextPanel);

        SetVisibleAvatarPanel(false);
    }

    private void StartGame()
    {
        TurnSystem.GetInstance().NextActorRun();
    }
}