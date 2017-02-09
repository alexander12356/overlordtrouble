using System.Collections.Generic;

namespace BattleSystemClasses.Bosses.Leshii
{
    public class LeshiiSpecial : Leshii
    {
        public bool isLeftHandDied
        {
            get { return m_LeftHand.isDead; }
        }

        public override void InitStats()
        {
            m_LeshiiData = LeshiiDataBase.GetInstance().GetSpecialLeshiiData();

            m_HealthDialogList.Add(new HealthDialogStruct("Boss:Leshii:Special:100hp", false));
            m_HealthDialogList.Add(new HealthDialogStruct("Boss:Leshii:Special:75hp", false));
            m_HealthDialogList.Add(new HealthDialogStruct("Boss:Leshii:Special:50hp", false));

            base.InitStats();

            m_ChargeMode = true;
            m_Mode = Mode.Charge;
        }

        #region RUN_TURN
        public override void RunTurn()
        {
            base.RunTurn();

            switch (m_Mode)
            {
                case Mode.Idle:
                    if (IsAllHandsDied())
                    {
                        SummonHands();
                    }
                    else
                    {
                        Attack(BattlePlayer.GetInstance());

                        if (m_RightHand.isDead)
                        {
                            SummonHands();
                        }
                        if (m_LeftHand.isDead)
                        {
                            SummonHands();
                        }
                    }
                    break;
                case Mode.Charge:
                    if (m_Body.health < m_CritivalHealthValue)
                    {
                        if (!m_LeftHand.isDead)
                        {
                            OrganDie(OrganType.LeftHand);
                        }

                        if (!m_RightHand.isDead)
                        {
                            OrganDie(OrganType.RightHand);
                        }

                        SetSimpleMode();

                        m_Mode = Mode.Idle;
                        m_ChargeMode = false;
                        break;
                    }

                    if (IsAllHandsDied())
                    {
                        SummonHands();
                        StartCharge_AnimStart();

                        m_ChargeCounter = 0;
                    }
                    else
                    {
                        Attack(BattlePlayer.GetInstance());
                    }

                    if (m_LeftHand.isDead)
                    {
                        ShowHealthDialog();
                    }

                    if (!m_RightHand.isDead)
                    {
                        if (m_ChargeCounter >= m_ChargeCount)
                        {
                            SpecialAttack();
                        }
                        else
                        {
                            CheckSpecialAttack();
                        }
                    }
                    else
                    {
                        m_ChargeCounter = 0;
                    }
                    break;
            }
            ResultSystem.GetInstance().ShowResult();
        }
        #endregion

        #region ATTACK
        public override void Attack(BattleActor p_Target)
        {
            if (!m_LeftHand.isDead)
            {
                AttackLeftHand();
            }

            if (!isChargeMode && !m_RightHand.isDead)
            {
                AttackRightHand();   
            }
        }
        #endregion

        #region DIE
        public override void LeshiiDie_ShowText()
        {
            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
            l_TextPanel.SetTalkingAnimator(headAnimator, "Talking");
            l_TextPanel.AddButtonAction(l_TextPanel.Close);

            string l_DieText = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:Die:Special");
            l_TextPanel.SetText(new List<string>() { l_DieText });

            BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
            ResultSystem.GetInstance().AddStep(l_Step);
        }
        #endregion

        #region SET_SIMPLE_MODE
        public override void LeshiiDie()
        {
            if (!m_LeftHand.CheckDeath())
            {
                OrganDie(OrganType.LeftHand);
            }
            if (!m_RightHand.CheckDeath())
            {
                OrganDie(OrganType.RightHand);
            }

            base.LeshiiDie();
        }

        private void SetSimpleMode()
        {
            BattleRunActionStep l_Step = new BattleRunActionStep(ChangeAnimatorMode);
            ResultSystem.GetInstance().AddStep(l_Step);
        }

        private void ChangeAnimatorMode()
        {
            m_Animator.SetBool("SimpleMode", true);
            SummonHands();
        }
        #endregion
    }
}
