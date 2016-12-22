using UnityEngine;

public class AttackRangeEffect : BaseEffect
{
    private float[] m_AttackValue;

    public AttackRangeEffect(Special p_Special, float[] p_AttackValue) : base(p_Special)
    {
        id = "Attack";
        m_AttackValue = p_AttackValue;
    }

    public override void Run(IEffectInfluenced p_Sender, IEffectInfluenced p_Target)
    {
        base.Run(p_Sender, p_Target);

        float l_DamageValue = Random.Range(m_AttackValue[0], m_AttackValue[1]);
        BattleActor l_Sender = p_Sender as BattleActor;
        BattleActor l_Target = p_Target as BattleActor;

        DamageSystem.GetInstance().AddDamageValue(l_Sender, l_Target, l_DamageValue, m_Special.element);
    }
}
