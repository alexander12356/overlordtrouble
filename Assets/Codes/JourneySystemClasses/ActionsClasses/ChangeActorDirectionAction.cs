using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeActorDirectionAction : MonoBehaviour
{
    [SerializeField]
    private ActorDirection m_NewDirection = ActorDirection.NONE;

    [SerializeField]
    private JourneyActor m_TargetActor = null;

    public void Run()
    {
        m_TargetActor.SetDirection(m_NewDirection);
    }
}
