using UnityEngine;

using System;
using System.Collections.Generic;

public class DialogCollideBehaviors : BaseCollideBehavior
{
    [SerializeField]
    protected string m_DialogId = string.Empty;

    [SerializeField]
    private List<ActionStruct> m_AnswerActionList = null;

    public override void Awake()
    {
        base.Awake();
    }

    public override void RunAction(JourneyActor p_Sender)
    {
        base.RunAction(p_Sender);

        JourneySystem.GetInstance().StartDialog(m_DialogId, m_AnswerActionList);

        m_JourneyActor.ApplyTo(p_Sender);
        m_JourneyActor.StopLogic();
    }

    public override void StopAction()
    {
        base.StopAction();

        m_JourneyActor.StartLogic();
    }
}
