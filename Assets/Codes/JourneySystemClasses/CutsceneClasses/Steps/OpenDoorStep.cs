using UnityEngine;

public class OpenDoorStep : BaseStep
{
    [SerializeField]
    FrontDoor m_Door = null;

    public override void StartStep()
    {
        base.StartStep();

        m_Door.closed = false;

        EndStep();
    }
}
