﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogListCollideBehaviors : BaseCollideBehavior
{
    private int m_CurrentDialogId = 0;

    [SerializeField]
    private List<string> m_DialogList = null;

    public override void RunAction(JourneyActor p_Sender)
    {
        base.RunAction(p_Sender);

        if (p_Sender.direction != GetMyObjectSide(p_Sender))
        {
            return;
        }

        JourneySystem.GetInstance().StartDialog(m_DialogList[m_CurrentDialogId], new List<ActionStruct>());

        m_JourneyActor.ApplyTo(p_Sender.myTransform.position);
        m_JourneyActor.StopLogic();

        if (m_DialogList.Count > m_CurrentDialogId + 1)
        {
            m_CurrentDialogId++;
        }
    }

    public override void StopAction()
    {
        base.StopAction();

        m_JourneyActor.StartLogic();
    }
}