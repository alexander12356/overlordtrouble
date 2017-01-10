using System.Collections.Generic;
using UnityEngine;

public class LibraryCupboardBehavior : BaseNpcBehavior
{
    [SerializeField]
    protected string m_DialogId = string.Empty;

    [SerializeField]
    private ActionStruct m_AddMonettAction;

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


        CheckHasQuest();
        StartDialog();
    }

    public override void StopAction()
    {
        base.StopAction();

        m_JourneyActor.StartLogic();
    }

    private void CheckHasQuest()
    {
        if (!QuestSystem.GetInstance().HasQuest("ReadingLibrary"))
        {
            CountQuest l_Quest = new CountQuest("ReadingLibrary", 8);
            QuestSystem.GetInstance().AddQuest(l_Quest);
        }
    }

    private void StartDialog()
    {
        if (state != "MonettCatchUp")
        {
            state = "MonettCatchUp";

            JourneySystem.GetInstance().StartDialog(m_DialogId, new List<ActionStruct>() { m_AddMonettAction });
            QuestSystem.GetInstance().CompleteQuest("ReadingLibrary");

            return;
        }

        JourneySystem.GetInstance().StartDialog(m_DialogId, new List<ActionStruct>());
    }
}
