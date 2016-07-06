using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class TextPanel : Panel
{
    #region Variables
    private Text m_Text;
    private string m_FullText = "Тестовый текст";
    private int m_CurrentWord = 0;
    private float m_ShowingSpeed = 0.05f;
    private bool m_EndShowing = false;
    #endregion

    #region Interface
    public void SetText(string p_Text)
    {
        m_FullText = p_Text;
    }
    #endregion

    #region Private
    private void Awake()
    {
        m_Text = GetComponent<Text>();
    }

    private void Start()
    {
        StartCoroutine(ShowingText());
    }

    private IEnumerator ShowingText()
    {
        while (m_CurrentWord < m_FullText.Length)
        {
            m_Text.text += m_FullText[m_CurrentWord];
            m_CurrentWord++;
            yield return new WaitForSeconds(m_ShowingSpeed);
        }
        EndShowing();
    }

    private void EndShowing()
    {
        m_EndShowing = true;
    }

    private void Update()
    {
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
            }
        }
    }
    #endregion
}
