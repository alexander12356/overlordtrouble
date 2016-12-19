﻿using System.Collections;
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

    public override void Run(BattleActor p_Sender, BattleActor p_Target)
    {
        base.Run(p_Sender, p_Target);

        p_Sender.health += m_HealingValue;

        DamageSystem.GetInstance().AddRestoration(p_Sender, RestorationType.Healing, m_HealingValue);
    }

    public override void Upgrade()
    {
        base.Upgrade();

        m_HealingValue += m_HealingValueBase * 0.1f;
    }
}
