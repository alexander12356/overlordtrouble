using UnityEngine;

using System.Collections.Generic;

public delegate void ButtonHandler();
public class JourneyPlayer : JourneyActor
{
    #region Variables
    private Vector2 m_InputDirection = Vector2.zero;
    private Rigidbody2D m_RigidBody2d = null;
    private JourneyActor m_InteractJourneyActor = null;
    private PlayerStatistics m_Statistics;
    private CheckTrigger m_CheckTrigger = null;

    [SerializeField]
    private float m_Speed = 5.0f;
    #endregion

    #region Interface
    public PlayerStatistics statistics
    {
        get { return m_Statistics; }
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            enabled = true;
        }
        else
        {
            enabled = false;
            myAnimator.SetBool("IsWalking", false);
        }
    }

    public override void Awake()
    {
        base.Awake();
        
        m_RigidBody2d = GetComponent<Rigidbody2D>();
        m_Statistics = GetComponent<PlayerStatistics>();
        m_CheckTrigger = myTransform.FindChild("CheckTrigger").GetComponent<CheckTrigger>();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Z))
        {
            PressActiveButtonAction();
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            PressDisactiveButtonAction();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            RunPauseMenu();
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            OpenInventory();
        }

        if (enabled != false)
        {
            m_InputDirection.x = Input.GetAxisRaw("Horizontal");
            m_InputDirection.y = Input.GetAxisRaw("Vertical");
        }

        if (m_InputDirection != Vector2.zero)
        {
            myAnimator.SetBool("IsWalking", true);
            myAnimator.SetFloat("Input_X", m_InputDirection.x);
            myAnimator.SetFloat("Input_Y", m_InputDirection.y);

            if (m_InputDirection.x > 0)
            {
                SetDirection(ActorDirection.Right);
            }
            else if (m_InputDirection.x < 0)
            {
                SetDirection(ActorDirection.Left);
            }
            else if (m_InputDirection.y > 0)
            {
                SetDirection(ActorDirection.Up);
            }
            else if (m_InputDirection.y < 0)
            {
                SetDirection(ActorDirection.Down);
            }
        }
        else
        {
            myAnimator.SetBool("IsWalking", false);
        }

        UpdateSortingLayer();
    }

    public void FixedUpdate()
    {
        m_RigidBody2d.MovePosition(m_RigidBody2d.position + m_Speed * m_InputDirection.normalized * Time.deltaTime);
    }

    public override void StopLogic()
    {
        base.StopLogic();

        myAnimator.SetBool("IsWalking", false);
        m_InputDirection = Vector2.zero;
        FixedUpdate();
    }

    public void LoadImprove()
    {
        m_Animator.runtimeAnimatorController = PlayerData.GetInstance().GetAnimatorController();
    }

    public void PressDisactiveButtonAction()
    {
        if (m_InteractJourneyActor != null)
        {
            m_InteractJourneyActor.EndInteract();
        }
    }

    public void SetInteractActor(JourneyActor p_JourneyActor)
    {
        m_InteractJourneyActor = p_JourneyActor;
    }

    public void RemoveInteractActor(JourneyActor p_JourneyActor)
    {
        if (m_InteractJourneyActor == p_JourneyActor)
        {
            m_InteractJourneyActor = null;
        }
    }

    public override void GoTo(Vector3 p_Target, float p_Delay)
    {
        base.GoTo(p_Target, p_Delay);

        UpdateSortingLayer();
        myTransform.position = Vector3.MoveTowards(myTransform.position, p_Target, p_Delay);

        float l_DeltaX = myTransform.position.x - p_Target.x;
        float l_DeltaY = myTransform.position.y - p_Target.y;
        if (Mathf.Abs(l_DeltaX) > Mathf.Abs(l_DeltaY))
        {
            if (l_DeltaX > 0)
            {
                myAnimator.SetFloat("Input_X", -1);
                myAnimator.SetFloat("Input_Y", 0);
                SetDirection(ActorDirection.Left);
            }
            else
            {
                myAnimator.SetFloat("Input_X", 1);
                myAnimator.SetFloat("Input_Y", 0);
                SetDirection(ActorDirection.Right);
            }
        }
        else
        {
            if (l_DeltaY > 0)
            {
                myAnimator.SetFloat("Input_X", 0);
                myAnimator.SetFloat("Input_Y", -1);
                SetDirection(ActorDirection.Down);
            }
            else
            {
                myAnimator.SetFloat("Input_X", 0);
                myAnimator.SetFloat("Input_Y", 1);
                SetDirection(ActorDirection.Up);
            }
        }
    }
    #endregion

    #region Private
    private void RunPauseMenu()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Panel);

        PauseMenuPanel l_PauseMenuPanel = Instantiate(PauseMenuPanel.prefab);
        JourneySystem.GetInstance().ShowPanel(l_PauseMenuPanel);
    }

    private void OpenInventory()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Panel);
        InventoryPanel lInventoryPanel = Instantiate(InventoryPanel.prefab);
        JourneySystem.GetInstance().ShowPanel(lInventoryPanel);
    }

    private void PressActiveButtonAction()
    {
        if (m_InteractJourneyActor != null)
        {
            m_InteractJourneyActor.Interact(this);
        }
    }

    private void SetDirection(ActorDirection p_Direction)
    {
        m_ActorDirection = p_Direction;
        
        switch (m_ActorDirection)
        {
            case ActorDirection.Right:
                m_CheckTrigger.checkCollider.size = new Vector2(0.05f, 0.05f);
                m_CheckTrigger.checkCollider.offset = new Vector2(0.18f, 0.0f);

                m_CheckTrigger.transform.eulerAngles = Vector3.zero;
                break;
            case ActorDirection.Left:
                m_CheckTrigger.checkCollider.size = new Vector2(0.05f, 0.05f);
                m_CheckTrigger.checkCollider.offset = new Vector2(0.18f, 0.0f);

                m_CheckTrigger.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
                break;
            case ActorDirection.Down:
                m_CheckTrigger.checkCollider.size = new Vector2(0.2f, 0.05f);
                m_CheckTrigger.checkCollider.offset = new Vector2(0.0f, -0.08f);

                m_CheckTrigger.transform.eulerAngles = Vector3.zero;
                break;
            case ActorDirection.Up:
                m_CheckTrigger.checkCollider.size = new Vector2(0.2f, 0.05f);
                m_CheckTrigger.checkCollider.offset = new Vector2(0.0f, -0.08f);

                m_CheckTrigger.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
                break;
        }
    }
    #endregion
}
