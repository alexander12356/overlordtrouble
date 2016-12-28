using UnityEngine;

public class SetObjectPositionStep : BaseStep
{
    [SerializeField]
    JourneyActor m_TargetActor = null;

    [SerializeField]
    Vector3 m_NewPosition = Vector3.zero;


    public override void StartStep()
    {
        base.StartStep();

        m_TargetActor.transform.localPosition = m_NewPosition;
        m_TargetActor.UpdateSortingLayer();

        EndStep();
    }
}
