public class AttackEffect : BaseEffect
{
    private float m_BaseAttackValue;
    private float m_AttackValue;
    private Element m_Element;

    public float attackValue
    {
        get { return m_AttackValue; }
    }
    public Element element
    {
        get { return m_Element; }
    }

    public AttackEffect(float p_AttackValue, Element p_Element)
    {
        m_BaseAttackValue = m_AttackValue = p_AttackValue;
        m_Element = p_Element;
    }

    public override void Run(BattleActor p_Sender, BattleActor m_Target)
    {
        base.Run(p_Sender, m_Target);

        DamageSystem.GetInstance().AddDamageValue(m_AttackValue);
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
