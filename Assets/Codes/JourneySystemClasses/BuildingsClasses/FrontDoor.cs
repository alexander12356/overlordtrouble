using UnityEngine;

public class FrontDoor : MonoBehaviour
{
    private SpriteRenderer m_FrontDoorRenderer = null;
    private CheckCollide m_CheckCollide = null;

    private void Awake()
    {
        m_FrontDoorRenderer = transform.FindChild("FrontRenderer").GetComponent<SpriteRenderer>();

        if (m_FrontDoorRenderer == null)
        {
            return;
        }

        m_CheckCollide = GetComponentInChildren<CheckCollide>();

        m_CheckCollide.AddCollideEnterAction(OpenDoor);
        m_CheckCollide.AddCollideExitAction(CloseDoor);
    }

    private void OpenDoor(JourneyActor p_JourneyActor)
    {
        m_FrontDoorRenderer.enabled = false;
    }

    private void CloseDoor(JourneyActor p_JourneyActor)
    {
        m_FrontDoorRenderer.enabled = true;
    }
}
