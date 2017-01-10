using UnityEngine.UI;

public class InventoryTabButton : PanelButton {
    protected new bool m_Selected;
    protected Image m_BackgroundImage = null;
    protected new Image m_SelectedImage = null;
    private InventoryTabNew m_InventoryTab = null;

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

    public InventoryTabNew inventoryTab
    {
        get
        {
            return m_InventoryTab;
        }
        set
        {
            m_InventoryTab = value;
        }
    }

    public override void Awake()
    {
        this.m_BackgroundImage = backgroundImage;
        this.m_SelectedImage = selectedImage;

        this.backgroundImage.gameObject.SetActive(true);
        this.selectedImage.gameObject.SetActive(false);
    }

    public override void Select(bool p_Value)
    {
        this.m_Selected = p_Value;
        this.backgroundImage.gameObject.SetActive(!m_Selected);
        this.selectedImage.gameObject.SetActive(m_Selected);
    }
}
