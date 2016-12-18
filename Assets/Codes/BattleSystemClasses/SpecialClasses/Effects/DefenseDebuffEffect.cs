using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseDebuffEffect : BaseEffect
{
    private float m_BaseDefenseValue = 0;
    private float m_DefenseValue = 0;
    private int m_Duration = 0;
    private int m_DurationCounter = 0;
    private BattleActor m_Target;

    public DefenseDebuffEffect(Special p_Special, float p_DefenseValue, int p_Duration) : base(p_Special)
    {
        id = "DefenseDebuff";
        m_DefenseValue = m_BaseDefenseValue = p_DefenseValue;
        m_Duration = p_Duration;
    }

    public override void Run(BattleActor p_Sender, BattleActor p_Target)
    {
        base.Run(p_Sender, p_Target);

        m_Target = p_Target;

        if (m_Target.HasSpecial(m_Special.id))
        {
            m_Target.StackEffect(m_Special.id, this);
        }
        else
        {
            m_Target.defenseStat -= m_DefenseValue;
            m_Target.AddEffect(m_Special.id, this);
            m_Target.AddDebuff();
        }

        DamageSystem.GetInstance().AddEffectSpecial(p_Target, m_Special);
    }

    public override void Upgrade()
    {
        base.Upgrade();

        m_DefenseValue += m_BaseDefenseValue * 0.1f;
    }

    public override void Effective()
    {
        base.Effective();

        m_DurationCounter++;
    }

    public override bool CheckEnd()
    {
        if (m_Duration > m_DurationCounter)
        {
            return false;
        }
        m_Target.defenseStat += m_DefenseValue;
        m_Target.RemoveDebuff();

        EffectSystem.GetInstance().AddRemoveEffectSpecial(m_Target, m_Special);

        return true;
    }

    public override void Stack(BaseEffect p_Effect)
    {
        base.Stack(p_Effect);

        m_DurationCounter = 0;
    }
}
