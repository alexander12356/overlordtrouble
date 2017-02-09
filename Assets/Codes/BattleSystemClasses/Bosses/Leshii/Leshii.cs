using System.Collections.Generic;

using UnityEngine;

namespace BattleSystemClasses.Bosses.Leshii
{
    public enum OrganType
    {
        NONE = -1,
        RightHand,
        LeftHand,
        Body,
        Headmain
    }

    public enum Mode
    {
        HandsDied,
        Idle,
        Charge
    }

    public class Leshii : BattleEnemy
    {
        protected Animator m_BodyAnimator = null;
        protected Animator m_HeadAnimator = null;
        protected Vector2 m_HandsLive = Vector2.zero;
        protected Mode m_Mode = Mode.Idle;
        protected int m_SummonHandsCounter = 0;
        protected int m_SummonHandsCount = 2;
        protected VisualEffectChecker m_EndEffectChecker = null;
        protected bool m_ChargeMode = false;
        protected int m_ChargeCounter = 0;
        protected int m_ChargeCount = 3;
        protected LeshiiData m_LeshiiData;
        protected float m_CritivalHealthValue = 35.0f;

        private bool m_IsHealCast = false;
        private bool m_IsStun = false;

        [SerializeField]
        protected LeshiiOrgan m_RightHand = null;

        [SerializeField]
        protected LeshiiOrgan m_LeftHand = null;

        [SerializeField]
        protected LeshiiOrgan m_Body = null;

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
        public LeshiiData leshiiData
        {
            get { return m_LeshiiData; }
        }

        public override void Awake()
        {
            base.Awake();

            m_EndEffectChecker = GetComponent<VisualEffectChecker>();
        }
        
        public List<LeshiiOrgan> GetOrgans()
        {
            List<LeshiiOrgan> l_LeshiiOrgans = new List<LeshiiOrgan>() { m_RightHand, m_Body, m_LeftHand };
            return l_LeshiiOrgans;
        }

        public virtual bool IsAllHandsDied()
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

        protected void CalculateIdle()
        {
            m_BodyAnimator.SetFloat("LeftHand", m_HandsLive.x);
            m_BodyAnimator.SetFloat("RightHand", m_HandsLive.y);
        }

        public override void InitStats()
        {
            attackStat = m_LeshiiData.attackStat;
            defenseStat = m_LeshiiData.defenseStat;
            level = m_LeshiiData.level;
            m_SummonHandsCount = m_LeshiiData.summonHandsCount;
            m_ChargeCount = m_LeshiiData.specialAttackChargeCount;
            m_CritivalHealthValue = m_LeshiiData.criticalHealthValue;

            m_BodyAnimator = GetComponent<Animator>();
            m_HeadAnimator = transform.FindChild(OrganType.Headmain.ToString()).GetComponentInChildren<Animator>();

            actorName = LocalizationDataBase.GetInstance().GetText("Boss:Leshii");

            m_RightHand.Init(OrganType.RightHand, this);
            m_LeftHand.Init(OrganType.LeftHand, this);
            m_Body.Init(OrganType.Body, this);

            m_LeftHand.deathOrder = 0;
            m_RightHand.deathOrder = 1;
            m_Body.deathOrder = 2;

            CalculateIdle();
        }

        public override void RunTurn()
        {
        }

        #region SUMMON_HANDS
        protected void SummonHands()
        {
            m_SummonHandsCounter = 0;

            m_HandsLive = Vector2.zero;
            m_LeftHand.Recovery();
            m_RightHand.Recovery();
            ((BattleSystemBoss)(BattleSystem.GetInstance())).InitLeshiiOrgans();

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
            CalculateIdle();
        }
        #endregion

        #region BLOCK
        public void Block()
        {
            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
            l_TextPanel.SetText(new List<string>() { "Эти руки висят на мне не для красоты." });
            l_TextPanel.SetTalkingAnimator(headAnimator, "Talking");
            l_TextPanel.AddButtonAction(l_TextPanel.Close);

            BattleShowPanelStep l_DialogStep = new BattleShowPanelStep(l_TextPanel);
            DamageSystem.GetInstance().AddAfterAttackStep(l_DialogStep);

            LeshiiAttackEffect l_BlockStopEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_BlockStopEffect.AddPlayAction(PlayStopBlock);
            m_EndEffectChecker.AddAttackEffect(l_BlockStopEffect);

            BattlePlayEffectStep l_BlockStopStep = new BattlePlayEffectStep(l_BlockStopEffect);
            DamageSystem.GetInstance().AddAfterAttackStep(l_BlockStopStep);
        }
        
        public void StartBlock()
        {
            LeshiiAttackEffect l_BlockStartEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_BlockStartEffect.AddPlayAction(PlayStartBlock);
            m_EndEffectChecker.AddAttackEffect(l_BlockStartEffect);

            BattlePlayEffectStep l_BlockStartStep = new BattlePlayEffectStep(l_BlockStartEffect);
            DamageSystem.GetInstance().AddBeforeAttackSteps(l_BlockStartStep);
        }
        
