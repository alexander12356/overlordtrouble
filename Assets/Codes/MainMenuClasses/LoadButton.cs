using UnityEngine;
using UnityEngine.UI;

public class LoadButton : PanelButton {

    private static LoadButton m_Prefab = null;
    private string m_Id;
    private SaveData m_SaveData = null;
    private Image m_AvatarImage;

    public static LoadButton prefab
    {
        get
        {
            if(m_Prefab == null)
            {
                m_Prefab = Resources.Load<LoadButton>("Prefabs/Button/LoadButton");
            }
            return m_Prefab;
        }
    }
    private new Image selectedImage
    {
        get
        {
            if(m_SelectedImage == null)
            {
                m_SelectedImage = transform.FindChild("BackgroundSelected").GetComponent<Image>();
            }
            return m_SelectedImage;
        }
    }
    public string id
    {
        get { return m_Id; }
        set { m_Id = value; }
    }
    public SaveData saveData
    {
        get { return m_SaveData; }
    }

    public override void Awake()
    {
        m_SelectedImage = selectedImage;
        selectedImage.gameObject.SetActive(false);

        m_TitleText = GetComponentInChildren<Text>();
        m_TitleText.text = m_Title;
        m_AvatarImage = transform.FindChild("Avatar").GetComponent<Image>();
    }

    public override void Select(bool p_Value)
    {
        m_Selected = p_Value;
        selectedImage.gameObject.SetActive(m_Selected);
    }

    public void Init(SaveData p_SaveData)
    {
        m_SaveData = p_SaveData;
        Sprite[] l_Sprites = Resources.LoadAll<Sprite>("Sprites/Creations/" + m_SaveData.enchancement + "/Sprite");
        m_AvatarImage.sprite = l_Sprites[8];
    }
}
