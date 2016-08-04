using UnityEngine;

public class JourneyActor : MonoBehaviour
{
    protected Animator m_Animator = null;
    protected Vector3 m_CurrentSpeed = Vector3.zero;
    protected float m_Speed = 5.0f;

    public virtual void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public virtual void Update()
    {
    }
}
