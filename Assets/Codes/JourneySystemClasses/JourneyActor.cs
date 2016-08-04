using UnityEngine;

public class JourneyActor : MonoBehaviour
{
    #region Interface
    protected Animator  m_Animator = null;
    protected Vector3   m_CurrentSpeed = Vector3.zero;
    protected float     m_Speed = 5.0f;
    protected Transform m_Transform = null;
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

    public virtual void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Transform = transform;
    }

    public virtual void Update()
    {
    }
    #endregion
}
