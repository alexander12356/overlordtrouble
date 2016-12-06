public class BattleShowPanelStep : BattleBaseStep
{
    private TextPanel m_TextPanel = null;

    public BattleShowPanelStep(TextPanel p_TextPanel)
    {
        m_TextPanel = p_TextPanel;
        m_TextPanel.AddButtonAction(ResultSystem.GetInstance().NextStep);
    }

    public override void RunStep()
    {
        base.RunStep();

        BattleSystem.GetInstance().ShowPanel(m_TextPanel);
    }
}
