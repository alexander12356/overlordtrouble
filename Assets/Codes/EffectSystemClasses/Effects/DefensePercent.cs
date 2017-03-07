using System;

public class DefensePercent : BaseEffect
{
    private int m_DefensePercent = 0;
    private int m_BaseDefensePercent = 0;
    private int m_Duration = 0;
    private int m_DurationCounter = 0;
    private int m_Chance = 0;

    private float m_DefenseValue = 0.0f;

    private BattleActor m_Sender = null;

    public DefensePercent(Special p_Special, int p_Chance, int p_DefensePercent, int p_Duration) : base(p_Special)
    {
        id = "Defense";
        m_Chance = p_Chance;
        m_BaseDefensePercent = p_DefensePercent;
        m_Duration = p_Duration;
    }

    public override void Run(IEffectInfluenced p_Sender, IEffectInfluenced p_Target)
    {
        base.Run(p_Sender, p_Target);

        Random l_Random = new Random();

        if (m_Chance < l_Random.Next(0, 100))
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
            m_DefenseValue = m_Sender.defenseStat * m_DefensePercent / 100.0f;

            m_Sender.defenseStat += m_DefenseValue;
            m_Sender.AddEffect(m_Special.id, this);
            m_Sender.AddBuffIcon();
        }

        DamageSystem.GetInstance().AddEffectSpecial(m_Sender, m_Special);
    }

    public override void Upgrade()
    {
        base.Upgrade();

        m_DefensePercent += (int)(m_BaseDefensePercent * 0.1f);
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
        m_Sender.defenseStat -= m_DefenseValue;
        m_Sender.RemoveBuffIcon();

        EffectSystem.GetInstance().AddRemoveEffectSpecial(m_Sender, m_Special);

        return true;
    }

    public override void Stack(BaseEffect p_Effect)
    {
        base.Stack(p_Effect);

        m_DurationCounter = 0;
        DefensePercent l_Effect = (DefensePercent)p_Effect;

        m_Sender.defenseStat += l_Effect.m_DefenseValue;
        m_DefenseValue += l_Effect.m_DefenseValue;
    }
}
