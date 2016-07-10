using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class TextPanel : Panel
{
    #region Variables
    private Text m_Text;
    private string m_FullText = "Тестовый текст";
    private int m_CurrentWord = 0;
    private float m_ShowingTextSpeed = 0.05f;
    private bool m_EndShowing = false;
    private static TextPanel m_Prefab = null;
    private PanelButtonActionHandler m_ButtonAction = null;
    #endregion

    #region Interface
    public void AddButtonAction(PanelButtonActionHandler p_Action)
    {
        m_ButtonAction += p_Action;
    }

    public void RemoveButtonAction(PanelButtonActionHandler p_Action)
    {
        m_ButtonAction -= p_Action;
    }

    public void ButtonAction()
    {
        if (m_ButtonAction != null)
        {
            m_ButtonAction();
        }
    }

    public static TextPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<TextPanel>("Prefabs/Panels/TextPanel");
            }
            return m_Prefab;
        }
    }

    public void SetText(string p_Text)
    {
        m_FullText = p_Text;
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            if (!m_EndShowing)
            {
                StopAllCoroutines();
                m_Text.text = m_FullText;
                m_EndShowing = true;
            }
            else
            {
                PanelManager.GetInstance().ClosePanel(this);
                ButtonAction();
            }
        }
    }
    #endregion

    #region Private
    private void Awake()
    {
        m_Text = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        AddPushAction(ShowText);
    }

    private void ShowText()
    {
        StartCoroutine(ShowingText());
    }

    private IEnumerator ShowingText()
    {
        while (m_CurrentWord < m_FullText.Length)
        {
            m_Text.text += m_FullText[m_CurrentWord];
            m_CurrentWord++;
            yield return new WaitForSeconds(m_ShowingTextSpeed);
        }
        EndTextShowing();
    }

    private void EndTextShowing()
    {
        m_EndShowing = true;
    }
    #endregion
}
