using UnityEngine;
using System.Collections;

public class ProfileSystem : MonoBehaviour
{
    private static ProfileSystem m_Instance = null;

    [SerializeField]
    private PanelManager m_PanelManager = null;

    public static ProfileSystem GetInstance()
    {
        return m_Instance;
    }

    public void Awake()
    {
        m_Instance = this;
    }

    public void ShowPanel(Panel p_Panel, bool p_WithOverlay = false)
    {
        m_PanelManager.ShowPanel(p_Panel, p_WithOverlay);
    }

    public void StartLocation(string p_LocationId)
    {
        m_PanelManager.StartLocation(p_LocationId);
    }

    public void AddScene(string p_SceneId)
    {
        m_PanelManager.AddScene(p_SceneId);
    }

    public void UnloadScene()
    {
        m_PanelManager.UnloadScene();
    }
}
