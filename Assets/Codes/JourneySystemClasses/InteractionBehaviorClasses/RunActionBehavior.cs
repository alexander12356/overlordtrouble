using UnityEngine;

public class RunActionBehavior : BaseCollideBehavior
{
    [SerializeField]
    private ActionStruct m_ActionStruct;

    public override void RunAction(JourneyActor p_Sender)
    {
        base.RunAction(p_Sender);

        m_ActionStruct.actionEvent.Invoke();
    }
}
