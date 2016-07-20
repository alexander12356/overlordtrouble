using UnityEngine;
using UnityEngine.UI;

public class PopUpText : MonoBehaviour
{
    #region Variables
    private static PopUpText m_Instance = null;
    private Animator m_Animator = null;
    private Text m_Text = null;
    #endregion

    #region Interface
    public static PopUpText prefab
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = Resources.Load<PopUpText>("Prefabs/PopUpText/PopUpText");
            }
            return m_Instance;
        }
    }
    public Text text
    {
        get
        {
            return m_Text;
        }
    }

    public void EndPopUp()
    {
        Destroy(gameObject);
    }

    public void SetText(string p_Text)
    {
        m_Text.text = p_Text;
    }
    #endregion

    #region Private
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Text     = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        m_Animator.SetTrigger("PopUp");
    }
    #endregion
}
