using UnityEngine;
using System.Collections;
using System;

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
