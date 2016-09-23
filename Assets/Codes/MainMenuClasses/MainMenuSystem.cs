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

        MainMenuPanel m_MainMenuPanel = Instantiate(MainMenuPanel.prefab);
        m_PanelManager.ShowPanel(m_MainMenuPanel);

        PlayerData.GetInstance();
    }

    public void StartLocation(string p_LocationId)
    {
        m_PanelManager.StartLocation(p_LocationId);
        LoadDataBases();
    }

    private void LoadDataBases()
    {
        DataLoader.GetInstance();
    }
}
