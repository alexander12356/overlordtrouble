using UnityEngine;
using System.Collections.Generic;

public class PatrolMovement : BaseMovement
{
    #region Variables
    [System.Serializable]
    private struct PatrolPosition
    {
        public Vector2 position;
        public float   speed;
        public bool    isDelay;

        public PatrolPosition(Vector2 p_Position, bool p_IsDelay, float p_Speed)
        {
            position = p_Position;
            isDelay = p_IsDelay;
            speed = p_Speed;
        }
    }
    [SerializeField]
    private List<PatrolPosition> m_Patrol = null;

    private int   m_CurrentPoint = 0;
    private float m_ElapsedTime = 0.0f;
    #endregion

    #region Interface
    public override void Awake()
    {
        base.Awake();
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
                m_CurrentPoint++;
            }
        }
        else
        {
            journeyActor.GoTo(m_Patrol[m_CurrentPoint].position, m_Patrol[m_CurrentPoint].speed * Time.deltaTime);
            journeyActor.myAnimator.SetBool("IsWalking", true);

            if ((Vector2)journeyActor.myTransform.localPosition == m_Patrol[m_CurrentPoint].position)
            {
                m_CurrentPoint++;

                if (m_CurrentPoint >= m_Patrol.Count)
                {
                    m_CurrentPoint = 0;
                }
            }
        }

        journeyActor.UpdateSortingLayer();
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

    [ContextMenu("GeneratePoints")]
    public void GeneratePointPositions()
    {
        for (int i = 0; i < m_Patrol.Count; i++)
        {
            GameObject l_GameObject = new GameObject();
            l_GameObject.transform.SetParent(transform);
            l_GameObject.transform.position = m_Patrol[i].position;
        }
    }
    #endregion
}