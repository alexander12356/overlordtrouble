using UnityEngine;
using System.Collections;

namespace BattleSystemClasses.Bosses.Leshii
{
    public class LeshiiOrgan : BattleEnemy
    {
        private OrganIds m_Id = OrganIds.NONE;
        private Leshii m_Leshii = null;

        public override void Awake()
        {
            base.Awake();
        }
        
        public void Init(OrganIds p_Id, Leshii p_Leshii)
        {
            m_Id = p_Id;
            m_Leshii = p_Leshii;
            InitStats();
        }

        public override void InitStats()
        {
            actorName = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:" + m_Id);
            baseHealth = health = 10.0f;
        }

        public override void Damage(float p_DamageValue)
        {
            if (m_Id == OrganIds.Body)
            {
                if (m_Leshii.IsAllHandsDied())
                {
                    health -= p_DamageValue;
                    DamageSystem.GetInstance().AttackSuccess();
                }
                else
                {
                    m_Leshii.Block();
                }
            }
            else
            {
                health -= p_DamageValue;
                DamageSystem.GetInstance().AttackSuccess();
            }

            DamageSystem.GetInstance().ShowResult();
        }

        public override void Die()
        {
            isDead = true;

            m_Leshii.OrganDie(m_Id);
        }
    }
}