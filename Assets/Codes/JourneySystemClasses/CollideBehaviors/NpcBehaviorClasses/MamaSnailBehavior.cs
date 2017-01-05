using System.Collections.Generic;
using UnityEngine;

public class MamaSnailBehavior : BaseCollideBehavior
{
    private bool m_QuestAccepted = false;
    private bool m_QuestDeny = false;
    private bool m_QuestCompleted = false;

    [SerializeField]
    private string m_QuestId = string.Empty;

    [SerializeField]
    private string m_AskDialogId = string.Empty;

    [SerializeField]
    private string m_AcceptedDialogId = string.Empty;

    [SerializeField]
    private string m_DenyDialogId = string.Empty;

    [SerializeField]
    private string m_CompletedDialogId = string.Empty;

    [SerializeField]
    private string m_CommonDialogId = string.Empty;

    [SerializeField]
    private ActionStruct m_AddingQuestAction;

    [SerializeField]
    private ActionStruct m_DenyQuestAction;

    [SerializeField]
    private ActionStruct m_CompletedQuestAction;

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

        m_JourneyActor.ApplyTo(p_Sender.myTransform.position);
        m_JourneyActor.StopLogic();

        if (m_QuestCompleted)
        {
            JourneySystem.GetInstance().StartDialog(m_CommonDialogId, new List<ActionStruct>() { });
            return;
        }
        
        if (m_QuestDeny)
        {
            JourneySystem.GetInstance().StartDialog(m_DenyDialogId, new List<ActionStruct>() { m_AddingQuestAction });
        }
        else if (m_QuestAccepted && !QuestSystem.GetInstance().HasCompleted(m_QuestId))
        {
            JourneySystem.GetInstance().StartDialog(m_AcceptedDialogId, new List<ActionStruct>());
        }
        else if (!QuestSystem.GetInstance().HasQuest(m_QuestId))
        {
            JourneySystem.GetInstance().StartDialog(m_AskDialogId, new List<ActionStruct>() { m_AddingQuestAction, m_DenyQuestAction });
        }
        else if (QuestSystem.GetInstance().HasCompleted(m_QuestId))
        {
            JourneySystem.GetInstance().StartDialog(m_CompletedDialogId, new List<ActionStruct>() { m_CompletedQuestAction });
            m_QuestCompleted = true;
        }
    }

    public override void StopAction()
    {
        base.StopAction();

        m_JourneyActor.StartLogic();
    }

    public void QuestAccept()
    {
        m_QuestAccepted = true;
        m_QuestDeny = false;
    }

    public void QuestDeny()
    {
        m_QuestDeny = true;
        m_QuestAccepted = false;
    }
}
