using UnityEngine;

public class CheckCollide : MonoBehaviour
{
    public delegate void CheckCollideHandler(JourneyActor p_JourneyActor);

    #region Variables
    private CheckCollideHandler m_EnterAction = null;
    private CheckCollideHandler m_ExitAction  = null;

    [SerializeField]
    private string[] m_ActorTags = { "Player" };
    #endregion

    #region Interface
    public void AddCollideEnterAction(CheckCollideHandler p_Action)
    {
        m_EnterAction += p_Action;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (HasTriggered(collision.gameObject.tag))
        {
            if (m_EnterAction != null)
            {
                m_EnterAction(collision.GetComponentInParent<JourneyActor>());
            }
        }
    }

    public void AddCollideExitAction(CheckCollideHandler p_Action)
    {
        m_ExitAction += p_Action;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (HasTriggered(collision.gameObject.tag))
        {
            if (m_ExitAction != null)
            {
                m_ExitAction(collision.GetComponentInParent<JourneyActor>());
            }
        }
    }
    #endregion

    private bool HasTriggered(string p_Tag)
    {
        for (int i = 0; i < m_ActorTags.Length; i++)
        {
            if (m_ActorTags[i] == p_Tag)
            {
                return true;
            }
        }

        return false;
    }
}
