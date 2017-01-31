using UnityEngine.UI;

public abstract class InventoryPanelButton : PanelButton
{
    private Image m_ChoosedImage = null;

    public Image choosedImage
    {
        get
        {
            if(m_ChoosedImage == null)
            {
                m_ChoosedImage = transform.GetChild(1).gameObject.GetComponent<Image>();
            }
            return m_ChoosedImage;
        }
    }

    public new Image selectedImage
    {
        get
        {
            if(m_SelectedImage == null)
            {
                m_SelectedImage = transform.GetChild(0).gameObject.GetComponent<Image>();
            }
            return m_SelectedImage;
        }
    }

    public override void Awake()
    {
        m_ChoosedImage = choosedImage;
        m_SelectedImage = selectedImage;

        choosedImage.gameObject.SetActive(false);
        selectedImage.gameObject.SetActive(false);
    }

    public override void Select(bool p_Value)
    {
        m_Selected = p_Value;
        selectedImage.gameObject.SetActive(m_Selected);
    }

    public override void Choose(bool p_Value)
    {
        base.Choose(p_Value);
        choosedImage.gameObject.SetActive(p_Value);
    }
}
