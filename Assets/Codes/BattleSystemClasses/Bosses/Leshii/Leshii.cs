﻿using UnityEngine;
using System.Collections.Generic;

namespace BattleSystemClasses.Bosses.Leshii
{
    public enum OrganIds
    {
        NONE = -1,
        RightHand,
        LeftHand,
        Body,
        Headmain
    }

    public class Leshii : BattleEnemy
    {
        private Animator m_BodyAnimator = null;
        private Animator m_HeadAnimator = null;
        private Vector2 m_HandsLive = Vector2.zero;
        private int m_SummonHandsCounter = 0;
        private int m_SummonHandsCount = 2;
        private int m_ChargeCounter = 0;
        private int m_ChargeCount = 3;
        private bool m_ChargeMode = false;
        private bool m_IsHealCast = false;
        private EndEffectChecker m_EndEffectChecker = null;

        [SerializeField]
        private LeshiiOrgan m_RightHand = null;

        [SerializeField]
        private LeshiiOrgan m_LeftHand = null;

        [SerializeField]
        private LeshiiOrgan m_Body = null;

        public Animator bodyAnimator
        {
            get { return m_BodyAnimator; }
        }
        public Animator headAnimator
        {
            get { return m_HeadAnimator; }
        }
        public bool isChargeMode
        {
            get { return m_ChargeMode; }
        }

        public override void Awake()
        {
            base.Awake();

            m_EndEffectChecker = GetComponent<EndEffectChecker>();
        }

        public override void InitStats()
        {
            actorName = LocalizationDataBase.GetInstance().GetText("Boss:Leshii");

            m_RightHand.Init(OrganIds.RightHand, this);
            m_LeftHand.Init(OrganIds.LeftHand, this);
            m_Body.Init(OrganIds.Body, this);
            m_Body.baseHealth = m_Body.health = 50;

            m_BodyAnimator = GetComponent<Animator>();
            m_HeadAnimator = transform.FindChild(OrganIds.Headmain.ToString()).GetComponentInChildren<Animator>();

            CalculateIdle(m_HandsLive);
        }
        
        public List<LeshiiOrgan> GetOrgans()
        {
            List<LeshiiOrgan> l_LeshiiOrgans = new List<LeshiiOrgan>() { m_RightHand, m_Body, m_LeftHand };
            return l_LeshiiOrgans;
        }

        public override void RunTurn()
        {
            if (IsAllHandsDied())
            {
                if (m_SummonHandsCounter < m_SummonHandsCount)
                {
                    List<string> l_Text = new List<string>();
                    string l_SummonHandsText = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:SummonHands", new string[] { (m_SummonHandsCount - m_SummonHandsCounter).ToString() });
                    l_Text.Add(l_SummonHandsText);

                    TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
                    l_TextPanel.SetTalkingAnimator(m_HeadAnimator, "Talking");
                    l_TextPanel.SetText(l_Text);
                    l_TextPanel.AddButtonAction(l_TextPanel.Close);

                    BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
                    ResultSystem.GetInstance().AddStep(l_Step);

                    m_SummonHandsCounter++;
                }
                else
                {
                    SummonHands();
                }

                ResultSystem.GetInstance().ShowResult();
                return;
            }

            if (m_Body.health > 40)
            {
                m_ChargeMode = false;

                Attack(BattlePlayer.GetInstance());
            }
            else
            {
                if (m_ChargeMode == false)
                {
                    m_ChargeMode = true;
                    
                    BattleSystem.GetInstance().EnemyDied(m_LeftHand);
                    BattleSystem.GetInstance().EnemyDied(m_RightHand);

                    List<string> l_Text = new List<string>();
                    l_Text.Add("Ну всеее… с меня ДОСТАТОЧНО!");

                    TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
                    l_TextPanel.SetTalkingAnimator(m_HeadAnimator, "Talking");
                    l_TextPanel.SetText(l_Text);
                    l_TextPanel.AddButtonAction(l_TextPanel.Close);

                    BattleShowPanelStep l_ShowStep = new BattleShowPanelStep(l_TextPanel);
                    ResultSystem.GetInstance().AddStep(l_ShowStep);

                    LeshiiAttackEffect l_LeshiiAttackEffect = Instantiate(LeshiiAttackEffect.prefab);
                    l_LeshiiAttackEffect.AddPlayAction(PlayStartCharge);
                    m_EndEffectChecker.AddAttackEffect(l_LeshiiAttackEffect);

                    BattlePlayEffectStep l_PlayStep = new BattlePlayEffectStep(l_LeshiiAttackEffect);
                    ResultSystem.GetInstance().AddStep(l_PlayStep);
                }
                else
                {
                    m_ChargeCounter++;
                    if (m_ChargeCounter >= m_ChargeCount)
                    {
                        SpecialAttack();
                    }
                }
            }

            ResultSystem.GetInstance().ShowResult();
        }

