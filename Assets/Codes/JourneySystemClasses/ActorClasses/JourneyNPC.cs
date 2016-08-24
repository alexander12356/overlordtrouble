using UnityEngine;
using System;

public class JourneyNPC : JourneyActor
{
    #region Variables
    private CheckCollide     m_CheckCollide    = null;
    private BaseCollideBehaviors m_BaseCollideBehaviors = null;

    [SerializeField]
    private string m_NpcId = "TestNPC";

    [SerializeField]
    private BaseMovement m_BaseMovement = null;
    #endregion

    #region Interface
    public override void Awake()
    {
        base.Awake();

        m_BaseCollideBehaviors = GetComponent<BaseCollideBehaviors>();
        m_CheckCollide = GetComponentInChildren<CheckCollide>();

        m_CheckCollide.AddCollideEnterAction(m_BaseCollideBehaviors.EnterAction);
        m_CheckCollide.AddCollideExitAction(m_BaseCollideBehaviors.ExitAction);

        if (!m_BaseMovement)
        {
            Debug.LogWarning("Movement logic is null");
        }
    }

    public override void Update()
    {
        base.Update();

        if (m_BaseMovement)
        {
            m_BaseMovement.LogicUpdate();
        }
    }

    public override void StartLogic()
    {
        base.StartLogic();

        m_BaseMovement.LogicStart();
    }

    public override void StopLogic()
    {
        base.StopLogic();

        m_BaseMovement.LogicStop();
    }
    #endregion
}