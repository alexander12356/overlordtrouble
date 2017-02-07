using UnityEngine;

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
            base.InitStats();

            level = LeshiiDataBase.GetInstance().GetLevel();
            attackStat = LeshiiDataBase.GetInstance().GetAttackStat();
            defenseStat = LeshiiDataBase.GetInstance().GetDefenseStat();

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
                    m_Animator.SetBool("SimpleMode", true);
                    if (IsAllHandsDied())
                    {
                        SummonHands();
                    }
                    else
                    {
                        Attack(BattlePlayer.GetInstance());
                    }
                    break;
                case Mode.Charge:
                    if (m_Body.health < 35)
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

        public override void LeshiiDie()
        {
            if (!m_LeftHand.isDead)
            {
                OrganDie(OrganType.LeftHand);
            }

            if (!m_RightHand.isDead)
            {
                OrganDie(OrganType.RightHand);
            }

            LeshiiAttackEffect l_LeshiiAttackEffect = Instantiate(LeshiiAttackEffect.prefab);
            l_LeshiiAttackEffect.AddPlayAction(Die);
            m_EndEffectChecker.AddAttackEffect(l_LeshiiAttackEffect);

            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_LeshiiAttackEffect);
            ResultSystem.GetInstance().AddStep(l_Step);
        }
    }
}
