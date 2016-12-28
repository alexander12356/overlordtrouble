using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateJourneyActor : MonoBehaviour
{
    [SerializeField]
    private JourneyNPC m_Target = null;

    public void Activate()
    {
        m_Target.enabled = true;
    }
}
