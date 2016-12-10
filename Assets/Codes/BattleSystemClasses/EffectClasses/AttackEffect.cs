public class AttackEffect : BaseEffect
{
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
        m_AttackValue = p_AttackValue;
        m_Element = p_Element;
    }
}
