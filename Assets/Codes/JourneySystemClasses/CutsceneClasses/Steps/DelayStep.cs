using UnityEngine;
using System.Collections;

public class DelayStep : BaseStep
{
    private float m_ElapsedTime = 0.0f;

    [SerializeField]
    private float m_DelayTime = 0.0f;

    public override void UpdateStep()
    {
        base.UpdateStep();

        m_ElapsedTime += Time.deltaTime;
        if (m_ElapsedTime >= m_DelayTime)
        {
            EndStep();
        }
    }
}
