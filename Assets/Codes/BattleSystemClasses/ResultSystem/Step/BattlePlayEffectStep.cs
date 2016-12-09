public class BattlePlayEffectStep : BattleBaseStep
{
    private VisualEffect m_AttackEffect = null;

    public BattlePlayEffectStep(VisualEffect p_Effect)
    {
        m_AttackEffect = p_Effect;
    }

    public override void RunStep()
    {
        base.RunStep();

        m_AttackEffect.PlayEffect();
    }
}
