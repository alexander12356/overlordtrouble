using UnityEngine;

public class DestroyObjectStep : BaseStep
{
    [SerializeField]
    private JourneyActor m_TargetObject = null;

    public override void StartStep()
    {
        base.StartStep();

        m_TargetObject.Die();

        EndStep();
    }
}
