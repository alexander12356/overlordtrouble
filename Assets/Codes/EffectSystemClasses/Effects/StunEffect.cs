using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEffect : BaseEffect
{
    private int m_Duration = 5;
    private int m_DurationCounter = 0;
    private BattlePlayer m_Player = null;

    public StunEffect(Special p_Special) : base(p_Special)
    {
        id = "Stun";
    }

    public override void Run(IEffectInfluenced p_Sender, IEffectInfluenced p_Target)
    {
        base.Run(p_Sender, p_Target);
        
        m_Player = (BattlePlayer)p_Target;

        if (m_Player.HasSpecial(m_Special.id))
        {
            m_Player.StackEffect(m_Special.id, this);
        }
        else
        {
            m_Player.monstyleCapacity = 3;
            m_Player.AddEffect(m_Special.id, this);
            m_Player.AddStatusEffectIcon(id);
        }

        DamageSystem.GetInstance().AddEffectSpecial(m_Player, m_Special);
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

        m_Player.monstyleCapacity = 4;
        m_Player.RemoveStatusEffectIcon(id);
        EffectSystem.GetInstance().AddRemoveEffectSpecial(m_Player, m_Special);

        return true;
    }

    public override void Stack(BaseEffect p_Effect)
    {
        base.Stack(p_Effect);

        m_DurationCounter = 0;
    }

    public override void EndImmediately()
    {
        base.EndImmediately();

        m_Player.monstyleCapacity = 4;
        m_Player.RemoveStatusEffectIcon(id);
    }
}
