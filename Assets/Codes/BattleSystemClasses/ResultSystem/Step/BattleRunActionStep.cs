using UnityEngine.Events;

public class BattleRunActionStep : BattleBaseStep
{
    private UnityEvent m_UnityEvent = null;

    public BattleRunActionStep(UnityAction p_Action)
    {
        m_UnityEvent = new UnityEvent();
        m_UnityEvent.AddListener(p_Action);
    }

    public override void RunStep()
    {
        base.RunStep();

        m_UnityEvent.Invoke();

        ResultSystem.GetInstance().NextStep();
    }
}
