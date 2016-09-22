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
    #endregion

    #region Interface
    public ScreenFader screenFader
    {
        get { return m_ScreenFader; }
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
        m_PanelStack.Pop();
    }

    public void StartBattle()
    {
        StartCoroutine(StartingBattle());
    }

    public void EndBattle()
    {
        StartCoroutine(EndingBattle());
    }

    public void StartLocation(string p_LocationId)
    {
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
        if (m_PanelStack.Count == 0)
        {
            return;
        }

        Panel m_Panel = m_PanelStack.Peek();

        if (!m_Panel.isShowed && !m_Panel.moving)
        {
            m_Panel.Show();
        }
        else
        {
            m_Panel.UpdatePanel();
        }
    }

    private IEnumerator StartingBattle()
    {
        yield return StartCoroutine(screenFader.FadeToBlack());

        GameManager.GetInstance().StartBattle();
    }

    private IEnumerator EndingBattle()
    {
        yield return StartCoroutine(screenFader.FadeToBlack());

        GameManager.GetInstance().EndBattle();
    }

    private IEnumerator StartingLocation(string p_LocationId)
    {
        yield return StartCoroutine(screenFader.FadeToBlack());

        GameManager.GetInstance().StartLocation(p_LocationId);
    }
    #endregion
}
