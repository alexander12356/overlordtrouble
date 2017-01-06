using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaSkeletonBehavior : BaseCollideBehavior
{
    private int m_CurrentTaskDialogId = 0;
    private bool m_TaskComplete = false;

    [SerializeField]
    private List<string> m_TaskDialogs = null;

    [SerializeField]
    private string m_CompleteDialog = null;

    [SerializeField]
    private string m_CommonDialog = null;

    public override void RunAction(JourneyActor p_Sender)
    {
        base.RunAction(p_Sender);

        if (p_Sender.direction != GetMyObjectSide(p_Sender))
        {
            return;
        }

        m_JourneyActor.ApplyTo(p_Sender.myTransform.position);
        m_JourneyActor.StopLogic();

        if (m_TaskComplete)
        {
            JourneySystem.GetInstance().StartDialog(m_CommonDialog, new List<ActionStruct>());

            return;
        }

        if (PlayerInventory.GetInstance().GetItemCount("Scoop") > 0)
        {
            m_TaskComplete = true;

            JourneySystem.GetInstance().StartDialog(m_CompleteDialog, new List<ActionStruct>());

            return;
        }

        JourneySystem.GetInstance().StartDialog(m_TaskDialogs[m_CurrentTaskDialogId], new List<ActionStruct>());

        if (m_TaskDialogs.Count > m_CurrentTaskDialogId + 1)
        {
            m_CurrentTaskDialogId++;
        }
    }

    public override void StopAction()
    {
        base.StopAction();

        m_JourneyActor.StartLogic();
    }
}
