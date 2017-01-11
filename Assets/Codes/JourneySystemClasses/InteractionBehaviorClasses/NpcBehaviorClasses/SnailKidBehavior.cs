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

        if (QuestSystem.GetInstance().HasQuest(m_QuestId) && !QuestSystem.GetInstance().HasCompleted(m_QuestId))
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
}
