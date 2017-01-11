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

    protected ActorDirection GetMyObjectSide(JourneyActor p_OtherActor)
    {
        Vector2 l_Position = p_OtherActor.pivotTransform.position;
        Vector2 l_ThisPosition = m_JourneyActor.pivotTransform.position;
        double l_Angle = Math.Atan2(l_Position.y - l_ThisPosition.y, l_Position.x - l_ThisPosition.x) / Math.PI * 180;
        l_Angle = (l_Angle < 0) ? l_Angle + 360 : l_Angle;

        if ((l_Angle > 315.0f && l_Angle < 360.0f) || (l_Angle > 0.0f && l_Angle < 45.0f))
        {
            return ActorDirection.Left;
        }
        else if (l_Angle > 45.0f && l_Angle < 135.0f)
        {
            return ActorDirection.Down;
        }
        else if (l_Angle > 135.0f && l_Angle < 225.0f)
        {
            return ActorDirection.Right;
        }
        else if (l_Angle > 225.0f && l_Angle < 315.0f)
        {
            return ActorDirection.Up;
        }

        return ActorDirection.NONE;
    }
}
