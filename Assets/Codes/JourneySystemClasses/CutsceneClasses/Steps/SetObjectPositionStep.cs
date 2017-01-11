using UnityEngine;

public class SetObjectPositionStep : BaseStep
{
    [SerializeField]
    JourneyActor m_TargetActor = null;


    public override void StartStep()
    {
        base.StartStep();

        m_TargetActor.transform.position = transform.position;
        m_TargetActor.UpdateSortingLayer();

        EndStep();
    }
}
