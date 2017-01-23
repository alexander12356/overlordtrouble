using UnityEngine;

public class SaveState : MonoBehaviour
{
    public void Awake()
    {
        JourneyActor l_JourneyActor = GetComponent<JourneyActor>();
        SaveSystem.GetInstance().AddActor(l_JourneyActor);

        JourneyActorUnityEvent l_OnDieEvent = new JourneyActorUnityEvent();
        l_OnDieEvent.AddListener(Die);

        l_JourneyActor.onDieEvent = l_OnDieEvent;
    }

    public void Die(JourneyActor p_JourneyActor)
    {
        SaveSystem.GetInstance().ActorDie(p_JourneyActor.actorId);
    }
}