        private void SummonHands()
        {
            m_SummonHandsCounter = 0;

            List<string> l_Text = new List<string>();
            l_Text.Add("Хрммфф!");

            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
            l_TextPanel.SetTalkingAnimator(m_HeadAnimator, "Talking");
            l_TextPanel.SetText(l_Text);
            l_TextPanel.AddButtonAction(l_TextPanel.Close);

            BattleShowPanelStep l_ShowStep = new BattleShowPanelStep(l_TextPanel);
            ResultSystem.GetInstance().AddStep(l_ShowStep);

            LeshiiAttackEffect l_SummonHandsEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_SummonHandsEffect.AddPlayAction(PlaySummonHands);
            m_EndEffectChecker.AddAttackEffect(l_SummonHandsEffect);

            BattlePlayEffectStep l_PlayStep = new BattlePlayEffectStep(l_SummonHandsEffect);
            ResultSystem.GetInstance().AddStep(l_PlayStep);
        }

        private void PlaySummonHands()
        {
            m_BodyAnimator.SetTrigger("SummonHands");

            m_HandsLive = Vector2.zero;
            m_LeftHand.Recovery();
            m_RightHand.Recovery();
            CalculateIdle(m_HandsLive);
            ((BattleSystemBoss)(BattleSystem.GetInstance())).InitLeshiiOrgans();
        }

        private void SpecialAttack()
        {
            m_ChargeCounter = 0;

            LeshiiAttackEffect l_LeshiiAttackEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeshiiAttackEffect.AddPlayAction(PlaySpecialAttack);
            m_EndEffectChecker.AddAttackEffect(l_LeshiiAttackEffect);

            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_LeshiiAttackEffect);
            ResultSystem.GetInstance().AddStep(l_Step);

            DamageSystem.GetInstance().Attack(this, BattlePlayer.GetInstance(), 10.0f);
        }

        private void PlayStartCharge()
        {
            m_BodyAnimator.SetTrigger("StartCharge");
        }

        private void PlaySpecialAttack()
        {
            m_BodyAnimator.SetTrigger("AttackCharge");

            LeshiiAttackEffect l_LeshiiAttackEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeshiiAttackEffect.AddPlayAction(PlayStartCharge);
            m_EndEffectChecker.AddAttackEffect(l_LeshiiAttackEffect);

            BattlePlayEffectStep l_PlayStep = new BattlePlayEffectStep(l_LeshiiAttackEffect);
            ResultSystem.GetInstance().AddStep(l_PlayStep);
        }
        
        public bool IsAllHandsDied()
        {
            if (m_LeftHand.isDead && m_RightHand.isDead)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Block()
        {
            DamageSystem.GetInstance().AttackFail();

            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
            l_TextPanel.SetText(new List<string>() { "Лол блок" });
            l_TextPanel.SetTalkingAnimator(headAnimator, "Talking");
            l_TextPanel.AddButtonAction(l_TextPanel.Close);

            BattleShowPanelStep l_DialogStep = new BattleShowPanelStep(l_TextPanel);
            ResultSystem.GetInstance().AddStep(l_DialogStep);

            LeshiiAttackEffect l_BlockStopEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_BlockStopEffect.AddPlayAction(PlayStopBlock);
            m_EndEffectChecker.AddAttackEffect(l_BlockStopEffect);

            BattlePlayEffectStep l_BlockStopStep = new BattlePlayEffectStep(l_BlockStopEffect);
            ResultSystem.GetInstance().AddStep(l_BlockStopStep);
        }

        public void StartBlock()
        {
            LeshiiAttackEffect l_BlockStartEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_BlockStartEffect.AddPlayAction(PlayStartBlock);
            m_EndEffectChecker.AddAttackEffect(l_BlockStartEffect);

            BattlePlayEffectStep l_BlockStartStep = new BattlePlayEffectStep(l_BlockStartEffect);
            ResultSystem.GetInstance().AddStep(l_BlockStartStep);
        }

        public void OrganDie(OrganIds p_OrganIds)
        {
            switch (p_OrganIds)
            {
                case OrganIds.LeftHand:
                    RightHandDie();
                    break;
                case OrganIds.RightHand:
                    LeftHandDie();
                    break;
                case OrganIds.Body:
                    LeshiiDie();
                    break;
            }
        }

        private void PlayStartBlock()
        {
            bodyAnimator.SetTrigger("BlockStart");
        }

        private void PlayStopBlock()
        {
            bodyAnimator.SetTrigger("BlockStop");
        }

        private void RightHandDie()
        {
            LeshiiAttackEffect l_LeftHandDieEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeftHandDieEffect.AddPlayAction(PlayLeftHandDie);
            m_EndEffectChecker.AddAttackEffect(l_LeftHandDieEffect);

            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_LeftHandDieEffect);
            ResultSystem.GetInstance().AddStep(l_Step);

            m_HandsLive.x = 1.0f;
            BattleSystem.GetInstance().EnemyDied(m_LeftHand);
        }

