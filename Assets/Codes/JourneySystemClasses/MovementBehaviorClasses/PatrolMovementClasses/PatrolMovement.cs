using UnityEngine;
using System.Collections.Generic;

public class PatrolMovement : BaseMovement
{
    #region Variables
    private List<PatrolPosition> m_Patrol = new List<PatrolPosition>();
    private int   m_CurrentPoint = 0;
    private float m_ElapsedTime = 0.0f;

    [SerializeField]
    private Transform m_PatrolPointsTransform;
    #endregion

    #region Interface
    public override void Awake()
    {
        base.Awake();

        for (int i = 0; i < m_PatrolPointsTransform.childCount; i++)
        {
            m_Patrol.Add(m_PatrolPointsTransform.GetChild(i).GetComponent<PatrolPosition>());
        }
    }

    public override void LogicStart()
    {
        base.LogicStart();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (m_Patrol.Count == 0)
        {
            return;
        }

        if (m_Patrol[m_CurrentPoint].isDelay)
        {
            m_ElapsedTime += Time.deltaTime;
            journeyActor.myAnimator.SetBool("IsWalking", false);
            if (m_Patrol[m_CurrentPoint].speed <= m_ElapsedTime)
            {
                m_ElapsedTime = 0.0f;
                PointIncrement();
            }
        }
        else
        {
            journeyActor.GoTo(m_Patrol[m_CurrentPoint].myPosition, m_Patrol[m_CurrentPoint].speed * Time.deltaTime);
            journeyActor.myAnimator.SetBool("IsWalking", true);

            if (journeyActor.myTransform.position == m_Patrol[m_CurrentPoint].myPosition)
            {
                PointIncrement();
            }
        }
    }

    public override void LogicStop()
    {
        base.LogicStop();

        journeyActor.myAnimator.SetBool("IsWalking", false);
    }
    #endregion

    #region Private
    private float GetWaitTime()
    {
        return Random.Range(1.0f, 2.5f);
    }

    private void PointIncrement()
    {
        m_CurrentPoint++;

        if (m_CurrentPoint >= m_Patrol.Count)
        {
            m_CurrentPoint = 0;
        }
    }

    [ContextMenu("GeneratePoints")]
    public void GeneratePointPositions()
    {
        for (int i = 0; i < m_Patrol.Count; i++)
        {
            GameObject l_GameObject = new GameObject();
            l_GameObject.transform.SetParent(transform);
            l_GameObject.transform.position = m_Patrol[i].myPosition;
        }
    }
    #endregion
}