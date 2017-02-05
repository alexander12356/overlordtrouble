using UnityEngine;

public class DestroyActorStep : BaseStep
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
