using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogPanel : Panel 
{
    private static DialogPanel m_Prefab;

    #region Variables
    private List<string> m_FullText;

    private Text    m_Text;
    private int     m_CurrentWord      = 0;
    private int     m_CurrentPhrase    = 0;
    private float   m_ShowingTextSpeed = 0.05f;
    private bool    m_IsTextShowing    = false;

    [SerializeField]
    private Image m_AvatarImage = null;
    #endregion

    #region Interface
    public static DialogPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<DialogPanel>("Prefabs/Windows/DialogPanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_Text = GetComponentInChildren<Text>();
    }

    public void SetDialog(Dialog p_Dialog)
    {
        m_FullText = p_Dialog.phrases;
        m_AvatarImage.sprite = Resources.Load<Sprite>(p_Dialog.avatarImagePath);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (Input.GetKeyUp(KeyCode.Z))
        {
            NextPhrase();
        }
    }
    #endregion

    #region Private
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
                PanelManager.GetInstance().ClosePanel(this);
                DialogManager.GetInstance().EndDialog();
            }
            else
            {
                ShowText();
            }
        }
    }
    #endregion
}