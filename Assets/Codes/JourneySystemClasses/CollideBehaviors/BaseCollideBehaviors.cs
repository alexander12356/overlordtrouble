using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct ActionStruct
{
    public string id;
    public UnityEvent actionEvent;
}

public class BaseCollideBehavior : MonoBehaviour
{
    protected JourneyActor m_JourneyActor = null;

	public virtual void Awake()
    {
        m_JourneyActor = GetComponent<JourneyActor>();
    }

    public virtual void RunAction(JourneyActor p_Sender)
    {
    }

    public virtual void StopAction()
    {
    }
}
