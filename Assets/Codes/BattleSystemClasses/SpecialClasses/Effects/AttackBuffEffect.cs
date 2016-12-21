public class AttackBuffEffect : BaseEffect
{
    private float m_AttackStatValue = 0.0f;
    private float m_BaseAttackStatValue = 0.0f;
    private float m_Duration = 0.0f;
    private float m_DurationCounter = 0.0f;
    private BattleActor m_Sender = null;

    public AttackBuffEffect(Special p_Special, float p_Value, float p_Duration) : base(p_Special)
    {
        m_AttackStatValue = m_BaseAttackStatValue = p_Value;
        m_Duration = p_Duration;
    }

    public override void Run(BattleActor p_Sender, BattleActor p_Target)
    {
        base.Run(p_Sender, p_Target);

        m_Sender = p_Sender;

        if (m_Sender.HasSpecial(m_Special.id))
        {
            m_Sender.StackEffect(m_Special.id, this);
        }
        else
        {
            m_Sender.attackStat += m_AttackStatValue;
            m_Sender.AddEffect(m_Special.id, this);
            m_Sender.AddBuffIcon();
        }

        DamageSystem.GetInstance().AddEffectSpecial(m_Sender, m_Special);
    }

    public override void Upgrade()
    {
        base.Upgrade();

        m_AttackStatValue += m_BaseAttackStatValue * 0.1f;
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
        m_Sender.attackStat -= m_AttackStatValue;
        m_Sender.RemoveBuffIcon();

        EffectSystem.GetInstance().AddRemoveEffectSpecial(m_Sender, m_Special);

        return true;
    }

    public override void Stack(BaseEffect p_Effect)
    {
        base.Stack(p_Effect);

        m_DurationCounter = 0;
        AttackBuffEffect l_Effect = (AttackBuffEffect)p_Effect;

        m_Sender.attackStat += l_Effect.m_AttackStatValue;
        m_AttackStatValue += l_Effect.m_AttackStatValue;
    }
}