        private void PlayRightHandDie()
        {
            m_BodyAnimator.SetTrigger("RightHandDestroy");
            CalculateIdle(m_HandsLive);
        }

        private void LeftHandDie()
        {
            LeshiiAttackEffect l_RightHandDieEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_RightHandDieEffect.AddPlayAction(PlayRightHandDie);
            m_EndEffectChecker.AddAttackEffect(l_RightHandDieEffect);

            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_RightHandDieEffect);
            ResultSystem.GetInstance().AddStep(l_Step);

            m_HandsLive.y = 1.0f;
            BattleSystem.GetInstance().EnemyDied(m_RightHand);
        }

        private void PlayLeftHandDie()
        {
            m_BodyAnimator.SetTrigger("LeftHandDestroy");
            CalculateIdle(m_HandsLive);
        }

        private void LeshiiDie()
        {
            LeshiiAttackEffect l_LeshiiAttackEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeshiiAttackEffect.AddPlayAction(PlayLeshiiDie);
            m_EndEffectChecker.AddAttackEffect(l_LeshiiAttackEffect);

            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_LeshiiAttackEffect);
            ResultSystem.GetInstance().AddStep(l_Step);
        }

        private void PlayLeshiiDie()
        {
            m_BodyAnimator.SetTrigger("Die");
            BattleSystem.GetInstance().EnemyDied(m_Body);
            Die();
        }

        private void CalculateIdle(Vector2 p_HandsLive)
        {
            m_BodyAnimator.SetFloat("LeftHand", p_HandsLive.x);
            m_BodyAnimator.SetFloat("RightHand", p_HandsLive.y);
        }

        public override void Attack(BattleActor p_Target)
        {
            if (!m_LeftHand.isDead)
            {
                AttackLeftHand();
            }

            if (!m_RightHand.isDead)
            {
                AttackRightHand();
            }
        }

        private void AttackRightHand()
        {
            LeshiiAttackEffect l_LeshiiAttackEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeshiiAttackEffect.AddPlayAction(PlayAttackRightHand);
            m_EndEffectChecker.AddAttackEffect(l_LeshiiAttackEffect);

            BattlePlayEffectStep l_AnimationStep = new BattlePlayEffectStep(l_LeshiiAttackEffect);
            ResultSystem.GetInstance().AddStep(l_AnimationStep);

            if (Random.Range(0, 100) > 50)
            {
                m_IsHealCast = true;
                m_Body.health += 5;

                TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
                l_TextPanel.SetText(new List<string>() { m_RightHand.actorName + " восстановил 5 очков здоровья" });
                l_TextPanel.AddButtonAction(l_TextPanel.Close);

                BattleShowPanelStep l_ShowPanelStep = new BattleShowPanelStep(l_TextPanel);

                ResultSystem.GetInstance().AddStep(l_ShowPanelStep);
            }
            else
            {
                DamageSystem.GetInstance().Attack(m_RightHand, BattlePlayer.GetInstance(), 1.0f);
            }
        }

        private void PlayAttackRightHand()
        {
            if (m_IsHealCast)
            {
                m_IsHealCast = false;
                m_BodyAnimator.SetTrigger("Healcast");
                return;
            }
            m_BodyAnimator.SetTrigger("AttackRight");
        }

        private void AttackLeftHand()
        {
            LeshiiAttackEffect l_LeshiiAttackEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeshiiAttackEffect.AddPlayAction(PlayAttackLeftHand);
            m_EndEffectChecker.AddAttackEffect(l_LeshiiAttackEffect);

            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_LeshiiAttackEffect);
            ResultSystem.GetInstance().AddStep(l_Step);

            DamageSystem.GetInstance().Attack(m_LeftHand, BattlePlayer.GetInstance(), 1.0f);
        }

        private void PlayAttackLeftHand()
        {
            m_BodyAnimator.SetTrigger("AttackLeft");
        }
    }
}