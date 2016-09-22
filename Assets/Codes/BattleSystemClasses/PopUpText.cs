using UnityEngine;
using UnityEngine.UI;

public class PopUpText : MonoBehaviour
{
    #region Variables
    private static PopUpText m_Instance = null;
    private Animator m_Animator = null;
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

    public void EndPopUp()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Private
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        m_Animator.SetTrigger("PopUp");
    }
    #endregion
}
