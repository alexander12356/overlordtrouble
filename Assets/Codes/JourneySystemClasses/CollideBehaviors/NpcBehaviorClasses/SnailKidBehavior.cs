using System;
using System.Collections.Generic;

using UnityEngine;

public class SnailKidBehavior : BaseCollideBehavior
{
    [SerializeField]
    private string m_DialogId = string.Empty;

    [SerializeField]
    private string m_QuestId = string.Empty;

    [SerializeField]
    private string m_QuestDialogId = string.Empty;

    [SerializeField]
    private ActionStruct m_CompleteQuestAction;

    public override void Awake()
    {
        base.Awake();
    }

    public override void RunAction(JourneyActor p_Sender)
    {
        base.RunAction(p_Sender);

        if (p_Sender.direction != GetMyObjectSide(p_Sender))
        {
            return;
        }

        if (QuestSystem.GetInstance().HasQuest(m_QuestId))
        {
            QuestSystem.GetInstance().CompleteQuest(m_QuestId);
            JourneySystem.GetInstance().StartDialog(m_QuestDialogId, new List<ActionStruct>());

            if (m_CompleteQuestAction.id != "")
            {
                m_CompleteQuestAction.actionEvent.Invoke();
            }
        }
        else
        {
            JourneySystem.GetInstance().StartDialog(m_DialogId, new List<ActionStruct>());
        }

        m_JourneyActor.ApplyTo(p_Sender.myTransform.position);
        m_JourneyActor.StopLogic();
    }

    public override void StopAction()
    {
        base.StopAction();

        m_JourneyActor.StartLogic();
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
