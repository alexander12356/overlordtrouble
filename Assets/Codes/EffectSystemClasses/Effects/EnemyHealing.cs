using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealing : BaseEffect
{
    private float m_HealingValue = 0.0f;

    public EnemyHealing(Special p_Special, float p_HealingValue) : base (p_Special)
    {
        m_HealingValue = p_HealingValue;
    }

    public override void Run(IEffectInfluenced p_Sender, IEffectInfluenced p_Target)
    {
        base.Run(p_Sender, p_Target);

        if (BattleSystem.IsInstance())
        {
            for (int i = 0; i < BattleSystem.GetInstance().GetEnemyList().Count; i++)
            {
                BattleActor l_Target = BattleSystem.GetInstance().GetEnemyList()[i] as BattleActor;

                float l_TargetHealedValue = l_Target.baseHealth * m_HealingValue;
                l_Target.health += l_TargetHealedValue;

                if (l_Target != p_Sender as BattleActor)
                {
                    DamageSystem.GetInstance().AddRestoration(l_Target, RestorationType.Healing, l_TargetHealedValue);
                }
            }
        }
    }
}
