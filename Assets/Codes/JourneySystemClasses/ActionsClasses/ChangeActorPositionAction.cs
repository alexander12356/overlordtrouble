using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeActorPositionAction : MonoBehaviour
{
    [SerializeField]
    private JourneyActor m_Actor = null;

    [SerializeField]
    private Transform m_NewTransform = null;

    public void Run()
    {
        m_Actor.myTransform.position = m_NewTransform.position;
        m_Actor.UpdateSortingLayer();
    }
}
