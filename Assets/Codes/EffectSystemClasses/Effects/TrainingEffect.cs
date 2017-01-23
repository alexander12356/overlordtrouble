public class TrainingEffect : BaseEffect
{
    private int m_ExperienceValue = 0;

    public TrainingEffect(Special p_Special, int p_ExperienceValue) : base (p_Special)
    {
        m_ExperienceValue = p_ExperienceValue;
    }

    public override void Run(IEffectInfluenced p_Sender, IEffectInfluenced p_Target)
    {
        base.Run(p_Sender, p_Target);

        PlayerData.GetInstance().AddExperience(m_ExperienceValue);
    }
}
