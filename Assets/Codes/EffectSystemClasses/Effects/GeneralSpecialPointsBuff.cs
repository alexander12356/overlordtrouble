using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSpecialPointsBuff : BaseEffect
{
    private int m_Chance;
    private int m_Value;
    private int m_Duration;
    private BattleActor m_Target = null;
    private System.Random m_Random = null;

    public GeneralSpecialPointsBuff(Special p_Special, int p_Chance, int p_Value, int p_Duration) : base(p_Special)
    {
        m_Random = new System.Random();

        m_Chance = p_Chance;
        m_Value = p_Value;
        m_Duration = p_Duration;
    }

    public override void Run(IEffectInfluenced p_Sender, IEffectInfluenced p_Target)
    {
        base.Run(p_Sender, p_Target);

        if (m_Chance < m_Random.Next(0, 100))
        {
            return;
        }

        if (BattleSystem.IsInstance())
        {
            m_Target = p_Target as BattleActor;

            if (m_Target.HasSpecial(m_Special.id))
            {
                m_Target.StackEffect(m_Special.id, this);
            }
            else
            {
                m_Target.baseSpecialPoints += m_Value;
                m_Target.AddEffect(m_Special.id, this);
                m_Target.AddBuffIcon();
            }

            DamageSystem.GetInstance().AddEffectSpecial(m_Target, m_Special);
        }
        else
        {
            p_Target.baseSpecialPoints += m_Value;
        }
    }

    public override void EndImmediately(IEffectInfluenced p_Actor)
    {
        base.EndImmediately(p_Actor);

        p_Actor.baseSpecialPoints -= m_Value;
    }
}
