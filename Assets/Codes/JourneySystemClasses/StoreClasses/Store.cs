using UnityEngine;
using System.Collections;

public class Store : JourneyActor
{
    private CheckCollide m_CheckCollide;
    private BaseCollideBehaviors m_BaseCollideBehaviors;

    public override void Awake()
    {
        base.Awake();

        m_CheckCollide = GetComponentInChildren<CheckCollide>();
        m_BaseCollideBehaviors = GetComponent<BaseCollideBehaviors>();

        m_CheckCollide.AddCollideEnterAction(m_BaseCollideBehaviors.EnterAction);
        m_CheckCollide.AddCollideExitAction(m_BaseCollideBehaviors.ExitAction);
    }
}
