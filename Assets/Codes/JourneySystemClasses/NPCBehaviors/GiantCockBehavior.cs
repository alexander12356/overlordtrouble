using UnityEngine;
using System.Collections;

public class GiantCockBehavior : BaseMovement
{
    private float m_WaitTime = 0.0f;
    private float m_ElapsedTime = 0.0f;

    public override void Awake()
    {
        base.Awake();

        m_WaitTime = GetWaitTime();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        m_ElapsedTime += Time.deltaTime;

        if (m_ElapsedTime >= m_WaitTime)
        {
            m_ElapsedTime = 0.0f;
            journeyActor.myAnimator.SetTrigger("Gape");
            m_WaitTime = GetWaitTime();
        }
    }

    private float GetWaitTime()
    {
        return Random.Range(60.0f, 180.0f);
    }
}
