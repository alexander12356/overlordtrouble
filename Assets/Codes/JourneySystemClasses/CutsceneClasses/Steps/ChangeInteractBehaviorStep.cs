using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeInteractBehaviorStep : BaseStep
{
    [SerializeField]
    private JourneyActor m_JourneyActor;

    [SerializeField]
    private string m_NewBehaviorId = "";

    public override void StartStep()
    {
        base.StartStep();

        m_JourneyActor.ChangeInteractionBehavior(m_NewBehaviorId);
        EndStep();
    }
}
