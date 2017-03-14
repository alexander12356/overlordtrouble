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

        int l_IntHealingValue = System.Convert.ToInt32(System.Math.Ceiling(m_HealingValue));
        p_Sender.health += l_IntHealingValue;

        if (BattleSystem.IsInstance())
        {
            BattleActor l_Sender = p_Sender as BattleActor;
            DamageSystem.GetInstance().AddRestoration(l_Sender, RestorationType.Healing, l_IntHealingValue);
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();

        m_HealingValue += m_HealingValueBase * 0.1f;
    }
}
