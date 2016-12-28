using UnityEngine;

public class DestroyObjectStep : BaseStep
{
    public GameObject m_TargetObject = null;

    public override void StartStep()
    {
        base.StartStep();

        Destroy(m_TargetObject);

        EndStep();
    }
}
