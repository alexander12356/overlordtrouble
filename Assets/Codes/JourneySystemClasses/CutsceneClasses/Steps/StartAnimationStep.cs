using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimationStep : BaseStep
{
    [SerializeField]
    private AnimationObject m_AnimationObject = null;

    [SerializeField]
    private string m_TriggerId = "";

    public override void StartStep()
    {
        base.StartStep();

        m_AnimationObject.SetState(m_TriggerId);
        m_AnimationObject.AddEndAnimationAction(EndStep);
    }
}
