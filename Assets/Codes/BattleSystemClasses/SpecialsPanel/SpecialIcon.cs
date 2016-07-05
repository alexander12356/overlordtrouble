using UnityEngine;
using UnityEngine.UI;

public class SpecialIcon : MonoBehaviour
{
    private string m_Id = string.Empty;
    private Text m_Text = null;

    private void Awake()
    {
        m_Text = GetComponentInChildren<Text>(); ;
    }

    public string id
    {
        get
        {
            return m_Id;
        }
        set
        {
            m_Id = value;
            SetText(m_Id);
        }
        
    }

    private void SetText(string p_NewText)
    {
        m_Text.text = p_NewText;
    }
}
