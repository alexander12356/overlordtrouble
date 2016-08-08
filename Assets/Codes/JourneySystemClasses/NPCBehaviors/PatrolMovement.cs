﻿using UnityEngine;
using System.Collections.Generic;

public class PatrolMovement : BaseMovement
{
    #region Variables
    [System.Serializable]
    private struct PatrolPosition
    {
        public Vector2 position;
        public string  animationName;
    }
    [SerializeField]
    private List<PatrolPosition> m_Patrol = null;

    private int   m_CurrentPoint = 0;
    private float m_WaitTime = 0.0f;
    private float m_MovingTime = 2.0f;
    private float m_ElapsedTime = 0.0f;
    #endregion

    #region Interface
    public override void Awake()
    {
        base.Awake();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        m_ElapsedTime += Time.deltaTime;

        journeyActor.myTransform.position = Vector2.MoveTowards(journeyActor.myTransform.position, m_Patrol[m_CurrentPoint].position, m_MovingTime * Time.deltaTime);
        journeyActor.myAnimator.SetBool(m_Patrol[m_CurrentPoint].animationName, true);

        if ((Vector2)journeyActor.myTransform.position == m_Patrol[m_CurrentPoint].position)
        {
            journeyActor.myAnimator.SetBool(m_Patrol[m_CurrentPoint].animationName, false);
            m_CurrentPoint++;

            if (m_CurrentPoint >= m_Patrol.Count)
            {
                m_CurrentPoint = 0;
            }
        }
    }
    #endregion

    #region Private
    private float GetWaitTime()
    {
        return Random.Range(1.0f, 2.5f);
    }
    #endregion
}