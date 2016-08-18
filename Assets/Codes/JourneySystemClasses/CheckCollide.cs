using UnityEngine;

public class CheckCollide : MonoBehaviour
{
    public delegate void CheckCollideHandler(JourneyPlayer p_JourneyPlayer);

    #region Variables
    private CheckCollideHandler m_EnterAction = null;
    private CheckCollideHandler m_ExitAction  = null;
    #endregion

    #region Interface
    public void AddCollideEnterAction(CheckCollideHandler p_Action)
    {
        m_EnterAction += p_Action;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (m_EnterAction != null)
            {
                m_EnterAction(collision.GetComponentInParent<JourneyPlayer>());
            }
        }
    }

    public void AddCollideExitAction(CheckCollideHandler p_Action)
    {
        m_ExitAction += p_Action;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (m_ExitAction != null)
            {
                m_ExitAction(collision.GetComponentInParent<JourneyPlayer>());
            }
        }
    }
    #endregion
}
