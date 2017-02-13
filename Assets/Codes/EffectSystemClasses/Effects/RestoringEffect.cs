public class RestoringEffect : BaseEffect
{
    private float m_SpecialPointsValue = 0.0f;
    private float m_SpecialPointsValueBase = 0.0f;

    public RestoringEffect(Special p_Special, float p_SpecialPointsValue) : base (p_Special)
    {
        m_SpecialPointsValue = m_SpecialPointsValueBase = p_SpecialPointsValue;
    }

    public override void Run(IEffectInfluenced p_Sender, IEffectInfluenced p_Target)
    {
        base.Run(p_Sender, p_Target);

        p_Sender.specialPoints += m_SpecialPointsValue;

        if (BattleSystem.IsInstance())
        {
            BattleActor l_Sender = p_Sender as BattleActor;
            DamageSystem.GetInstance().AddRestoration(l_Sender, RestorationType.Restoring, m_SpecialPointsValue);
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();

        m_SpecialPointsValue += m_SpecialPointsValueBase * 0.1f;
    }
}
