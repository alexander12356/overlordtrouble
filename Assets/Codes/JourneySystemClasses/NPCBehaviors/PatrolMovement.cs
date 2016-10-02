using UnityEngine;
using System.Collections.Generic;

public class PatrolMovement : BaseMovement
{
    #region Variables
    [System.Serializable]
    private struct PatrolPosition
    {
        public Vector2 position;
        public string  animationName;
        public float   speed;

        public PatrolPosition(Vector2 p_Position, string p_AnimationName, float p_Speed)
        {
            position = p_Position;
            animationName = p_AnimationName;
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

        journeyActor.myAnimator.SetBool(m_Patrol[m_CurrentPoint].animationName, true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (m_Patrol.Count == 0)
        {
            return;
        }

        if (m_Patrol[m_CurrentPoint].animationName.Contains("Delay"))
        {
            m_ElapsedTime += Time.deltaTime;
            journeyActor.myAnimator.SetTrigger(m_Patrol[m_CurrentPoint].animationName.Remove(0, 5));
            if (m_Patrol[m_CurrentPoint].speed <= m_ElapsedTime)
            {
                m_ElapsedTime = 0.0f;
                m_CurrentPoint++;
            }
        }
        else
        {
            journeyActor.myTransform.localPosition = Vector2.MoveTowards(journeyActor.myTransform.localPosition, m_Patrol[m_CurrentPoint].position, m_Patrol[m_CurrentPoint].speed * Time.deltaTime);
            journeyActor.myAnimator.SetBool(m_Patrol[m_CurrentPoint].animationName, true);

            if ((Vector2)journeyActor.myTransform.localPosition == m_Patrol[m_CurrentPoint].position)
            {
                journeyActor.myAnimator.SetBool(m_Patrol[m_CurrentPoint].animationName, false);
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

        journeyActor.myAnimator.SetBool(m_Patrol[m_CurrentPoint].animationName, false);
    }
    #endregion

    #region Private
    private float GetWaitTime()
    {
        return Random.Range(1.0f, 2.5f);
    }
    #endregion
}