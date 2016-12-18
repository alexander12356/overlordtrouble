public class AttackEffect : BaseEffect
{
    private float m_BaseAttackValue;
    private float m_AttackValue;

    public float attackValue
    {
        get { return m_AttackValue; }
    }

    public AttackEffect(Special p_Special, float p_AttackValue) : base(p_Special)
    {
        id = "Attack";
        m_BaseAttackValue = m_AttackValue = p_AttackValue;
    }

    public override void Run(BattleActor p_Sender, BattleActor p_Target)
    {
        base.Run(p_Sender, p_Target);

        DamageSystem.GetInstance().AddDamageValue(p_Sender, p_Target, m_AttackValue, m_Special.element);
    }

    public override void Upgrade()
    {
        base.Upgrade();

        m_AttackValue += m_BaseAttackValue * 0.1f;
    }

    public override string GetDescription()
    {
        return LocalizationDataBase.GetInstance().GetText("Effect:Attack", new string[] { m_BaseAttackValue.ToString() });
    }
}
