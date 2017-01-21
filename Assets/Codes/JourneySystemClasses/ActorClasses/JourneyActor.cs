using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

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
    protected SpriteRenderer m_SpriteRenderer = null;

    private Dictionary<string, BaseCollideBehavior> m_InteractBehaviorDictionary = new Dictionary<string, BaseCollideBehavior>();
    private Dictionary<string, BaseMovement> m_BaseMovementDictionary = new Dictionary<string, BaseMovement>();
    private Transform m_PivotTransform = null;
    private UnityEvent m_OnDieEvent;

    [SerializeField]
    private string m_ActorId = "TestNPC";

    [SerializeField]
    private string m_InteractBehaviorId = "";

    [SerializeField]
    private string m_MovementBehaviorId = "";
    #endregion

    #region Interface
    public string actorId
    {
        get { return m_ActorId; }
    }
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
    public Transform pivotTransform
    {
        get { return m_PivotTransform; }
    }
    public SpriteRenderer spriteRenderer
    {
        get { return m_SpriteRenderer; }
    }
    public string interactBehaviorId
    {
        get { return m_InteractBehaviorId; }
    }
    public string movementBehaviorId
    {
        get { return m_MovementBehaviorId; }
    }
    public UnityEvent onDieEvent
    {
        set { m_OnDieEvent = value; }
    }

    public virtual void Awake()
    {
        m_Animator  = myAnimator;
        m_Transform = myTransform;
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_PivotTransform = transform.FindChild("Pivot");

        InitBehaviors();
    }

    public virtual void Start()
    {
        UpdateSortingLayer();
    }

    public virtual void InitBehaviors()
    {
        InitInteractBehavior();
        InitMovementBeahvior();
    }

    public virtual void Update()
    {
        if (m_MovementBehaviorId == "")
        {
            return;
        }

        m_BaseMovementDictionary[m_MovementBehaviorId].LogicUpdate();
    }
    
    public virtual void Interact(JourneyActor p_Sender)
    {
        if (m_InteractBehaviorId == "")
        {
            return;
        }

        m_InteractBehaviorDictionary[m_InteractBehaviorId].RunAction(p_Sender);
    }

    public virtual void EndInteract()
    {
        if (m_InteractBehaviorId == "")
        {
            return;
        }

        m_InteractBehaviorDictionary[m_InteractBehaviorId].StopAction();
    }

    public virtual void StartLogic()
    {
        enabled = true;
        m_Animator.SetBool("StopLogic", false);

        if (m_MovementBehaviorId != "")
        {
            m_BaseMovementDictionary[m_MovementBehaviorId].LogicStart();
        }
    }

    public virtual void StopLogic()
    {
        enabled = false;
        m_Animator.SetBool("StopLogic", true);

        if (m_MovementBehaviorId != "")
        {
            m_BaseMovementDictionary[m_MovementBehaviorId].LogicStop();
        }
    }

    public virtual void GoTo(Vector3 p_Target, float p_Delay)
    {
    }

    public void ApplyTo(JourneyActor p_JourneyActor)
    {
        switch(p_JourneyActor.direction)
        {
            case ActorDirection.Down:
                m_Animator.SetTrigger("Up");
                break;
            case ActorDirection.Up:
                m_Animator.SetTrigger("Down");
                break;
            case ActorDirection.Left:
                m_Animator.SetTrigger("Right");
                break;
            case ActorDirection.Right:
                m_Animator.SetTrigger("Left");
                break;
        }
    }

    public void UpdateSortingLayer()
    {
        m_SpriteRenderer.sortingOrder = RoomSystem.GetInstance().GetSortingOrderBound(m_PivotTransform);
    }

    public virtual void ChangeMovementBehavior(string p_BehaviorId)
    {
        m_MovementBehaviorId = p_BehaviorId;
    }

    public virtual void ChangeInteractionBehavior(string p_BehaviorId)
    {
        m_InteractBehaviorId = p_BehaviorId;
    }

    public void Die()
    {
        m_OnDieEvent.Invoke();
        Destroy(gameObject);
    }
    #endregion

    private void InitInteractBehavior()
    {
        Transform l_InteractBehaviorTransform = myTransform.FindChild("InteractBehavior");
        if (l_InteractBehaviorTransform != null)
        {
            for (int i = 0; i < l_InteractBehaviorTransform.childCount; i++)
            {
                BaseCollideBehavior l_InteractBehavior = l_InteractBehaviorTransform.GetChild(i).GetComponent<BaseCollideBehavior>();
                l_InteractBehavior.journeyActor = this;

                m_InteractBehaviorDictionary.Add(l_InteractBehavior.name, l_InteractBehavior);
            }
        }
    }

    private void InitMovementBeahvior()
    {
        Transform l_MovementBehaviorTransform = myTransform.FindChild("MovementBehavior");
        if (l_MovementBehaviorTransform != null)
        {
            for (int i = 0; i < l_MovementBehaviorTransform.childCount; i++)
            {
                BaseMovement l_BaseMovement = l_MovementBehaviorTransform.GetChild(i).GetComponent<BaseMovement>();
                l_BaseMovement.journeyActor = this;

                m_BaseMovementDictionary.Add(l_BaseMovement.name, l_BaseMovement);
            }
        }
    }
}
