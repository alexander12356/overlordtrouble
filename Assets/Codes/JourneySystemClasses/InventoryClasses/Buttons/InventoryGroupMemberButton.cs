using UnityEngine;
using UnityEngine.UI;

public class InventoryGroupMemberButton : PanelButton
{
    private static InventoryGroupMemberButton m_Prefab = null;
    private Image m_PlayerImage = null;
    private Text m_HealthText = null;
    private Text m_SpecialText = null;
    private Image m_HealthPointBar = null;
    private Image m_SpecialPointBar = null;
    private TestGroupMemberData m_MemberData = null;

    public static InventoryGroupMemberButton prefab
    {
        get
        {
            if(m_Prefab == null)
            {
                m_Prefab = Resources.Load<InventoryGroupMemberButton>("Prefabs/Button/GroupMemberButton");
            }
            return m_Prefab;
        }
    }

    public Image playerImage
    {
        get
        {
            if(m_PlayerImage == null)
            {
                m_PlayerImage = transform.GetChild(1).gameObject.GetComponent<Image>();
            }
            return m_PlayerImage;
        }
        set
        {
            m_PlayerImage = value;
        }
    }

    public new Image selectedImage
    {
        get
        {
            if (m_SelectedImage == null)
            {
                m_SelectedImage = transform.GetChild(0).gameObject.GetComponent<Image>();
            }
            return m_SelectedImage;
        }
    }

    public TestGroupMemberData testMemberData
    {
        get
        {
            return m_MemberData;
        }
        set
        {
            m_MemberData = value;

            m_HealthText.text = m_MemberData.m_Health + "/" + m_MemberData.m_BaseHealth;
            Vector3 l_HealthBarScale = m_HealthPointBar.transform.localScale;
            l_HealthBarScale.x = m_MemberData.m_Health / m_MemberData.m_BaseHealth;
            m_HealthPointBar.transform.localScale = l_HealthBarScale;

            m_SpecialText.text = m_MemberData.m_SpecialPoints + "/" + m_MemberData.m_BaseSpecialPoints;
            Vector3 l_SpecialBarScale = m_SpecialPointBar.transform.localScale;
            l_SpecialBarScale.x = m_MemberData.m_SpecialPoints / m_MemberData.m_BaseSpecialPoints;
            m_SpecialPointBar.transform.localScale = l_SpecialBarScale;
        }
    }

    public override void Awake()
    {
        m_PlayerImage = playerImage;
        m_SelectedImage = selectedImage;

        playerImage.gameObject.SetActive(true);
        selectedImage.gameObject.SetActive(false);

        GameObject l_PlayerStatInfo = transform.FindChild("Info").gameObject;

        m_HealthText = l_PlayerStatInfo.transform.FindChild("HealthBarText").GetComponent<Text>();
        m_SpecialText = l_PlayerStatInfo.transform.FindChild("SpecialBarText").GetComponent<Text>();
        m_HealthPointBar = l_PlayerStatInfo.transform.FindChild("HealthBarBackground").GetComponent<Image>();
        m_SpecialPointBar = l_PlayerStatInfo.transform.FindChild("SpecialBarBackground").GetComponent<Image>();
    }

    public override void Select(bool p_Value)
    {
        m_Selected = p_Value;
        selectedImage.gameObject.SetActive(m_Selected);
    }
}
