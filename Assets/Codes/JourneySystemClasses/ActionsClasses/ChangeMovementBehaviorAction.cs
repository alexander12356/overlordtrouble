using UnityEngine;

public class ChangeMovementBehaviorAction : MonoBehaviour
{
    [SerializeField]
    private JourneyActor m_JourneyActor = null;

    [SerializeField]
    private string m_NewBehaviorId = "";

    public void Run()
    {
        m_JourneyActor.ChangeMovementBehavior(m_NewBehaviorId);
    }
}
