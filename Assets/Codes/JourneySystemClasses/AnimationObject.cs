using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationObject : MonoBehaviour
{
    private Animator m_Animator = null;
    private UnityEvent m_UnityEvent = new UnityEvent();

    [SerializeField]
    private string m_Id;

    [SerializeField]
    private string m_Trigger = "";

    public string id
    {
        get { return m_Id; }
    }
    public string currentState
    {
        get { return m_Trigger; }
    }

    public void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void SetState(string p_Trigger)
    {
        m_Trigger = p_Trigger;
        m_Animator.SetTrigger(m_Trigger);
    }

    public void AddEndAnimationAction(UnityAction p_Action)
    {
        m_UnityEvent.AddListener(p_Action);
    }

    public void EndAnimation()
    {
        m_UnityEvent.Invoke();
    }
}
