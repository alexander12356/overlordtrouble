using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
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
    private ScreenFader m_ScreenFader;
    private List<Panel> m_ClosingPanel = new List<Panel>();
    #endregion

    #region Interface
    public ScreenFader screenFader
    {
        get { return m_ScreenFader; }
    }
    public int panelCount
    {
        get { return m_PanelStack.Count; }
    }

    public void ShowPanel(Panel p_NewPanel, bool p_WithOverlay = false, Transform m_Parent = null)
    {
        if (m_PanelStack.Count > 0 && m_PanelStack.Peek().isShowed)
        {
            if (!p_WithOverlay)
            {
                m_PanelStack.Peek().Hide();
            }
        }
        if (m_Parent == null)
        {
            p_NewPanel.myTransform.SetParent(transform);
        }
        else
        {
            p_NewPanel.myTransform.SetParent(m_Parent);
        }
        p_NewPanel.myTransform.localPosition = Vector3.zero;
        p_NewPanel.myTransform.localScale = Vector3.one;
        p_NewPanel.gameObject.SetActive(false);
        p_NewPanel.SetPanelManager(this);

        m_PanelStack.Push(p_NewPanel);
    }

    public void ClosePanel()
    {
        m_ClosingPanel.Add(m_PanelStack.Pop());
    }

    public void PanelDestroyed(Panel p_Panel)
    {
        m_ClosingPanel.Remove(p_Panel);
    }

    public void AddScene(string p_SceneId)
    {
        enabled = false;
        StartCoroutine(AddingScene(p_SceneId));
    }

    public void UnloadScene()
    {
        enabled = false;
        StartCoroutine(UnloadingScene());
    }

    public void UnloadAndLoadScene(string p_SceneId)
    {
        StartCoroutine(UnloadingAndLoadingScene(p_SceneId));
    }

    public void StartLocation(string p_LocationId)
    {
        enabled = false;
        StartCoroutine(StartingLocation(p_LocationId));
    }
    #endregion

    #region Private
    private void Awake()
    {
        m_Instance = this;

        m_ScreenFader = transform.parent.FindChild("ScreenFader").GetComponent<ScreenFader>();
    }

    private void Update()
    {
        if (m_PanelStack.Count == 0 || m_ClosingPanel.Count > 0)
        {
            return;
        }

        Panel m_Panel = m_PanelStack.Peek();

        if (m_Panel == null)
        {
            m_PanelStack.Pop();
            return;
        }

        if (!m_Panel.isShowed && !m_Panel.moving)
        {
            m_Panel.Show();
        }
        else
        {
            m_Panel.UpdatePanel();
        }
    }

    private IEnumerator AddingScene(string p_SceneId)
    {
        yield return StartCoroutine(screenFader.FadeToBlack());

        GameManager.GetInstance().AddScene(p_SceneId);
        enabled = true;
    }

    private IEnumerator UnloadingScene()
    {
        yield return StartCoroutine(screenFader.FadeToBlack());

        GameManager.GetInstance().UnloadScene();
        enabled = true;
    }

    private IEnumerator StartingLocation(string p_LocationId)
    {
        yield return StartCoroutine(screenFader.FadeToBlack());

        GameManager.GetInstance().StartLocation(p_LocationId);
    }

    private IEnumerator UnloadingAndLoadingScene(string p_SceneId)
    {
        yield return StartCoroutine(screenFader.FadeToBlack());

        GameManager.GetInstance().UnloadScene();
        GameManager.GetInstance().AddScene(p_SceneId);
    }
    #endregion
}
