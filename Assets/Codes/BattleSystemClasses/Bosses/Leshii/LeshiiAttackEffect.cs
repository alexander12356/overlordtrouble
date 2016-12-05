public class LeshiiAttackEffect : AttackEffect
{
    private PanelActionHandler m_PlayAction = null;

    public override void PlayEffect()
    {
        if (m_PlayAction != null)
        {
            m_PlayAction();
            m_PlayAction = null;
        }
    }

    public void AddPlayAction(PanelActionHandler p_Action)
    {
        m_PlayAction += p_Action;
    }
}
