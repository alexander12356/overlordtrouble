using System;

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
    public JourneyActor journeyActor
    {
        get { return m_JourneyActor; }
        set { m_JourneyActor = value; }
    }

	public virtual void Awake()
    {
    }

    public virtual void RunAction(JourneyActor p_Sender)
    {
    }

    public virtual void StopAction()
    {
    }
}
