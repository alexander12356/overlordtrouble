using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEffect : BaseEffect
{
    private float m_HealingValue = 0.0f;
    private float m_HealingValueBase = 0.0f;

    public HealingEffect(Special p_Special, float p_HealingValue) : base (p_Special)
    {
        m_HealingValue = m_HealingValueBase = p_HealingValue;
    }

    public override void Run(IEffectInfluenced p_Sender, IEffectInfluenced p_Target)
    {
        base.Run(p_Sender, p_Target);

        p_Sender.health += m_HealingValue;

        if (BattleSystem.GetInstance().IsInstance())
        {
            BattleActor l_Sender = p_Sender as BattleActor;
            DamageSystem.GetInstance().AddRestoration(l_Sender, RestorationType.Healing, m_HealingValue);
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();

        m_HealingValue += m_HealingValueBase * 0.1f;
    }
}
