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

        int l_IntRestoringValue = System.Convert.ToInt32(System.Math.Ceiling(m_SpecialPointsValue));

        p_Sender.specialPoints += l_IntRestoringValue;

        if (BattleSystem.IsInstance())
        {
            BattleActor l_Sender = p_Sender as BattleActor;
            DamageSystem.GetInstance().AddRestoration(l_Sender, RestorationType.Restoring, l_IntRestoringValue);
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();

        m_SpecialPointsValue += m_SpecialPointsValueBase * 0.1f;
    }
}
