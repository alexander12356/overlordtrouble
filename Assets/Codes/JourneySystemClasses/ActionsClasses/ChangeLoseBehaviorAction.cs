using UnityEngine;

public class ChangeLoseBehaviorAction : MonoBehaviour
{
    [SerializeField]
    private JourneyEnemy m_JourneyEnemy;

    [SerializeField]
    private string m_LoseBehaviorId;

    public void Run()
    {
        m_JourneyEnemy.ChangeLoseBehavior(m_LoseBehaviorId);
    }
}
