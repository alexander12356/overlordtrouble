using UnityEngine;
using UnityEngine.UI;

public delegate void PanelButtonActionHandler();

public class PanelButton : MonoBehaviour
{
    #region Variables
    protected event PanelButtonActionHandler m_ConfirmAction;
    private bool  m_Selected;
    private Image m_SelectedImage;
    private Text  m_Text;

    [SerializeField]
    protected string m_Title = string.Empty;

    private static PanelButton m_Prefab = null;
    #endregion

    #region Interface
    public static PanelButton prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<PanelButton>("Prefabs/Button/PanelButton");
            }
            return m_Prefab;
        }
    }
    public string title
    {
        get { return m_Title; }
        set
        {
            if (m_Text == null)
            {
                m_Text = GetComponentInChildren<Text>();
            }
            m_Text.text = m_Title = value;
        }
    }    
    public Text text
    {
        get { return m_Text; }
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
    public Image selectedImage
    {
        get
        {
            if (m_SelectedImage == null)
            {
                m_SelectedImage = GetComponentInChildren<Image>(true);
            }
            return m_SelectedImage;
        }
    }

    public virtual void Awake()
    {
        m_SelectedImage = selectedImage;
        selectedImage.gameObject.SetActive(false);

        m_Text = GetComponentInChildren<Text>();
        m_Text.text = m_Title;
    }

    public virtual void AddAction(PanelButtonActionHandler p_Action)
    {
        m_ConfirmAction += p_Action;
    }

    public virtual void RemoveAction(PanelButtonActionHandler p_Action)
    {
        m_ConfirmAction -= p_Action;
    }

    public virtual void RunAction()
    {
        if (m_ConfirmAction != null)
        {
            m_ConfirmAction();
        }
        else
        {
            Debug.LogWarning("Button: " + gameObject.name + " not have a action!");
        }
    }
    #endregion

    private void Select(bool p_Value)
    {
        m_Selected = p_Value;
        selectedImage.gameObject.SetActive(m_Selected);
    }
}
