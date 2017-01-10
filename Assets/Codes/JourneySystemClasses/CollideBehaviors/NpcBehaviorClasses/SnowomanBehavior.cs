using System.Collections.Generic;

using UnityEngine;

public class SnowomanBehavior : BaseNpcBehavior
{
    private int m_CurrentTaskDialogId = 0;

    [SerializeField]
    private List<string> m_TaskDialogs = null;

    [SerializeField]
    private string m_CompleteDialog = string.Empty;

    [SerializeField]
    private string m_PreCompleteDialog = string.Empty;

    [SerializeField]
    private ActionStruct m_CompleteDialogAction;

    [SerializeField]
    private string m_CommonDialog = null;

    public void Start()
    {
        state = "QuestNotAdded";
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

        switch (state)
        {
            case "QuestNotAdded":
                if (QuestSystem.GetInstance().HasQuest("ReadingLibrary") && QuestSystem.GetInstance().HasCompleted("ReadingLibrary"))
                {
                    state = "QuestComplete";
                    JourneySystem.GetInstance().StartDialog(m_PreCompleteDialog, new List<ActionStruct>() { m_CompleteDialogAction });
                    return;
                }

                CountQuest l_Quest = new CountQuest("ReadingLibrary", 8);
                QuestSystem.GetInstance().AddQuest(l_Quest);

                JourneySystem.GetInstance().StartDialog(m_TaskDialogs[m_CurrentTaskDialogId]);

                state = "QuestAdded";

                break;
            case "QuestAdded":
                if (QuestSystem.GetInstance().HasCompleted("ReadingLibrary"))
                {
                    state = "QuestComplete";
                    JourneySystem.GetInstance().StartDialog(m_CompleteDialog, new List<ActionStruct>() { m_CompleteDialogAction });
                    return;
                }

                if (m_TaskDialogs.Count > m_CurrentTaskDialogId + 1)
                {
                    m_CurrentTaskDialogId++;
                }

                JourneySystem.GetInstance().StartDialog(m_TaskDialogs[m_CurrentTaskDialogId]);
                break;
            case "QuestComplete":
                JourneySystem.GetInstance().StartDialog(m_CommonDialog);
                break;
        }
    }

    public override void StopAction()
    {
        base.StopAction();

        m_JourneyActor.StartLogic();
    }
}
