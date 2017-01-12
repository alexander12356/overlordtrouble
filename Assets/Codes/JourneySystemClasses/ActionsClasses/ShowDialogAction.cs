using System.Collections.Generic;

using UnityEngine;

public class ShowDialogAction : MonoBehaviour
{
    [SerializeField]
    private string m_DialogId = string.Empty;

    [SerializeField]
    private JourneyPlayer m_JourneyPlayer = null;

    [SerializeField]
    private JourneyActor m_TargetActor = null;

    public void StartDialog()
    {
        DialogPanel l_DialogPanel = JourneySystem.GetInstance().StartDialog(m_DialogId, new List<ActionStruct>());

        m_TargetActor.ApplyTo(m_JourneyPlayer.myTransform.position);
        m_JourneyPlayer.ApplyTo(m_TargetActor.myTransform.position);
        m_TargetActor.StopLogic();
    }
}
