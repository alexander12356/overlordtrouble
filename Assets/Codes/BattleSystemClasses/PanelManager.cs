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
    private Stack<Panel> m_PanelStack = new Stack<Panel>();
    private static PanelManager m_Instance;

    [Header("Panels")]

    [SerializeField]
    private MainPanel m_MainPanelPrefab;

    [SerializeField]
    private TextPanel m_TextPanelPrefab;

    #endregion

    #region Interface
    public MainPanel mainPanelPrefab
    {
        get { return m_MainPanelPrefab; }
    }

    public TextPanel textPanelPrefab
    {
        get { return m_TextPanelPrefab; }
    }

    public static PanelManager GetInstance()
    {
        return m_Instance;
    }

    public void ShowPanel(Panel p_NewPanel)
    {
        if (m_PanelStack.Count > 0)
        {
            m_PanelStack.Peek().gameObject.SetActive(false);
        }
        m_PanelStack.Push(p_NewPanel);

        p_NewPanel.transform.SetParent(transform);
        p_NewPanel.transform.localPosition = Vector3.zero;
        p_NewPanel.transform.localScale = Vector3.one;
    }

    public void ClosePanel(Panel p_Panel)
    {
        Panel l_PoppedPanel = m_PanelStack.Pop();
        l_PoppedPanel.PopAction();
        Destroy(l_PoppedPanel.gameObject);

        m_PanelStack.Peek().gameObject.SetActive(true);
    }
    #endregion

    #region Private
    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        MainPanel l_MainPanel = Instantiate(m_MainPanelPrefab);
        ShowPanel(l_MainPanel);
    }

    private void LoadPanelsPrefabs()
    {
    }
    #endregion

    #region In Unity Editor Only
    #if UNITY_EDITOR
    [ContextMenu("Get panel prefabs!")]
    public void GetPanelPrefabs()
    {
        m_MainPanelPrefab = Resources.Load<MainPanel>("Prefabs/Panels/MainPanel");
        m_TextPanelPrefab = Resources.Load<TextPanel>("Prefabs/Panels/TextPanel");
    }
    #endif
    #endregion
}
