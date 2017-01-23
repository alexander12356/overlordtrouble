using UnityEngine;
using UnityEngine.Events;

public class SaveTrigger : MonoBehaviour
{
    private GameEventTrigger m_GameEventTrigger = null;

    public void Awake()
    {
        m_GameEventTrigger = GetComponent<GameEventTrigger>();

        SaveSystem.GetInstance().AddTrigger(m_GameEventTrigger);

        UnityEvent l_OnDestroyEvent = new UnityEvent();
        l_OnDestroyEvent.AddListener(EventDestroy);

        m_GameEventTrigger.onDestroyEvent = l_OnDestroyEvent;
    }

    public void EventDestroy()
    {
        SaveSystem.GetInstance().GameEventDestroy(m_GameEventTrigger.id);
    }
}
