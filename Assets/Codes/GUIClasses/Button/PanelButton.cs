using UnityEngine;
using UnityEngine.UI;

public delegate void PanelButtonActionHandler();

public class PanelButton : MonoBehaviour
{
    #region Variables
    private event PanelButtonActionHandler Action;
    private bool  m_Selected;
    private Image m_SelectedImage;
    private Text  m_Text;
	private RectTransform m_RectTransform = null;

    [SerializeField]
    private string m_Title = string.Empty;


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


	public float titleSizeH
	{
		get 
		{ 
			return m_RectTransform.rect.height; 
		}
		set
		{
			Vector2 l_Vector2 = m_RectTransform.sizeDelta;
			l_Vector2.y = value;
			m_RectTransform.sizeDelta = l_Vector2;
		}
	}

	public float titleSizeW
	{
		get 
		{ 
			return m_RectTransform.rect.width; 
		}
		set
		{
			Vector2 l_Vector2 = m_RectTransform.sizeDelta;
			l_Vector2.x = value;
			m_RectTransform.sizeDelta = l_Vector2;
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

    public virtual void AddAction(PanelButtonActionHandler p_Action)
    {
        Action += p_Action;
    }

    public virtual void RemoveAction(PanelButtonActionHandler p_Action)
    {
        Action -= p_Action;
    }

    public virtual void RunAction()
    {
        if (Action != null)
        {
            Action();
        }
        else
        {
            Debug.LogWarning("Button: " + gameObject.name + " not have a action!");
        }
    }
    #endregion
    private void Awake()
    {
        m_SelectedImage = selectedImage;
        selectedImage.gameObject.SetActive(false);

        m_Text = GetComponentInChildren<Text>();
        m_Text.text = m_Title;

		m_RectTransform = GetComponent<RectTransform> ();
    }

    private void Select(bool p_Value)
    {
        m_Selected = p_Value;
        selectedImage.gameObject.SetActive(m_Selected);
    }
}
