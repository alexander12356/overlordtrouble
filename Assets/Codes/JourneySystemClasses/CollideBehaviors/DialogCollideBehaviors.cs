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

    public override void EnterAction(JourneyPlayer p_JourneyPlayer)
    {
        base.EnterAction(p_JourneyPlayer);

        m_JourneyPlayer = p_JourneyPlayer;
        p_JourneyPlayer.AddActiveButtonAction(StartDialog);
        p_JourneyPlayer.AddDisactiveButtonAction(EndDialog);
    }

    public override void ExitAction(JourneyPlayer p_JourneyPlayer)
    {
        base.ExitAction(p_JourneyPlayer);

        m_JourneyPlayer = null;
        p_JourneyPlayer.RemoveActiveButtonAction(StartDialog);
        p_JourneyPlayer.RemoveDisactiveButtonAction(EndDialog);
    }

    public virtual void EndDialog()
    {
        m_JourneyActor.StartLogic();
    }

    private void StartDialog()
    {
        Debug.Log("PlayerDirection: " + m_JourneyPlayer.direction + ", ObjectSide: " + GetMyObjectSide());

        if (m_JourneyPlayer.direction != GetMyObjectSide())
        {
            return;
        }

        JourneySystem.GetInstance().StartDialog(m_DialogId);
        m_JourneyActor.ApplyTo(m_JourneyPlayer.myTransform.position);
        m_JourneyActor.StopLogic();
    }

    private JourneyActorDirection GetMyObjectSide()
    {
        Vector2 l_Position = m_JourneyPlayer.myTransform.position;
        Vector2 l_ThisPosition = m_JourneyActor.myTransform.position; 
        double l_Angle = Math.Atan2(l_Position.y - l_ThisPosition.y, l_Position.x - l_ThisPosition.x) / Math.PI * 180;
        l_Angle = (l_Angle < 0) ? l_Angle + 360 : l_Angle;

        if ((l_Angle > 315.0f && l_Angle < 360.0f) || (l_Angle > 0.0f && l_Angle < 45.0f))
        {
            return JourneyActorDirection.Left;
        }
        else if (l_Angle > 45.0f && l_Angle < 135.0f)
        {
            return JourneyActorDirection.Down;
        }
        else if (l_Angle > 135.0f && l_Angle < 225.0f)
        {
            return JourneyActorDirection.Right;
        }
        else if (l_Angle > 225.0f && l_Angle < 315.0f)
        {
            return JourneyActorDirection.Up;
        }

        return JourneyActorDirection.NONE;
    }
}
