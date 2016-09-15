using UnityEngine;
using System.Collections;
using System;

public class BaseCollideBehaviors : MonoBehaviour
{
    protected JourneyActor m_JourneyActor = null;

	public virtual void Awake()
    {
        m_JourneyActor = GetComponent<JourneyActor>();
    }

    public virtual void EnterAction(JourneyActor p_JourneyActor)
    {
    }

    public virtual void ExitAction(JourneyActor p_JourneyActor)
    {
    }
}
