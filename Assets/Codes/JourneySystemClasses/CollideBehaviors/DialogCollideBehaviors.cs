using UnityEngine;

using System;

public class DialogCollideBehaviors : BaseCollideBehaviors
{
    [SerializeField]
    private string m_DialogId = string.Empty;
    private JourneyPlayer m_JourneyPlayer = null;

    public override void Awake()
    {
        base.Awake();
    }

    public override void EnterAction(JourneyActor p_JourneyActor)
    {
        base.EnterAction(p_JourneyActor);

        m_JourneyPlayer = (JourneyPlayer)p_JourneyActor;
        m_JourneyPlayer.AddActiveButtonAction(StartDialog);
        m_JourneyPlayer.AddDisactiveButtonAction(EndDialog);
    }

    public override void ExitAction(JourneyActor p_JourneyActor)
    {
        base.ExitAction(p_JourneyActor);

        m_JourneyPlayer.RemoveActiveButtonAction(StartDialog);
        m_JourneyPlayer.RemoveDisactiveButtonAction(EndDialog);
        m_JourneyPlayer = null;
    }

    public virtual void EndDialog()
    {
        m_JourneyActor.StartLogic();
    }

    private void StartDialog()
    {
        if (m_JourneyPlayer.direction != GetMyObjectSide())
        {
            return;
        }

        JourneySystem.GetInstance().StartDialog(m_DialogId);
        m_JourneyActor.ApplyTo(m_JourneyPlayer.myTransform.position);
        m_JourneyActor.StopLogic();
    }

    private ActorDirection GetMyObjectSide()
    {
        Vector2 l_Position = m_JourneyPlayer.myTransform.position;
        Vector2 l_ThisPosition = m_JourneyActor.myTransform.position; 
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
