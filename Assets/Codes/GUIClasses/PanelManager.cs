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
    #endregion

    #region Interface
    public static PanelManager GetInstance()
    {
        return m_Instance;
    }

    public void ShowPanel(Panel p_NewPanel, bool p_WithOverlay = false)
    {
        if (m_PanelStack.Count > 0 && m_PanelStack.Peek().isShowed)
        {
            if (!p_WithOverlay)
            {
                m_PanelStack.Peek().Hide();
            }
        }
        p_NewPanel.myTransform.SetParent(transform);
        p_NewPanel.myTransform.localPosition = Vector3.zero;
        p_NewPanel.myTransform.localScale = Vector3.one;
        p_NewPanel.gameObject.SetActive(false);

        m_PanelStack.Push(p_NewPanel);
    }

    public void ClosePanel(Panel p_Panel)
    {
        Panel l_PoppedPanel = m_PanelStack.Pop();
        l_PoppedPanel.Close();
    }
    #endregion

    #region Private
    private void Awake()
    {
        m_Instance = this;
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
    #endregion
}
