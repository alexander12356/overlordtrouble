using UnityEngine;
using UnityEngine.UI;

public delegate void PanelButtonActionHandler();

public class PanelButton : MonoBehaviour
{
    #region Variables
    private Text  m_Text;
    private Image m_SelectedArrowImage;
    private bool  m_Selected;
    private event PanelButtonActionHandler Action;

    [SerializeField]
    private string m_Title = string.Empty;
    #endregion

    #region Interface
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

    public void AddAction(PanelButtonActionHandler p_Action)
    {
        Action += p_Action;
    }

    public void RemoveAction(PanelButtonActionHandler p_Action)
    {
        Action -= p_Action;
    }

    public void RunAction()
    {
        if (Action != null)
        {
            Action();
        }
        else
        {
            Debug.LogWarning("Button: " + m_Title + " not have a action!");
        }
    }
    #endregion

    #region Private
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
    #endregion
}
