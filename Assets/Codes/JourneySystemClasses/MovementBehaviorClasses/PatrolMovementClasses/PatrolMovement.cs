using UnityEngine;
using System.Collections.Generic;

public class PatrolMovement : BaseMovement
{
    #region Variables
    private List<PatrolPosition> m_Patrol = new List<PatrolPosition>();
    private int m_CurrentPoint = 0;
    private float m_ElapsedTime = 0.0f;
    private bool m_PathBlocked = false;

    [SerializeField]
    private Transform m_PatrolPointsTransform;
    #endregion
    
    public override void Awake()
    {
        base.Awake();

        for (int i = 0; i < m_PatrolPointsTransform.childCount; i++)
        {
            m_Patrol.Add(m_PatrolPointsTransform.GetChild(i).GetComponent<PatrolPosition>());
        }
    }
    
    #region LOGIC
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (m_Patrol.Count == 0 || m_PathBlocked)
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

    public void OnTriggerEnter2D(Collider2D p_Other)
    {
        //TODO надо избавиться от этого выделив новый слой
        if (p_Other.tag == "Player")
        {
            m_PathBlocked = true;
            journeyActor.myAnimator.SetBool("IsWalking", false);
        }
    }

    public void OnTriggerExit2D(Collider2D p_Other)
    {
        if (p_Other.tag == "Player")
        {
            m_PathBlocked = false;
        }
    }
}