        private void PlayStartBlock()
        {
            bodyAnimator.SetTrigger("BlockStart");
        }

        private void PlayStopBlock()
        {
            bodyAnimator.SetTrigger("BlockStop");
        }
        #endregion

        #region DIE
        public void OrganDie(OrganType p_OrganIds)
        {
            switch (p_OrganIds)
            {
                case OrganType.LeftHand:
                    LeftHandDie();
                    break;
                case OrganType.RightHand:
                    RightHandDie();
                    break;
                case OrganType.Body:
                    LeshiiDie();
                    break;
            }
        }

        public virtual void LeshiiDie()
        {
            LeshiiAttackEffect l_LeshiiAttackEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeshiiAttackEffect.AddPlayAction(Die);
            m_EndEffectChecker.AddAttackEffect(l_LeshiiAttackEffect);

            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_LeshiiAttackEffect);
            ResultSystem.GetInstance().AddStep(l_Step);
        }

        public override void Die()
        {
            m_BodyAnimator.SetTrigger("Die");
            BattleSystem.GetInstance().EnemyDied(m_Body);
            BattleSystem.GetInstance().EnemyDied(this);
        }
        #endregion

        #region RIGHT_HAND_DIE
        private void RightHandDie()
        {
            BattleSystem.GetInstance().EnemyDied(m_RightHand);

            LeshiiAttackEffect l_RightHandDieEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_RightHandDieEffect.AddPlayAction(PlayRightHandDie);
            m_EndEffectChecker.AddAttackEffect(l_RightHandDieEffect);

            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_RightHandDieEffect);
            ResultSystem.GetInstance().AddStep(l_Step);

