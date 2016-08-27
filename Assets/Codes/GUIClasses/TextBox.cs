using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class TextBox : MonoBehaviour
{
    #region Variables
    private Text m_Text;
    private List<string> m_FullText;
    private bool m_IsTextShowing;
    private int  m_CurrentWord;
    private int  m_CurrentPhrase;
    private PanelActionHandler m_EndAction;
    private bool m_Active = true;
    private Animator m_Animator = null;

    [SerializeField]
    private float m_ShowingTextSpeed = 0.5f;
    #endregion

    #region Interface
    public void Awake()
    {
        m_Text = GetComponentInChildren<Text>();
        m_Animator = GetComponent<Animator>();
    }

    public void SetText(List<string> p_Text)
    {
        m_FullText = p_Text;

        m_Text.text     = "";
        m_CurrentPhrase = 0;
        m_CurrentWord   = 0;
    }

    public void UpdateTextBox()
    {
        if (!m_Active)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            if (m_Animator != null)
            {
                m_Animator.SetTrigger("Bumped");
            }
            NextPhrase();
            Input.ResetInputAxes();
        }
    }

    public void ShowText()
    {
        StopAllCoroutines();
        StartCoroutine(ShowingText());
    }

    public void AddEndAction(PanelActionHandler p_Action)
    {
        m_EndAction += p_Action;
    }

    public void RemoveEndAction(PanelActionHandler p_Action)
    {
        m_EndAction -= p_Action;
    }

    public void Activate(bool p_Value)
    {
        m_Active = p_Value;
    }
    #endregion

    #region Private
    private IEnumerator ShowingText()
    {
        m_Text.text = "";
        m_IsTextShowing = true;
        while (m_CurrentWord < m_FullText[m_CurrentPhrase].Length)
        {
            m_Text.text += m_FullText[m_CurrentPhrase][m_CurrentWord];
            m_CurrentWord++;
            yield return new WaitForSeconds(m_ShowingTextSpeed);
        }
        m_CurrentWord = 0;
        m_IsTextShowing = false;
    }

    private void NextPhrase()
    {
        if (m_IsTextShowing == true)
        {
            StopAllCoroutines();
            m_Text.text = m_FullText[m_CurrentPhrase];
            m_IsTextShowing = false;
            m_CurrentWord = 0;
        }
        else
        {
            m_CurrentPhrase++;
            if (m_CurrentPhrase >= m_FullText.Count)
            {
                EndAction();
            }
            else
            {
                ShowText();
            }
        }
    }

    private void EndAction()
    {
        if (m_EndAction != null)
        {
            m_EndAction();
        }
    }
    #endregion
}
