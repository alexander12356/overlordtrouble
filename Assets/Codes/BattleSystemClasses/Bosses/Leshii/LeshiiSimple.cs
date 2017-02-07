using System.Collections.Generic;

namespace BattleSystemClasses.Bosses.Leshii
{
    public class LeshiiSimple : Leshii
    {
        private int m_ChargeCounter = 0;
        private int m_ChargeCount = 3;

        public override void InitStats()
        {
            base.InitStats();
            
            level = LeshiiDataBase.GetInstance().GetLevel();
            attackStat = LeshiiDataBase.GetInstance().GetAttackStat();
            defenseStat = LeshiiDataBase.GetInstance().GetDefenseStat();
        }

        public override void RunTurn()
        {
            base.RunTurn();

            switch (m_Mode)
            {
                case Mode.Idle:
                    if (IsAllHandsDied())
                    {
                        CheckSummonHands();
                        m_Mode = Mode.HandsDied;
                    }
                    else
                    {
                        Attack(BattlePlayer.GetInstance());
                    }
                    break;
                case Mode.Charge:
                    if (m_ChargeCounter >= m_ChargeCount)
                    {
                        SpecialAttack();
                    }
                    else
                    {
                        CheckSpecialAttack();
                    }
                    break;
                case Mode.HandsDied:
                    if (m_Body.health < 35)
                    {
                        SummonHands();
                        StartCharge();
                        m_Mode = Mode.Charge;
                    }
                    else
                    {
                        CheckSummonHands();
                    }
                    break;
            }
            ResultSystem.GetInstance().ShowResult();
        }

        #region START_CHARGE
        private void StartCharge()
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

        private void PlayStartCharge()
        {
            m_BodyAnimator.SetTrigger("StartCharge");
        }
        #endregion

        #region CHECK_SUMMON_HANDS
        private void CheckSummonHands()
        {
            if (m_SummonHandsCounter < m_SummonHandsCount)
            {
                string l_StepText = string.Empty;
                int l_Step = m_SummonHandsCount - m_SummonHandsCounter;

                if (l_Step > 1)
                {
                    l_StepText = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:Steps");
                }
                else
                {
                    l_StepText = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:Step");
                }

                string l_SummonHandsText = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:SummonHands", new string[] { l_Step.ToString(), l_StepText });

                List<string> l_Text = new List<string>();
                l_Text.Add(l_SummonHandsText);

                TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
                l_TextPanel.SetText(l_Text);
                l_TextPanel.AddButtonAction(l_TextPanel.Close);

                BattleShowPanelStep l_ResultStep = new BattleShowPanelStep(l_TextPanel);
                ResultSystem.GetInstance().AddStep(l_ResultStep);

                m_SummonHandsCounter++;
            }
            else
            {
                SummonHands();
                m_Mode = Mode.Idle;
            }
        }
        #endregion

        #region SPECIAL_ATTACK
        private void SpecialAttack()
        {
            m_ChargeCounter = 0;

            LeshiiAttackEffect l_LeshiiAttackEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeshiiAttackEffect.AddPlayAction(PlaySpecialAttack);
            m_EndEffectChecker.AddAttackEffect(l_LeshiiAttackEffect);

            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_LeshiiAttackEffect);
            ResultSystem.GetInstance().AddStep(l_Step);

            float l_DamageValue = LeshiiDataBase.GetInstance().GetSpecialAttackValue();
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

            LeshiiAttackEffect l_LeshiiAttackEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeshiiAttackEffect.AddPlayAction(PlayStartCharge);
            m_EndEffectChecker.AddAttackEffect(l_LeshiiAttackEffect);

            BattlePlayEffectStep l_PlayStep = new BattlePlayEffectStep(l_LeshiiAttackEffect);
            ResultSystem.GetInstance().AddStep(l_PlayStep);
        }

        private void CheckSpecialAttack()
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
    }
}
