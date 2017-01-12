using UnityEngine.UI;

public class InventoryViewButton : PanelButton {

    protected Image m_BackgroundImage = null;
    private InventoryView m_InventoryView = null;

    public new bool selected
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

    public Image backgroundImage
    {
        get
        {
            if(m_BackgroundImage == null)
            {
                m_BackgroundImage = transform.GetChild(0).gameObject.GetComponent<Image>();
            }
            return m_BackgroundImage;
        }
    }

    public new Image selectedImage
    {
        get
        {
            if (m_SelectedImage == null)
            {
                m_SelectedImage = transform.GetChild(1).gameObject.GetComponent<Image>();
            }
            return m_SelectedImage;
        }
    }

    public InventoryView inventoryView
    {
        get
        {
            return m_InventoryView;
        }
        set
        {
            m_InventoryView = value;
        }
    }

    public override void Awake()
    {
        m_BackgroundImage = backgroundImage;
        m_SelectedImage = selectedImage;

        backgroundImage.gameObject.SetActive(true);
        selectedImage.gameObject.SetActive(false);
    }

    public override void Select(bool p_Value)
    {
        m_Selected = p_Value;
        backgroundImage.gameObject.SetActive(!m_Selected);
        selectedImage.gameObject.SetActive(m_Selected);
    }
}
