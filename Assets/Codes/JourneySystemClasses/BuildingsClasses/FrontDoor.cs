using UnityEngine;

public class FrontDoor : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer = null;
    private CheckCollide m_CheckCollide = null;

    public int sortingOrder
    {
        get { return spriteRenderer.sortingOrder;  }
        set { spriteRenderer.sortingOrder = value; }
    }
    public SpriteRenderer spriteRenderer
    {
        get
        {
            if (m_SpriteRenderer == null)
            {
                m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
            return m_SpriteRenderer;
        }
    }

    public void Awake()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        m_CheckCollide = GetComponentInChildren<CheckCollide>();
        m_CheckCollide.AddCollideEnterAction(OpenDoor);
        m_CheckCollide.AddCollideExitAction(CloseDoor);
    }

    private void OpenDoor(JourneyActor p_JourneyActor)
    {
        m_SpriteRenderer.enabled = false;
    }

    private void CloseDoor(JourneyActor p_JourneyActor)
    {
        m_SpriteRenderer.enabled = true;
    }
}
