﻿using UnityEngine;
using UnityEngine.UI;

public delegate void PanelButtonActionHandler();

public class PanelButton : MonoBehaviour
{
    #region Variables
    protected event PanelButtonActionHandler m_ConfirmAction;
    private bool    m_Selected;
    protected Image m_SelectedImage;
    protected Text  m_TitleText;
	private RectTransform m_RectTransform = null;

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
	public float titleSizeH
	{
		get 
		{ 
			return m_RectTransform.sizeDelta.y; 
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
            if (m_TitleText == null)
            {
                m_TitleText = GetComponentInChildren<Text>();
            }
            m_TitleText.text = m_Title = value;
        }
    }    
    public Text text
    {
        get { return m_TitleText; }
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

        m_TitleText = GetComponentInChildren<Text>();
        m_TitleText.text = m_Title;

		m_RectTransform = GetComponent<RectTransform> ();
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

    public virtual void Select(bool p_Value)
    {
        m_Selected = p_Value;
        selectedImage.gameObject.SetActive(m_Selected);
    }
    #endregion
}
