using System.Collections.Generic;

using UnityEngine;

public class StartDialogAction : MonoBehaviour
{
    [SerializeField]
    private string m_DialogId = string.Empty;

    [SerializeField]
    private JourneyPlayer m_JourneyPlayer = null;

    [SerializeField]
    private JourneyActor m_TargetActor = null;

    [SerializeField]
    private ActionStruct m_Action;

    public void StartDialog()
    {
        DialogPanel l_DialogPanel = JourneySystem.GetInstance().StartDialog(m_DialogId, new List<ActionStruct>() { m_Action } );

        m_TargetActor.ApplyTo(m_JourneyPlayer);
        m_JourneyPlayer.ApplyTo(m_TargetActor);
        m_TargetActor.StopLogic();
    }
}
