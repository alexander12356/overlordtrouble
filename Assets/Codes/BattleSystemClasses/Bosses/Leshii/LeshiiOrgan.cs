using UnityEngine;
using System.Collections;

namespace BattleSystemClasses.Bosses.Leshii
{
    public class LeshiiOrgan : BattleEnemy
    {
        private LeshiiOrganIds m_Id = LeshiiOrganIds.NONE;
        private Leshii m_Leshii = null;

        public override void Awake()
        {
            base.Awake();
        }
        
        public void Init(LeshiiOrganIds p_Id, Leshii p_Leshii)
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
            if (m_Id == LeshiiOrganIds.Body)
            {
                if (m_Leshii.IsAllHandsDied())
                {
                    health -= p_DamageValue;
                }
                else
                {
                    //m_Leshii.Block();
                }

            }
            else
            {
                health -= p_DamageValue;
            }
        }

        public override void Die()
        {
            isDead = true;
            m_Leshii.DeadHand();
        }
    }
}