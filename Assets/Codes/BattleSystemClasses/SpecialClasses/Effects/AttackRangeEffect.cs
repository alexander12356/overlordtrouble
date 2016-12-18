using UnityEngine;

public class AttackRangeEffect : BaseEffect
{
    private float[] m_AttackValue;

    public AttackRangeEffect(Special p_Special, float[] p_AttackValue) : base(p_Special)
    {
        id = "Attack";
        m_AttackValue = p_AttackValue;
    }

    public override void Run(BattleActor p_Sender, BattleActor p_Target)
    {
        base.Run(p_Sender, p_Target);

        float l_DamageValue = Random.Range(m_AttackValue[0], m_AttackValue[1]);

        DamageSystem.GetInstance().AddDamageValue(p_Sender, p_Target, l_DamageValue, m_Special.element);
    }
}
