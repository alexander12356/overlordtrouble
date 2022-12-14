using UnityEngine;
using System.Collections.Generic;

public class DialogStep : BaseStep
{
    [SerializeField]
    private string m_DialogId = string.Empty;

    [SerializeField]
    private JourneyActor m_SenderActor = null;

    [SerializeField]
    private JourneyActor m_TargetActor = null;

    [SerializeField]
    private List<ActionStruct> m_ActionStruct = null;

    public override void StartStep()
    {
        base.StartStep();

        DialogPanel l_DialogPanel = JourneySystem.GetInstance().StartDialog(m_DialogId, m_ActionStruct);

        l_DialogPanel.AddPopAction(EndStep);

        m_TargetActor.ApplyTo(m_SenderActor);
        m_SenderActor.ApplyTo(m_TargetActor);
        m_TargetActor.StopLogic();
    }

    public override void EndStep()
    {
        base.EndStep();
        
        m_TargetActor.StartLogic();
    }
}
