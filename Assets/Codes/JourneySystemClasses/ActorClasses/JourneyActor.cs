using UnityEngine;
using System;

public enum ActorDirection
{
    NONE = -1,
    Right,
    Left,
    Up,
    Down
}

public class JourneyActor : MonoBehaviour
{
    #region Interface
    protected Animator  m_Animator = null;
    protected Transform m_Transform = null;
    protected ActorDirection m_ActorDirection = ActorDirection.Down;
    protected int m_SortingOrder = 0;

    [SerializeField]
    protected float     m_Speed = 5.0f;
    #endregion

    #region Interface
    public Transform myTransform
    {
        get
        {
            if (m_Transform == null)
            {
                m_Transform = transform;
            }
            return m_Transform;
        }
    }
    public Animator myAnimator
    {
        get
        {
            if (m_Animator == null)
            {
                m_Animator = GetComponentInChildren<Animator>();
            }
            return m_Animator;
        }
    }
    public ActorDirection direction
    {
        get { return m_ActorDirection; }
    }
    public int sortingOrder
    {
        get { return m_SortingOrder; }
    }

    public virtual void Awake()
    {
        m_Animator  = myAnimator;
        m_Transform = myTransform;
        m_SortingOrder = GetComponentInChildren<SpriteRenderer>().sortingOrder;
    }

    public virtual void Update()
    {
    }

    public void ApplyTo(Vector3 p_Target)
    {
        Vector2 l_Position = myTransform.position;
        Vector2 l_SpeakerPosition = p_Target;
        double l_Angle = Math.Atan2(l_Position.y - l_SpeakerPosition.y, l_Position.x - l_SpeakerPosition.x) / Math.PI * 180;
        l_Angle = (l_Angle < 0) ? l_Angle + 360 : l_Angle;

        if ((l_Angle > 315.0f && l_Angle < 360.0f) || (l_Angle > 0.0f && l_Angle < 45.0f))
        {
            m_Animator.SetTrigger("Left");
        }
        else if (l_Angle > 45.0f && l_Angle < 135.0f)
        {
            m_Animator.SetTrigger("Down");
        }
        else if (l_Angle > 135.0f && l_Angle < 225.0f)
        {
            m_Animator.SetTrigger("Right");
        }
        else if (l_Angle > 225.0f && l_Angle < 315.0f)
        {
            m_Animator.SetTrigger("Up");
        }
    }

    public virtual void StartLogic()
    {
        enabled = true;
        m_Animator.SetBool("StopLogic", false);
    }

    public virtual void StopLogic()
    {
        enabled = false;
        m_Animator.SetBool("StopLogic", true);
    }
    #endregion
}
