using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationObject : MonoBehaviour
{
    private Animator m_Animator = null;
    private UnityEvent m_UnityEvent = new UnityEvent();
    //TODO fix
    private UnityEvent m_KostilEvent = new UnityEvent();
    private string m_KostilTrigger = "";

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
        m_KostilEvent = new UnityEvent();
    }

    public void SetState(string p_Trigger)
    {
        if (gameObject.activeInHierarchy == false)
        {
            m_KostilTrigger = p_Trigger;
            m_KostilEvent.AddListener(KostilSetState);
            return;
        }
        m_Trigger = p_Trigger;
        m_Animator.SetTrigger(m_Trigger);
    }

    public void AddEndAnimationAction(UnityAction p_Action)
    {
        m_UnityEvent.AddListener(p_Action);
    }

    public void RemoveEndAnimaitonAction(UnityAction p_Action)
    {
        m_UnityEvent.RemoveListener(p_Action);
    }

    public void EndAnimation()
    {
        m_UnityEvent.Invoke();
    }

    public void OnEnable()
    {
        m_KostilEvent.Invoke();
        m_KostilEvent.RemoveAllListeners();
    }

    private void KostilSetState()
    {
        m_Trigger = m_KostilTrigger;
        m_Animator.SetTrigger(m_Trigger);
    }
}
