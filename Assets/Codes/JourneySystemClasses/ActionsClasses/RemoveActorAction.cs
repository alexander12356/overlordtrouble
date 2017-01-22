using UnityEngine;

public class RemoveActorAction : MonoBehaviour
{
    [SerializeField]
    private JourneyActor m_JourneyActor;

    public void Run()
    {
        m_JourneyActor.Die();
    }
}
