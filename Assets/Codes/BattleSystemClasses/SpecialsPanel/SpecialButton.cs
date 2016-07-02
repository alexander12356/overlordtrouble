using UnityEngine;
using UnityEngine.UI;

public class SpecialButton : MonoBehaviour
{
    [SerializeField]
    private string m_Id;

    private bool m_Select = false;
    private Button m_Button = null;
    private Text m_Text;

    public string id
    {
        get { return m_Id;  }
        set { m_Id = value; }
    }
    public bool select
    {
        get { return m_Select;  }
        set { m_Select = value; }
    }

	private void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Text = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        ChangeText(m_Id);
    }

    public void Select()
    {
        SpecialSelectPanel.GetInstance().AddSpecial(m_Id);
    }

    public void ChangeText(string p_NewText)
    {
        m_Text.text = p_NewText;
    }
}
