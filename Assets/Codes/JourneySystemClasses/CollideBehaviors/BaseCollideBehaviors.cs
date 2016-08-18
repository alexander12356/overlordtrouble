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

    public virtual void EnterAction(JourneyPlayer p_JourneyPlayer)
    {
    }

    public virtual void ExitAction(JourneyPlayer p_JourneyPlayer)
    {
    }
}
