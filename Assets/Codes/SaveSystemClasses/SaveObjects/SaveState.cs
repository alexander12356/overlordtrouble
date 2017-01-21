using UnityEngine;
using UnityEngine.Events;

public class SaveState : MonoBehaviour
{
    private JourneyActor m_JourneyActor;

    public void Awake()
    {
        m_JourneyActor = GetComponent<JourneyActor>();
        SaveSystem.GetInstance().AddActor(m_JourneyActor);

        UnityEvent l_OnDieEvent = new UnityEvent();
        l_OnDieEvent.AddListener(Die);

        m_JourneyActor.onDieEvent = l_OnDieEvent;
    }

    public void Die()
    {
        SaveSystem.GetInstance().ActorDie(m_JourneyActor.actorId);
    }
}