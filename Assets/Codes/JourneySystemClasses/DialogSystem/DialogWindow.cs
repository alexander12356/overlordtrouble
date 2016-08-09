using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogWindow : MonoBehaviour 
{
    private Text   m_Text;
    private List<string> m_FullText;
    private int    m_CurrentWord = 0;
    private float  m_ShowingTextSpeed = 0.05f;

    [SerializeField]
    private Image m_AvatarImage = null;

    public void Awake()
    {
        m_Text = GetComponentInChildren<Text>();
    }

    public void SetText(List<string> p_Text)
    {
        m_FullText = p_Text;
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            DialogManager.GetInstance().EndDialog();
        }
    }

    private void ShowText()
    {
        StartCoroutine(ShowingText());
    }

    private IEnumerator ShowingText()
    {
        //while (m_CurrentWord < m_FullText.Length)
        //{
        //    m_Text.text += m_FullText[m_CurrentWord];
        //    m_CurrentWord++;
            yield return new WaitForSeconds(m_ShowingTextSpeed);
        //}
    }
}
