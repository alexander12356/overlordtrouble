using UnityEngine;

using System.Collections.Generic;

public delegate void ButtonHandler();
public class JourneyPlayer : JourneyActor
{
    #region Variables
    private Vector2 m_InputDirection = Vector2.zero;
    private Rigidbody2D m_RigidBody2d = null;
    private List<JourneyActor> m_InteractableActors = new List<JourneyActor>();
    private BaseCollideBehavior m_CurrentCollideBehavior = null;
    private PlayerStatistics m_Statistics;
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

        InitCheckCollider();
        m_RigidBody2d = GetComponent<Rigidbody2D>();
        m_Statistics = GetComponent<PlayerStatistics>();
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
                m_ActorDirection = ActorDirection.Right;
            }
            else if (m_InputDirection.x < 0)
            {
                m_ActorDirection = ActorDirection.Left;
            }
            else if (m_InputDirection.y > 0)
            {
                m_ActorDirection = ActorDirection.Up;
            }
            else if (m_InputDirection.y < 0)
            {
                m_ActorDirection = ActorDirection.Down;
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

        m_InputDirection = Vector2.zero;
        FixedUpdate();
    }

    public void LoadImprove()
    {
        m_Animator.runtimeAnimatorController = PlayerData.GetInstance().GetAnimatorController();
    }

    public void AddCollideActor(JourneyActor p_JourneyActor)
    {
        if (m_InteractableActors.Contains(p_JourneyActor))
        {
            return;
        }
        m_InteractableActors.Add(p_JourneyActor);
    }

    public void RemoveCollideActor(JourneyActor p_JourneyActor)
    {
        m_InteractableActors.Remove(p_JourneyActor);
    }

    public void PressDisactiveButtonAction()
    {
        m_CurrentCollideBehavior.StopAction();
    }

    public override void GoTo(Vector3 p_Target, float p_Delay)
    {
        base.GoTo(p_Target, p_Delay);

        UpdateSortingLayer();
        myTransform.localPosition = Vector3.MoveTowards(myTransform.localPosition, p_Target, p_Delay);

        float l_DeltaX = myTransform.localPosition.x - p_Target.x;
        float l_DeltaY = myTransform.localPosition.y - p_Target.y;
        if (Mathf.Abs(l_DeltaX) > Mathf.Abs(l_DeltaY))
        {
            if (l_DeltaX > 0)
            {
                myAnimator.SetFloat("Input_X", -1);
                myAnimator.SetFloat("Input_Y", 0);
            }
            else
            {
                myAnimator.SetFloat("Input_X", 1);
                myAnimator.SetFloat("Input_Y", 0);
            }
        }
        else
        {
            if (l_DeltaY > 0)
            {
                myAnimator.SetFloat("Input_X", 0);
                myAnimator.SetFloat("Input_Y", -1);
            }
            else
            {
                myAnimator.SetFloat("Input_X", 0);
                myAnimator.SetFloat("Input_Y", 1);
            }
        }
    }
    #endregion

    #region Private
    private void InitCheckCollider()
    {
        CheckCollide l_CheckCollide = myTransform.FindChild("CheckCollide").GetComponent<CheckCollide>();

        l_CheckCollide.AddCollideEnterAction(AddCollideActor);
        l_CheckCollide.AddCollideExitAction(RemoveCollideActor);
    }

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
        if (m_InteractableActors.Count == 0)
        {
            return;
        }

        float[] l_Distances = new float[m_InteractableActors.Count];
        for (int i = 0; i < m_InteractableActors.Count; i++)
        {
            l_Distances[i] = (myTransform.position - m_InteractableActors[i].myTransform.position).sqrMagnitude;
        }

        int l_MinId = 0;
        float l_MinValue = l_Distances[0];
        for (int i = 0; i < l_Distances.Length; i++)
        {
            if (l_MinValue > l_Distances[i])
            {
                l_MinValue = l_Distances[i];
                l_MinId = i;
            }
        }

        m_CurrentCollideBehavior = m_InteractableActors[l_MinId].GetComponent<BaseCollideBehavior>();
        m_CurrentCollideBehavior.RunAction(this);
    }
    #endregion
}
