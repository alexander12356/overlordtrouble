﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldWellBehavior : BaseNpcBehavior
{
    private int m_CurrentDialogId = 0;
    [SerializeField]
    public string m_CoopDialogId = string.Empty;

    [SerializeField]
    public ActionStruct m_CoopAction;

    [SerializeField]
    public string m_CommonDialogId = string.Empty;

    public override void RunAction(JourneyActor p_Sender)
    {
        base.RunAction(p_Sender);

        if (p_Sender.direction != GetMyObjectSide(p_Sender))
        {
            return;
        }

        m_JourneyActor.ApplyTo(p_Sender.myTransform.position);
        m_JourneyActor.StopLogic();

        if (state != "CoopCatched")// !m_CoopCatched)
        {
            JourneySystem.GetInstance().StartDialog(m_CoopDialogId, new List<ActionStruct>() { m_CoopAction });
            state = "CoopCatched";

            return;
        }

        JourneySystem.GetInstance().StartDialog(m_CommonDialogId, new List<ActionStruct>());
    }

    public override void StopAction()
    {
        base.StopAction();

        m_JourneyActor.StartLogic();
    }
}