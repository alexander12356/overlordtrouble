using UnityEngine;

public class ChangeInteractBehaviorAction : MonoBehaviour
{
    [SerializeField]
    private JourneyActor m_JourneyActor = null;

    [SerializeField]
    private string m_NewBehaviorId = "";

    public void Run()
    {
        m_JourneyActor.ChangeInteractionBehavior(m_NewBehaviorId);
    }
}
