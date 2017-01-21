using UnityEngine;
using UnityEngine.Events;

public class SaveCounter : MonoBehaviour
{
    private CheckCounterAction m_CheckCounter = null;

    public void Awake()
    {
        m_CheckCounter = GetComponent<CheckCounterAction>();

        UnityEvent m_OnRunEvent = new UnityEvent();
        m_OnRunEvent.AddListener(OnRun);
        m_CheckCounter.onRunEvent = m_OnRunEvent;

        SaveSystem.GetInstance().AddCheckCounter(m_CheckCounter);
    }

    public void OnRun()
    {
        SaveSystem.GetInstance().SetCheckCounterValue(m_CheckCounter.id, m_CheckCounter.counter);
    }
}
