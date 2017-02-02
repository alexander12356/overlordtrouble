using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeActorDirection : BaseStep
{
    [SerializeField]
    private ActorDirection m_Direction = ActorDirection.NONE;

    [SerializeField]
    private JourneyActor m_JourneyActor = null;

    public override void StartStep()
    {
        base.StartStep();

        m_JourneyActor.SetDirection(m_Direction);

        EndStep();
    }

}
