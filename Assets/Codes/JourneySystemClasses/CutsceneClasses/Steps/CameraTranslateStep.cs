using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTranslateStep : BaseStep
{
    private Vector3 m_TargetPosition = Vector3.zero;
    private int m_Index = 0;

    [SerializeField]
    private CameraFollow m_CameraFollow = null;

    [SerializeField]
    private List<Vector3> m_TranslatePositionList = new List<Vector3>();

    [SerializeField]
    private float m_Speed = 1.0f;

    public override void StartStep()
    {
        base.StartStep();

        m_CameraFollow.isFollow = false;
        m_TargetPosition = m_CameraFollow.transform.position + m_TranslatePositionList[0];
    }

    public override void UpdateStep()
    {
        base.UpdateStep();

        
        m_CameraFollow.transform.position = Vector3.MoveTowards(m_CameraFollow.transform.position, m_TargetPosition, Time.deltaTime * m_Speed);

        if (m_CameraFollow.transform.position == m_TargetPosition)
        {
            m_Index++;

            if (m_Index < m_TranslatePositionList.Count)
            {
                m_TargetPosition = m_CameraFollow.transform.position + m_TranslatePositionList[m_Index];
            }
            else
            {
                EndStep();
            }
            
        }
    }

    public override void EndStep()
    {
        base.EndStep();

        m_CameraFollow.isFollow = true;
    }
}
