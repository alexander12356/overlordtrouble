using System.Collections.Generic;

namespace BattleSystemClasses.Bosses.Leshii
{
    public class LeshiiOrgan : BattleEnemy
    {
        protected OrganType m_OrganType = OrganType.NONE;
        protected Leshii m_Leshii = null;

        public override void Awake()
        {
            base.Awake();
        }
        
        public void Init(OrganType p_Id, Leshii p_Leshii)
        {
            m_OrganType = p_Id;
            m_Leshii = p_Leshii;
            InitStats();
        }

        public override void InitStats()
        {
            actorName = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:" + m_OrganType);
            health = baseHealth = LeshiiDataBase.GetInstance().GetHealth(m_OrganType);
            level = LeshiiDataBase.GetInstance().GetLevel();
            attackStat = LeshiiDataBase.GetInstance().GetAttackStat();
            defenseStat = LeshiiDataBase.GetInstance().GetDefenseStat();
        }

        public override void Damage(float p_DamageValue)
        {
            health -= p_DamageValue;
        }

        public override bool IsCanDamage(float p_Damage)
        {
            if (m_OrganType == OrganType.Body)
            {
                if (m_Leshii.IsAllHandsDied() || m_Leshii.isChargeMode)
                {
                    return true;
                }
                if (!isAoeAttack)
                {
                    m_Leshii.Block();
                }
                else
                {
                    isAoeAttack = false;
                }
                return false;
            }

            return true;
        }

        public override void Die()
        {
            isDead = true;

            if (m_OrganType == OrganType.Body)
            {
                TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
                l_TextPanel.SetText(new List<string>() { "Ой ой…" });
                l_TextPanel.SetTalkingAnimator(m_Leshii.headAnimator, "Talking");
                l_TextPanel.AddButtonAction(l_TextPanel.Close);

                BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
                ResultSystem.GetInstance().AddStep(l_Step);

                m_Leshii.OrganDie(m_OrganType);
            }
            else
            {
                m_Leshii.OrganDie(m_OrganType);

                TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
                l_TextPanel.SetText(new List<string>() { actorName + " потеряла силу" });
                l_TextPanel.AddButtonAction(l_TextPanel.Close);

                BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
                ResultSystem.GetInstance().AddStep(l_Step);
            }
        }

        public void Recovery()
        {
            isDead = false;
            InitStats();
        }

        public override void CheckPrevAttack()
        {
            if (m_OrganType == OrganType.Body)
            {
                if (!m_Leshii.IsAllHandsDied() && !m_Leshii.isChargeMode)
                {
                    m_Leshii.StartBlock();
                }
            }
        }
    }
}