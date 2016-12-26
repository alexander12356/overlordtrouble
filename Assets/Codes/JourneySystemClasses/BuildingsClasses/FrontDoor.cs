using UnityEngine;

public class FrontDoor : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer = null;
    private CheckCollide m_CheckCollide = null;
    private Collider2D m_Collider = null;
    private GameObject m_Warp = null;

    [SerializeField]
    private bool m_Closed = false;

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
    public bool closed
    {
        get { return m_Closed; }
        set
        {
            m_Closed = value;
            if (m_Collider)
            {
                m_Collider.gameObject.SetActive(value);
            }
            if (m_Warp)
            {
                m_Warp.SetActive(!value);
            }
        }
    }

    public void Awake()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        m_CheckCollide = GetComponentInChildren<CheckCollide>();

        Transform l_WarpTransform = transform.FindChild("Warps");
        if (l_WarpTransform != null)
        {
            m_Warp = transform.FindChild("Warps").gameObject;
        }

        Transform l_ColliderTransform = transform.FindChild("Collider");
        if (l_ColliderTransform != null)
        {
            m_Collider = transform.FindChild("Collider").GetComponent<Collider2D>();
        }
    }

    private void Start()
    {
        m_CheckCollide.AddCollideEnterAction(OpenDoor);
        m_CheckCollide.AddCollideExitAction(CloseDoor);

        closed = m_Closed;
    }

    private void OpenDoor(JourneyActor p_JourneyActor)
    {
        if (enabled && !m_Closed)
        {
            m_SpriteRenderer.enabled = false;
        }
    }

    private void CloseDoor(JourneyActor p_JourneyActor)
    {
        if (enabled)
        {
            m_SpriteRenderer.enabled = true;
        }
    }
}
