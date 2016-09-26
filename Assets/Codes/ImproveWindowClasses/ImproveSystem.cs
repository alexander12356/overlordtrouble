using UnityEngine;

public class ImproveSystem : MonoBehaviour
{
    private static ImproveSystem m_Instance = null;

    [SerializeField]
    private PanelManager m_PanelManager = null;

    public static ImproveSystem GetInstance()
    {
        return m_Instance;
    }

    public void Awake()
    {
        m_Instance = this;
    }

    public void ShowPanel(Panel p_Panel, bool p_WithOverlay = false, Transform p_Parent = null)
    {
        m_PanelManager.ShowPanel(p_Panel, p_WithOverlay, p_Parent);
    }

    public void StartLocation(string p_LocationId)
    {
        m_PanelManager.StartLocation(p_LocationId);
    }

    public void UnloadScene()
    {
        m_PanelManager.UnloadScene();
    }
}
