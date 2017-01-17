using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private static CameraFollow m_Instance = null;
    private Camera m_Camera = null;
    private Transform m_Transform = null;
    private Vector3 m_Delta = Vector3.zero;
    private Vector3 borderMin, borderMax;

    [SerializeField]
    private Transform m_Target = null;
    [SerializeField]
    private BoxCollider2D Bounds = null;

    public void Awake()
    {
        m_Camera = GetComponent<Camera>();
        m_Transform = transform;
        m_Instance = this;
    }
    
	public void Start()
    {
        m_Delta = new Vector3(0.0f, 0.0f, -10.0f);
        borderMin = Bounds.bounds.min;
        borderMax = Bounds.bounds.max;
    }

    public static CameraFollow GetInstance()
    {
        return m_Instance;
    }
	
	public void Update()
    {
        if (m_Target != null)
        {
            m_Transform.position = Vector3.Lerp(m_Transform.position, m_Target.position, 0.1f) + m_Delta;
            var cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height);
            m_Transform.position = new Vector3(Mathf.Clamp(m_Transform.position.x, borderMin.x + cameraHalfWidth, borderMax.x - cameraHalfWidth), 
                Mathf.Clamp(m_Transform.position.y, borderMin.y + GetComponent<Camera>().orthographicSize, borderMax.y - GetComponent<Camera>().orthographicSize), m_Transform.position.z);
        }
	}

    public void SetCameraBounds(BoxCollider2D p_Bounds)
    {
        Bounds = p_Bounds;
        borderMin = Bounds.bounds.min;
        borderMax = Bounds.bounds.max;
    }

    public void InitPos()
    {
        m_Transform.position = m_Target.position;
    }
}
