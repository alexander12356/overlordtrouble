using UnityEngine;
using UnityEngine.UI;

using System;
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
    private Animator m_ActiveButtonAnimator = null;
    private Animator m_TalkingAnimator = null;
    private string m_TalkingAnimationId = string.Empty;
    private Image m_ActiveKey = null;
    private Text m_ActiveKeyText = null;

    [SerializeField]
    private float m_ShowingTextSpeed = 0.5f;

    [SerializeField]
    private int m_PageMaxSymbolCount = 255;
    #endregion

    #region Interface
    public bool isActiveButton
    {
        set
        {
            m_ActiveKey.enabled = value;
            m_ActiveKeyText.enabled = value;
            m_ActiveButtonAnimator.enabled = value; 
        }
    }

    public void Awake()
    {
        m_Text = GetComponentInChildren<Text>();
        m_ActiveButtonAnimator = GetComponent<Animator>();
        m_ActiveKey = GetComponentInChildren<Image>();

        Transform l_ActiveKeyTransform = transform.FindChild("ActiveKeyText");
        if (l_ActiveKeyTransform != null)
        {
            m_ActiveKeyText = l_ActiveKeyTransform.GetComponent<Text>();
        }
    }

    public void SetText(List<string> p_Text)
    {
        m_FullText = ResizeText(p_Text);

        m_Text.text     = "";
        m_CurrentPhrase = 0;
        m_CurrentWord   = 0;
    }

    public void SetTalkingAnimator(Animator p_TalkingAnimator, string p_TalkingAnimationId)
    {
        m_TalkingAnimator = p_TalkingAnimator;
        m_TalkingAnimationId = p_TalkingAnimationId;
    }

    public void UpdateTextBox()
    {
        if (!m_Active)
        {
            return;
        }

        if (ControlSystem.EnterButton())
        {
            if (m_ActiveButtonAnimator != null)
            {
                m_ActiveButtonAnimator.SetTrigger("Bumped");
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

    public void SetTalking(bool p_Value)
    {
        if (m_TalkingAnimator)
        {
            m_TalkingAnimator.SetBool(m_TalkingAnimationId, p_Value);
        }
    }
    #endregion

    #region Private
    private IEnumerator ShowingText()
    {
        SetTalking(true);

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

        SetTalking(false);
    }

    private void NextPhrase()
    {
        if (m_IsTextShowing == true)
        {
            SetTalking(false);

            StopAllCoroutines();
            m_Text.text = m_FullText[m_CurrentPhrase];
            m_IsTextShowing = false;
            m_CurrentWord = 0;
        }
        else
        {
            AudioSystem.GetInstance().PlaySound("SelectOption");
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

    private List<string> ResizeText(List<string> p_FullText)
    {
        List<string> l_NewFullText = new List<string>();

        for (int i = 0; i < p_FullText.Count; i++)
        {
            string[] l_Words = p_FullText[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            string l_Text = "";
            for (int j = 0; j < l_Words.Length; j++)
            {
                if (l_Text.Length + l_Words[j].Length + 4 < m_PageMaxSymbolCount)
                {
                    l_Text += l_Words[j] + " ";
                }
                else
                {
                    l_Text += " ...";
                    l_NewFullText.Add(l_Text);
                    l_Text = l_Words[j] + " ";
                }
            }

            l_NewFullText.Add(l_Text);
        }

        return l_NewFullText;
    }
    #endregion
}
