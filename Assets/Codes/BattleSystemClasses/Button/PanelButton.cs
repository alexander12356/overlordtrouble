using UnityEngine;
using UnityEngine.UI;

public class PanelButton : MonoBehaviour
{
    private Text  m_Text;
    private Image m_SelectedArrowImage;
    private bool  m_Selected;

    [SerializeField]
    private string m_Title = string.Empty;

    private void Awake()
    {
        m_Text = GetComponentInChildren<Text>();
        m_SelectedArrowImage = GetComponentInChildren<Image>(true);

        m_Text.text = m_Title;
        m_SelectedArrowImage.gameObject.SetActive(false);
    }

    private void Start()
    {
    }

    private void Select(bool p_Selected)
    {
        m_Selected = p_Selected;
        m_SelectedArrowImage.gameObject.SetActive(m_Selected);
    }

    public bool selected
    {
        get
        {
            return m_Selected;
        }
        set
        {
            Select(value);
        }
    }
}
