using UnityEngine;
using UnityEngine.Events;

public class CheckCounterAction : MonoBehaviour
{
    private int m_Counter = 0;
    private UnityEvent m_OnRunEvent = null;

    [SerializeField]
    private string m_Id;

    [SerializeField]
    private int m_Count;

    [SerializeField]
    private ActionStruct m_Action;

    public string id
    {
        get { return m_Id; }
    }
    public int counter
    {
        get { return m_Counter; }
        set { m_Counter = value; }
    }
    public UnityEvent onRunEvent
    {
        set { m_OnRunEvent = value; }
    }

    public void Run()
    {
        m_Counter++;
        m_OnRunEvent.Invoke();
        if (m_Counter >= m_Count)
        {
            m_Action.actionEvent.Invoke();
        }
    }
}