            BattleRunActionStep l_ActionStep = new BattleRunActionStep(CalculateIdle);
            ResultSystem.GetInstance().AddStep(l_ActionStep);
        }

        private void PlayRightHandDie()
        {
            m_BodyAnimator.SetTrigger("RightHandDestroy");

            m_HandsLive.y = 1.0f;
            //CalculateIdle();
        }
        #endregion

        #region LEFT_HAND_DIE
        private void LeftHandDie()
        {
            BattleSystem.GetInstance().EnemyDied(m_LeftHand);

            LeshiiAttackEffect l_LeftHandDieEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeftHandDieEffect.AddPlayAction(PlayLeftHandDie);
            m_EndEffectChecker.AddAttackEffect(l_LeftHandDieEffect);

            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_LeftHandDieEffect);
            ResultSystem.GetInstance().AddStep(l_Step);

            BattleRunActionStep l_ActionStep = new BattleRunActionStep(CalculateIdle);
            ResultSystem.GetInstance().AddStep(l_ActionStep);
        }

        private void PlayLeftHandDie()
        {
            m_BodyAnimator.SetTrigger("LeftHandDestroy");

            m_HandsLive.x = 1.0f;
            //CalculateIdle();
        }
        #endregion

        #region ATTACK
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
        #endregion

        #region ATTACK_RIGHT_HAND
        protected void AttackRightHand()
        {
            LeshiiAttackEffect l_LeshiiAttackEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeshiiAttackEffect.AddPlayAction(PlayAttackRightHand);
            m_EndEffectChecker.AddAttackEffect(l_LeshiiAttackEffect);

            BattlePlayEffectStep l_AnimationStep = new BattlePlayEffectStep(l_LeshiiAttackEffect);
            ResultSystem.GetInstance().AddStep(l_AnimationStep);

            float l_HealthEffect = m_LeshiiData.handsEffectChance[OrganType.RightHand];
            float l_DamageValue = m_LeshiiData.handsAttackValue[OrganType.RightHand];

            if (m_Body.health < m_Body.baseHealth && Random.Range(0, 100) < l_HealthEffect)
            {
                m_IsHealCast = true;

                float l_HealingValue = m_LeshiiData.rightHandHealingValue;
                m_Body.health += l_HealingValue;

                TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
                string l_Text = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:Health", new string[] { m_RightHand.actorName, l_HealingValue.ToString() });
                l_TextPanel.SetText(new List<string>() { l_Text });
                l_TextPanel.AddButtonAction(l_TextPanel.Close);

                BattleShowPanelStep l_ShowPanelStep = new BattleShowPanelStep(l_TextPanel);

                ResultSystem.GetInstance().AddStep(l_ShowPanelStep);
            }
            else
            {
                DamageSystem.GetInstance().Attack(m_RightHand, BattlePlayer.GetInstance(), Element.Physical, l_DamageValue);
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
        #endregion

        #region ATTACK_LEFT_HAND
        protected void AttackLeftHand()
        {
            LeshiiAttackEffect l_LeshiiAttackEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeshiiAttackEffect.AddPlayAction(PlayAttackLeftHand);
            m_EndEffectChecker.AddAttackEffect(l_LeshiiAttackEffect);

            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_LeshiiAttackEffect);
            ResultSystem.GetInstance().AddStep(l_Step);

            float l_StunChance = m_LeshiiData.handsEffectChance[OrganType.LeftHand];
            float l_DamageValue = m_LeshiiData.handsAttackValue[OrganType.LeftHand];

            if (Random.Range(0, 100) < l_StunChance)
            {
                m_IsStun = true;

                Special l_StunSpecial = new Special("Stun", Element.Physical, false, false);
                l_StunSpecial.specialName = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:Stun");

                StunEffect l_StunEffect = new StunEffect(l_StunSpecial);
                AttackEffect l_AttackEffect = new AttackEffect(l_StunSpecial, l_DamageValue * 0.5f);

                List<BaseEffect> l_EffectList = new List<BaseEffect>();
                l_EffectList.Add(l_AttackEffect);
                l_EffectList.Add(l_StunEffect);
                
                l_StunSpecial.SetEffects(l_EffectList);

                DamageSystem.GetInstance().EnemyAttack(this, BattlePlayer.GetInstance(), l_StunSpecial );
            }
            else
            {
                DamageSystem.GetInstance().Attack(m_LeftHand, BattlePlayer.GetInstance(), Element.Physical, l_DamageValue);
            }
        }

        private void PlayAttackLeftHand()
        {
            if (m_IsStun)
            {
                m_IsStun = false;
                m_BodyAnimator.SetTrigger("Stun");
                return;
            }
            m_BodyAnimator.SetTrigger("AttackLeft");
        }
        #endregion

        #region SPECIAL_ATTACK
        protected void SpecialAttack()
        {
            m_ChargeCounter = 0;

            LeshiiAttackEffect l_LeshiiSpecialAttackEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeshiiSpecialAttackEffect.AddPlayAction(PlaySpecialAttack);
            m_EndEffectChecker.AddAttackEffect(l_LeshiiSpecialAttackEffect);

            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_LeshiiSpecialAttackEffect);
            ResultSystem.GetInstance().AddStep(l_Step);

            float l_DamageValue = m_LeshiiData.specialAttackValue;
            Special l_NatureFury = new Special("NatureFury", Element.Physical, false, false);
            l_NatureFury.specialName = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:NatureFury");

            AttackEffect l_AttackEffect = new AttackEffect(l_NatureFury, l_DamageValue);

            List<BaseEffect> l_EffectList = new List<BaseEffect>();
            l_EffectList.Add(l_AttackEffect);

            l_NatureFury.SetEffects(l_EffectList);


            DamageSystem.GetInstance().EnemyAttack(this, BattlePlayer.GetInstance(), l_NatureFury);
        }

        private void PlaySpecialAttack()
        {
            m_BodyAnimator.SetTrigger("AttackCharge");

            StartCharge_AnimStart();
        }

        protected void CheckSpecialAttack()
        {
            string l_StepText = string.Empty;
            int l_Step = m_ChargeCount - m_ChargeCounter;

            if (l_Step > 1)
            {
                l_StepText = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:Steps");
            }
            else
            {
                l_StepText = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:Step");
            }

            string l_ChargeAttackText = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:CheckSpecialAttack", new string[] { l_Step.ToString(), l_StepText });

            List<string> l_Text = new List<string>();
            l_Text.Add(l_ChargeAttackText);

            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
            l_TextPanel.SetText(l_Text);
            l_TextPanel.AddButtonAction(l_TextPanel.Close);

            BattleShowPanelStep l_ResultStep = new BattleShowPanelStep(l_TextPanel);
            ResultSystem.GetInstance().AddStep(l_ResultStep);

            m_ChargeCounter++;
        }
        #endregion

        #region START_CHARGE
        public virtual void StartCharge()
        {
            m_ChargeMode = true;
            
            StartCharge_ShowText();
            StartCharge_AnimStart();
        }

        private void StartCharge_ShowText()
        {
            List<string> l_Text = new List<string>();
            l_Text.Add(LocalizationDataBase.GetInstance().GetText("Boss:Leshii:StartCharge"));

            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
            l_TextPanel.SetTalkingAnimator(m_HeadAnimator, "Talking");
            l_TextPanel.SetText(l_Text);
            l_TextPanel.AddButtonAction(l_TextPanel.Close);

            BattleShowPanelStep l_ShowStep = new BattleShowPanelStep(l_TextPanel);
            ResultSystem.GetInstance().AddStep(l_ShowStep);
        }

        protected void StartCharge_AnimStart()
        {
            LeshiiAttackEffect l_LeshiiStartChargeEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeshiiStartChargeEffect.AddPlayAction(PlayStartCharge);
            m_EndEffectChecker.AddAttackEffect(l_LeshiiStartChargeEffect);

            BattlePlayEffectStep l_PlayStep = new BattlePlayEffectStep(l_LeshiiStartChargeEffect);
            ResultSystem.GetInstance().AddStep(l_PlayStep);
        }

        private void PlayStartCharge()
        {
            m_BodyAnimator.SetTrigger("StartCharge");
        }
        #endregion
    }
}