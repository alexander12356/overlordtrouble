using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogWindow : MonoBehaviour 
{
    private enum State
    {
        Opened,
        Closing,
        Closed
    }

    private static DialogWindow m_Prefab;

    #region Variables
    private List<string> m_FullText;

    private Text    m_Text;
    private int     m_CurrentWord      = 0;
    private int     m_CurrentPhrase    = 0;
    private float   m_ShowingTextSpeed = 0.05f;
    private State   m_State            = State.Closed;
    private bool    m_IsTextShowing    = false;

    [SerializeField]
    private Image m_AvatarImage = null;

    [SerializeField]
    private Vector3 m_DestPos = Vector3.zero;

    [SerializeField]
    private Vector3 m_StartPos = Vector3.zero;

    [SerializeField]
    private float m_ShowSpeed = 0.0f;
    #endregion

    #region Interface
    public static DialogWindow prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<DialogWindow>("Prefabs/Windows/DialogWindow");
            }
            return m_Prefab;
        }
    }

    public void Awake()
    {
        m_Text = GetComponentInChildren<Text>();
    }

    public void SetDialog(Dialog p_Dialog)
    {
        m_FullText = p_Dialog.phrases;
        m_AvatarImage.sprite = Resources.Load<Sprite>(p_Dialog.avatarImagePath);
    }

    public void Update()
    {
        if (m_State == State.Closed || m_State == State.Closing)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            NextPhrase();
        }
    }

    public void Show()
    {
        StartCoroutine(Showing());
    }

    public void Close()
    {
        StartCoroutine(Closing());
    }
    #endregion

    #region Private
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
                Close();
            }
            else
            {
                ShowText();
            }
        }
    }

    private IEnumerator Showing()
    {
        while ((transform.localPosition - m_DestPos).sqrMagnitude > 0.05f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, m_DestPos, m_ShowSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        m_State = State.Opened;

        ShowText();
    }

    private IEnumerator Closing()
    {
        m_State = State.Closing;
        while ((transform.localPosition - m_StartPos).sqrMagnitude > 0.05f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, m_StartPos, m_ShowSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        m_State = State.Closed;

        DialogManager.GetInstance().EndDialog();
        Destroy(gameObject);
    }
    #endregion
}
