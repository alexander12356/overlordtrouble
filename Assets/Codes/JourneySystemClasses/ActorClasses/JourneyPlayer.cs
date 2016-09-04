using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void ButtonHandler();
public class JourneyPlayer : JourneyActor
{
    #region Variables
    private Vector2 m_InputDirection = Vector2.zero;
    private Rigidbody2D m_RigidBody2d = null;

    public event ButtonHandler m_ActiveButtonAction;
    public event ButtonHandler m_DisactiveButtonAction;
    #endregion

    #region Interface
    public override void Awake()
    {
        base.Awake();

        m_RigidBody2d = GetComponent<Rigidbody2D>();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Z))
        {
            ActiveButtonAction();
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            DisactiveButtonAction();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("DemoMainScene");
        }
        
        m_InputDirection.x = Input.GetAxisRaw("Horizontal");
        m_InputDirection.y = Input.GetAxisRaw("Vertical");

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
    }

    public void FixedUpdate()
    {
        m_RigidBody2d.MovePosition(m_RigidBody2d.position + m_Speed * m_InputDirection.normalized * Time.deltaTime);
    }

    public void AddActiveButtonAction(ButtonHandler p_Action)
    {
        RemoveActiveButtonAction(p_Action);
        m_ActiveButtonAction += p_Action;
        Debug.Log("Action add");
    }

    public void RemoveActiveButtonAction(ButtonHandler p_Action)
    {
        if (m_ActiveButtonAction != null)
        {
            m_ActiveButtonAction -= p_Action;
            Debug.Log("Action remove");
        }
    }

    public void AddDisactiveButtonAction(ButtonHandler p_Action)
    {
        RemoveDisactiveButtonAction(p_Action);
        m_DisactiveButtonAction += p_Action;
    }

    public void RemoveDisactiveButtonAction(ButtonHandler p_Action)
    {
        if (m_DisactiveButtonAction != null)
        {
            m_DisactiveButtonAction -= p_Action;
        }
    }

    public void ActiveButtonAction()
    {
        if (m_ActiveButtonAction != null)
        {
            m_ActiveButtonAction();
        }
    }

    public void DisactiveButtonAction()
    {
        if (m_DisactiveButtonAction != null)
        {
            m_DisactiveButtonAction();
        }
    }
    #endregion
}
