using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

public delegate void ButtonHandler();
public class JourneyPlayer : JourneyActor
{
    #region Variables
    private Vector2 m_InputDirection = Vector2.zero;
    private Rigidbody2D m_RigidBody2d = null;
    private List<JourneyActor> m_InteractableActors = new List<JourneyActor>();
    private BaseCollideBehavior m_CurrentCollideBehavior = null;
    #endregion

    #region Interface
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
