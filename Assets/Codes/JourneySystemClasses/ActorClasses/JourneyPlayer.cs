using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void ButtonHandler();
public class JourneyPlayer : JourneyActor
{
    #region Variables
    public event ButtonHandler m_ActiveButtonAction;
    public event ButtonHandler m_DisactiveButtonAction;
    #endregion

    #region Interface
    public override void Update()
    {
        base.Update();

        ControlUpdate();

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

        transform.position += m_CurrentSpeed * Time.deltaTime;
    }

    public void AddActiveButtonAction(ButtonHandler p_Action)
    {
        m_ActiveButtonAction += p_Action;
    }

    public void RemoveActiveButtonAction(ButtonHandler p_Action)
    {
        if (m_ActiveButtonAction != null)
        {
            m_ActiveButtonAction -= p_Action;
        }
    }

    public void AddDisactiveButtonAction(ButtonHandler p_Action)
    {
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

    #region Private
    private void ControlUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_Animator.SetBool("Right", true);
            m_CurrentSpeed.x = m_Speed;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_Animator.SetBool("Left", true);
            m_CurrentSpeed.x = -m_Speed;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_Animator.SetBool("Up", true);
            m_CurrentSpeed.y = m_Speed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_Animator.SetBool("Down", true);
            m_CurrentSpeed.y = -m_Speed;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            m_Animator.SetBool("Right", false);
            m_CurrentSpeed.x = 0.0f;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            m_Animator.SetBool("Left", false);
            m_CurrentSpeed.x = 0.0f;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            m_Animator.SetBool("Up", false);
            m_CurrentSpeed.y = 0.0f;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            m_Animator.SetBool("Down", false);
            m_CurrentSpeed.y = 0.0f;
        }
    }
    #endregion
}
