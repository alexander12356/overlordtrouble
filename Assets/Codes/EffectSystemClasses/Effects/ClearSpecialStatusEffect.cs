public class ClearSpecialStatusEffect : BaseEffect
{
    private float m_BonusCount = 1.0f;
    private float m_BaseSpecialPoints = 0.0f;

	public ClearSpecialStatusEffect(Special p_Special) : base(p_Special)
    {
        m_BaseSpecialPoints = SpecialDataBase.GetInstance().GetSpecialData(m_Special.id).sp;
    }

    public override void Run(IEffectInfluenced p_Sender, IEffectInfluenced p_Target)
    {
        base.Run(p_Sender, p_Target);

        if (m_BonusCount > 0.0f)
        {
            float l_BonusHP = m_BonusCount * (p_Sender.level / 2.0f);
            p_Sender.health += l_BonusHP;
            DamageSystem.GetInstance().AddBonuses(BonusType.Health, l_BonusHP);
        }

        BattleActor l_Sender = p_Sender as BattleActor;

        if (l_Sender.HasEffect("Stun"))
        {
            l_Sender.EffectEndImmediately("Stun");

            DamageSystem.GetInstance().AddRestoration(l_Sender, RestorationType.DebuffClear, 1.0f);
        }
        else
        {
            DamageSystem.GetInstance().AddRestoration(l_Sender, RestorationType.DebuffClear, 0.0f);
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();

        m_BonusCount += 1.0f;
    }
}
