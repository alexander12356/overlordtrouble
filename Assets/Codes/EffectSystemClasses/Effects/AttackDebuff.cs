using System;

public class AttackDebuff : BaseEffect
{
    private int m_BaseAttackDebuff = 0;
    private int m_AttackPercent = 0;
    private int m_Duration = 0;
    private int m_DurationCounter = 0;
    private int m_Chance = 0;

    private float m_AttackValue = 0;

    private BattleActor m_Target;

    public AttackDebuff(Special p_Special, int p_Chance, int p_AttackPercent, int p_Duration) : base(p_Special)
    {
        id = "AttackDebuff";

        m_Chance = p_Chance;
        m_AttackPercent = m_BaseAttackDebuff = p_AttackPercent;
        m_Duration = p_Duration;
    }

    public override void Run(IEffectInfluenced p_Sender, IEffectInfluenced p_Target)
    {
        base.Run(p_Sender, p_Target);

        m_Target = p_Target as BattleActor;

        Random l_Random = new Random();
        if (m_Chance < l_Random.Next(0, 100))
        {
            return;
        }

        if (m_Target.HasSpecial(m_Special.id))
        {
            m_Target.StackEffect(m_Special.id, this);
        }
        else
        {
            m_AttackValue = m_Target.attackStat * m_AttackPercent / 100;

            m_Target.attackStat -= m_AttackValue;
            m_Target.AddEffect(m_Special.id, this);
            m_Target.AddDebuffIcon();
        }

        DamageSystem.GetInstance().AddEffectSpecial(m_Target, m_Special);
    }

    public override void Upgrade()
    {
        base.Upgrade();

        m_AttackPercent += (int)(m_BaseAttackDebuff * 0.1f);
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
        m_Target.attackStat += m_AttackValue;
        m_Target.RemoveDebuffIcon();

        EffectSystem.GetInstance().AddRemoveEffectSpecial(m_Target, m_Special);

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

        m_Duration = m_DurationCounter;
    }
}
