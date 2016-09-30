using UnityEngine;
using System.Collections;

public class Store : JourneyActor
{
    private BaseCollideBehavior m_BaseCollideBehaviors;

    public override void Awake()
    {
        base.Awake();
        
        m_BaseCollideBehaviors = GetComponent<BaseCollideBehavior>();
    }
}
