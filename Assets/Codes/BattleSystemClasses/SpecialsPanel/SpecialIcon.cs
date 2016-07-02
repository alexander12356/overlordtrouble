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

    public void SetSpecial(string p_Id)
    {
        m_Id = p_Id;
        SetText(p_Id);
    }

    private void SetText(string p_NewText)
    {
        m_Text.text = p_NewText;
    }
}
