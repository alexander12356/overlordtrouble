using UnityEngine;
using System.Collections.Generic;

public enum PanelEnum
{
    Main,
    SpecialSelect,
    SpecialUpdate,
    WinPanel,
    LosePanel
}

public class PanelManager : MonoBehaviour
{
    #region Variables

    private Dictionary<PanelEnum, GameObject> m_PanelList = new Dictionary<PanelEnum, GameObject>();

    private PanelEnum m_CurrentShowedPanel = PanelEnum.Main;

    private static PanelManager m_Instance;

    #endregion

    #region Interface

    public static PanelManager GetInstance()
    {
        return m_Instance;
    }

    public void AddPanel(PanelEnum p_PanelName, GameObject p_Panel)
    {
        m_PanelList.Add(p_PanelName, p_Panel);
    }

    public void Show(PanelEnum p_PanelName)
    {
        if (p_PanelName == m_CurrentShowedPanel)
        {
            return;
        }

        HidePanel(m_CurrentShowedPanel);
        ShowPanel(p_PanelName);

        m_CurrentShowedPanel = p_PanelName;
    }

    #endregion

    #region Private

    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
    }

    private void HidePanel(PanelEnum p_PanelName)
    {
        m_PanelList[p_PanelName].gameObject.SetActive(false);
    }

    private void ShowPanel(PanelEnum p_PanelName)
    {
        m_PanelList[p_PanelName].gameObject.SetActive(true);
    }

    #endregion
}
