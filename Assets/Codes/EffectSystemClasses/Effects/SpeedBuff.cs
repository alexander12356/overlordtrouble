using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : BaseEffect
{
    private BattleActor m_Sender;
    private int m_Chance = 0;
    private int m_Value = 0;
    private int m_Duration = 0;

	public SpeedBuff(Special p_Special, int p_Chance, int p_Value, int p_Duration) : base(p_Special)
    {
        m_Chance = p_Chance;
        m_Value = p_Value;
        m_Duration = p_Duration;
    }

    public override void Run(IEffectInfluenced p_Sender, IEffectInfluenced p_Target)
    {
        base.Run(p_Sender, p_Target);

        if (BattleSystem.IsInstance())
        {
            m_Sender = p_Sender as BattleActor;

            if (m_Sender.HasSpecial(m_Special.id))
            {
                m_Sender.StackEffect(m_Special.id, this);
            }
            else
            {
                m_Sender.speedStat += m_Value;
                m_Sender.AddEffect(m_Special.id, this);
                m_Sender.AddBuffIcon();
            }

            DamageSystem.GetInstance().AddEffectSpecial(m_Sender, m_Special);
        }
        else
        {
            (p_Target as PlayerStatistics).speedStat += m_Value;
        }
    }

    public override void EndImmediately(IEffectInfluenced p_Target)
    {
        base.EndImmediately(p_Target);

        (p_Target as PlayerStatistics).speedStat -= m_Value;
    }
}
