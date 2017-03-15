using UnityEngine;
using System.Collections;

public class Acceleration : BaseEffect
{
    private int m_Chance = 0;
    private int m_RepeatCount = 0;
    private int m_Duration = 0;
    private int m_DurationCounter = 0;

    private BattleActor m_Sender = null;

    private System.Random m_Random = new System.Random();

    public Acceleration(Special p_Special, int p_Chance, int p_RepeatCounter, int p_Duration) : base(p_Special)
    {
        id = "Acceleration";

        m_Chance = p_Chance;
        m_RepeatCount = p_RepeatCounter;
        m_Duration = p_Duration;
    }

    public override void Run(IEffectInfluenced p_Sender, IEffectInfluenced p_Target)
    {
        base.Run(p_Sender, p_Target);

        if (m_Random.Next(0, 100) > m_Chance)
        {
            return;
        }

        m_Sender = p_Sender as BattleActor;

        if (m_Sender.HasSpecial(m_Special.id))
        {
            m_Sender.StackEffect(m_Special.id, this);
        }
        else
        {
            m_Sender.AddEffect(m_Special.id, this);
            m_Sender.AddBuffIcon();

            DamageSystem.GetInstance().AddEffectSpecial(m_Sender, m_Special);
        }
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

        EffectSystem.GetInstance().AddRemoveEffectSpecial(m_Sender, m_Special);

        return true;
    }

    public override void Stack(BaseEffect p_Effect)
    {
        m_DurationCounter = 0;
    }
}
