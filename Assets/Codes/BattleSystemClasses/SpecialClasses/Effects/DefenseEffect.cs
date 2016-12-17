public class DefenseEffect : BaseEffect
{
    private float m_BaseDefenseValue = 0.0f;
    private float m_DefenseValue = 0.0f;
    private int m_Duration = 0;
    private int m_DurationCounter = 0;
    private BattleActor m_Sender = null;

    public DefenseEffect(Special p_Special, float p_DefenseValue, int p_Duration) : base(p_Special)
    {
        id = "Defense";
        m_DefenseValue = m_BaseDefenseValue = p_DefenseValue;
        m_Duration = p_Duration;
    }

    public override void Run(BattleActor p_Sender, BattleActor m_Target)
    {
        base.Run(p_Sender, m_Target);

        m_Sender = p_Sender;

        if (m_Sender.HasSpecial(m_Special.id))
        {
            m_Sender.StackEffect(m_Special.id, this);
        }
        else
        {
            m_Sender.defenseStat += m_DefenseValue;
            m_Sender.AddEffect(m_Special.id, this);
            m_Sender.AddBuff();
        }

        DamageSystem.GetInstance().AddEffectSpecial(p_Sender, m_Special);
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
        m_Sender.defenseStat -= m_DefenseValue;
        m_Sender.RemoveBuff();

        EffectSystem.GetInstance().AddRemoveEffectSpecial(m_Sender, m_Special);

        return true;
    }

    public override void Stack(BaseEffect p_Effect)
    {
        base.Stack(p_Effect);

        m_DurationCounter = 0;
        DefenseEffect l_Effect = (DefenseEffect)p_Effect;

        m_Sender.defenseStat += l_Effect.m_DefenseValue;
        m_DefenseValue += l_Effect.m_DefenseValue;
    }
}
