using UnityEngine;

public class ImproveWindow : MonoBehaviour
{
    private static ImproveWindow m_Instance = null;

    [SerializeField]
    private PanelManager m_PanelManager = null;

    public static ImproveWindow GetInstance()
    {
        return m_Instance;
    }

    public void Awake()
    {
        m_Instance = this;
    }

    public void ShowPanel(Panel p_Panel, bool p_WithOverlay = false, Transform p_Transform = null)
    {
        m_PanelManager.ShowPanel(p_Panel, p_WithOverlay, p_Transform);
    }
}
