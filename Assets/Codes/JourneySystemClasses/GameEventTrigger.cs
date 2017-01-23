using UnityEngine;
using UnityEngine.Events;

public class GameEventTrigger : MonoBehaviour
{
    private UnityEvent m_OnDestroyEvent;

    [SerializeField]
    private string m_Id;

    [SerializeField]
    private bool m_Only = true;

    [SerializeField]
    public ActionStruct m_Action;

    public string id
    {
        get { return m_Id; }
    }
    public UnityEvent onDestroyEvent
    {
        set { m_OnDestroyEvent = value; }
    }

    public void OnTriggerEnter2D(Collider2D p_Collision)
    {
        if (p_Collision.tag == "Player")
        {
            m_Action.actionEvent.Invoke();

            if (m_Only)
            {
                m_OnDestroyEvent.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
