public class ClearSpecialStatus : BaseEffect
{
    private float m_BonusCount = 1.0f;
    private float m_BaseSpecialPoints = 0.0f;

	public ClearSpecialStatus(Special p_Special) : base(p_Special)
    {
        m_BaseSpecialPoints = SpecialDataBase.GetInstance().GetSpecialData(m_Special.id).sp;
    }

    public override void Run(BattleActor p_Sender, BattleActor p_Target)
    {
        base.Run(p_Sender, p_Target);

        if (m_BonusCount > 0.0f)
        {
            float l_BonusHP = m_BonusCount * (p_Sender.level / 2.0f);
            p_Sender.health += l_BonusHP;
            DamageSystem.GetInstance().AddBonuses(BonusType.Health, l_BonusHP);
        }

        if (p_Sender.HasEffect("Stun"))
        {
            p_Sender.EffectEndImmediately("Stun");

            DamageSystem.GetInstance().AddRestoration(p_Sender, RestorationType.DebuffClear, 1.0f);
        }
        else
        {
            DamageSystem.GetInstance().AddRestoration(p_Sender, RestorationType.DebuffClear, 0.0f);
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();

        m_BonusCount += 1.0f;
    }
}
