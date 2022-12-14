using UnityEngine;
using UnityEngine.UI;

public class SpecialUpgradeIcon : MonoBehaviour
{
    #region Variables
    private static SpecialUpgradeIcon m_Prefab = null;
    private KeyCode m_Key= KeyCode.UpArrow;
    private Image m_WrongImage     = null;
    private Image m_ArrowImage     = null;
    private Image m_SelectImage    = null;
    private Image m_TextBackground = null;
    private Text  m_Text        = null;
    private bool  m_Wrong       = false;
    private bool  m_Selected    = false;
    private bool m_IsBuffed = false;
    private string m_SpecialId = string.Empty;
    private Animator m_Animator = null;
    private Special m_Special = null;
    #endregion

    #region Interface
    public static SpecialUpgradeIcon prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<SpecialUpgradeIcon>("Prefabs/SpecialUpgradeIcon");
            }
            return m_Prefab;
        }
    }
    public KeyCode arrowKey
    {
        get { return m_Key;  }
        set
        {
            m_Key = value;
            SetKey(m_Key);
        }
    }
    public bool isWrong
    {
        get { return m_Wrong; }
    }
    public bool isBuffed
    {
        get { return m_IsBuffed; }
    }
    public bool select
    {
        get { return m_Selected; }
        set
        {
            m_Selected = value;
            m_SelectImage.gameObject.SetActive(m_Selected);
        }
    }
    public string specialId
    {
        get { return m_SpecialId;  }
        set { m_SpecialId = value; }
    }

    public void SetTitle(string p_Title)
    {
        m_Text.text = p_Title;
    }

    public void Upgrade()
    {
        Color l_Color;
        ColorUtility.TryParseHtmlString("#004E0DFF", out l_Color);

        if (m_IsBuffed)
        {
            PopUpText l_PopUpText = Instantiate(PopUpText.prefab);
            l_PopUpText.transform.SetParent(transform);
            l_PopUpText.transform.localPosition = Vector3.zero;
            l_PopUpText.transform.localScale    = Vector3.one;
        }
        else
        {
            m_TextBackground.sprite = Resources.Load<Sprite>("Sprites/GUI/BattleSystem/UsingMonstyle/SpecialIcon/UpgradedSpecialIcon");
            m_IsBuffed = true;
        }
        
        m_Special.Upgrade();

        m_Animator.SetTrigger("Upgrade");
    }

    public void Wrong()
    {
        m_Wrong = true;
        m_IsBuffed = false;
        
        m_ArrowImage.gameObject.SetActive(false);
        m_SelectImage.gameObject.SetActive(false);
        m_WrongImage.gameObject.SetActive(true);

        m_TextBackground.sprite = Resources.Load<Sprite>("Sprites/GUI/BattleSystem/UsingMonstyle/SpecialIcon/MissedSpecialIcon");
        m_Animator.SetTrigger("Wrong");
    }

    public Special GetSpecial()
    {
        return m_Special;
    }
    #endregion

    #region Private
    private void Awake()
    {
        InitComponents();
    }

    private void Start()
    {
        m_Special = SpecialDataBase.GetInstance().GetSpecialData(m_SpecialId).CreateSpecial();
    }

    private void InitComponents()
    {
        Image[] l_Images = GetComponentsInChildren<Image>(true);
        m_TextBackground = l_Images[0];
        m_WrongImage     = l_Images[1];
        m_SelectImage    = l_Images[2];
        m_ArrowImage     = l_Images[3];

        m_Text = GetComponentInChildren<Text>();
        m_Animator = GetComponent<Animator>();
    }

    private void SetKey(KeyCode p_Key)
    {
        m_Key = p_Key;

        m_ArrowImage.sprite = Resources.Load<Sprite>("Sprites/GUI/BattleSystem/SpecialUpgradeIcon/SpecialArrow");
        switch (p_Key)
        {
            case KeyCode.UpArrow:
                m_ArrowImage.transform.localRotation = Quaternion.Euler(Vector3.zero);
                break;
            case KeyCode.RightArrow:
                m_ArrowImage.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 270.0f));
                break;
            case KeyCode.LeftArrow:
                m_ArrowImage.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 90.0f));
                break;
            case KeyCode.DownArrow:
                m_ArrowImage.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 180.0f));
                break;
        }
    }
    #endregion
}
