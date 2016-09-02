using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    private Camera m_Camera = null;
    private Transform m_Transform = null;
    private Vector3 m_Delta = Vector3.zero;

    [SerializeField]
    private Transform m_Target = null;
	
	public void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_Transform = transform;
        m_Delta = new Vector3(0.0f, 0.0f, -10.0f);
    }
	
	public void Update()
    {
        m_Camera.orthographicSize = (Screen.height / 100.0f) / 2f;

        if (m_Target != null)
        {
            m_Transform.position = Vector3.Lerp(m_Transform.position, m_Target.position, 0.1f) + m_Delta;
        }
	}
}
