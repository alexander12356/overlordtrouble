using UnityEngine;

public class CheckCounterAction : MonoBehaviour
{
    private int m_Counter = 0;

    [SerializeField]
    private int m_Count;

    [SerializeField]
    private ActionStruct m_Action;

    public void Run()
    {
        m_Counter++;
        if (m_Counter >= m_Count)
        {
            m_Action.actionEvent.Invoke();
        }
    }
}
