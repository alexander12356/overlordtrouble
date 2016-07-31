using UnityEngine;

public class JourneyPlayer : JourneyActor
{
    public override void Update()
    {
        base.Update();

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

        transform.position += m_CurrentSpeed * Time.deltaTime;
    }
}
