using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct ActionStruct
{
    public string id;
    public UnityEvent actionEvent;
}

public class DialogAnswerCollideBehaviors : DialogCollideBehaviors
{
    private DialogQuestionPanel m_DialogPanel = null;

    [SerializeField]
    List<ActionStruct> m_YesEvent = null;

    public override void RunAction(JourneyActor p_Sender)
    {
        if (p_Sender.direction != GetMyObjectSide(p_Sender))
        {
            return;
        }

        m_DialogPanel = JourneySystem.GetInstance().StartQuestionDialog(m_DialogId, m_YesEvent);

        m_JourneyActor.ApplyTo(p_Sender.myTransform.position);
        m_JourneyActor.StopLogic();
    }

    public void DialogClose()
    {
        m_DialogPanel.DialogClose();
    }
}
