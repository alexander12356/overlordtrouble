public class BattlePlayEffectStep : BattleBaseStep
{
    private AttackEffect m_AttackEffect = null;

    public BattlePlayEffectStep(AttackEffect p_Effect)
    {
        m_AttackEffect = p_Effect;
    }

    public override void RunStep()
    {
        base.RunStep();

        m_AttackEffect.PlayEffect();
    }
}
