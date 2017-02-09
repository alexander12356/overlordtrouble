using System.Collections.Generic;

namespace BattleSystemClasses.Bosses.Leshii
{
    public class LeshiiSimple : Leshii
    {
        public override void InitStats()
        {
            m_LeshiiData = LeshiiDataBase.GetInstance().GetSimpleLeshiiData();

            m_HealthDialogList.Add(new HealthDialogStruct("Boss:Leshii:Simple:100hp", false));
            m_HealthDialogList.Add(new HealthDialogStruct("Boss:Leshii:Simple:75hp", false));
            m_HealthDialogList.Add(new HealthDialogStruct("Boss:Leshii:Simple:50hp", false));

            base.InitStats();
        }

        public override void RunTurn()
        {
            base.RunTurn();

            switch (m_Mode)
            {
                case Mode.Idle:
                    if (IsAllHandsDied())
                    {
                        ShowHealthDialog();
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
                    if (m_Body.health < m_CritivalHealthValue)
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

        #region DIE
        public override void LeshiiDie_ShowText()
        {
            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
            l_TextPanel.SetTalkingAnimator(headAnimator, "Talking");
            l_TextPanel.AddButtonAction(l_TextPanel.Close);

            string l_DieText = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:Die");
            l_TextPanel.SetText(new List<string>() { l_DieText });

            BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
            ResultSystem.GetInstance().AddStep(l_Step);
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

        #region START_CHARGE
        public override void StartCharge()
        {
            base.StartCharge();

            BattleSystem.GetInstance().EnemyDied(m_LeftHand);
            BattleSystem.GetInstance().EnemyDied(m_RightHand);
        }
        #endregion
    }
}
