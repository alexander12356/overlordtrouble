using UnityEngine;
using System.Collections;

public class MainMenuSystem : MonoBehaviour
{
    private static MainMenuSystem m_Instance;

    [SerializeField]
    private PanelManager m_PanelManager = null;

    public static MainMenuSystem GetInstance()
    {
        return m_Instance;
    }

    public void Awake()
    {
        m_Instance = this;
        PlayerData.GetInstance();
        LoadDataBases();
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

    private void LoadDataBases()
    {
        DataLoader.GetInstance();
    }
}
