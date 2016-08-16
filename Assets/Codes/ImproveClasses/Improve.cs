using UnityEngine;
using System.Collections;

public class Improve : MonoBehaviour
{
    #region Variables
    private PanelButton m_PanelButton;
    private Animator    m_Animator;
    private Transform   m_Transform;
    private float       m_SelectSpeed = 5.0f;
    private float       m_AwaySpeed   = 8.0f;
    private PanelActionHandler m_ShowProfileAction = null;

    [SerializeField]
    private Vector2     m_SelectPosition = Vector2.zero;

    [SerializeField]
    private Vector2     m_AwayPosition   = Vector2.zero;

    [SerializeField]
    private string      m_ImpoveId = "";
    #endregion

    #region Interface
    public PanelButton improveButton
    {
        get
        {
            return m_PanelButton;
        }
    }
    public string improveId
    {
        get { return m_ImpoveId; }
    }

    public void Awake()
    {
        m_PanelButton = GetComponentInChildren<PanelButton>();
        m_Animator    = GetComponent<Animator>();
        m_Transform   = transform;

        m_Animator.enabled = false;
    }

    public void Select()
    {
        StartCoroutine(Selecting());
    }

    public void Away()
    {
        StartCoroutine(Awaying());
    }

    public void ShowProfile()
    {
        if (m_ShowProfileAction != null)
        {
            m_ShowProfileAction();
        }
    }

    public void AddShowProfileAction(PanelActionHandler m_Action)
    {
        m_ShowProfileAction += m_Action;
    }
    #endregion

    #region Private
    private IEnumerator Selecting()
    {
        Vector2 m_CurrentPosition = m_Transform.localPosition;
        while ((m_CurrentPosition - m_SelectPosition).sqrMagnitude > 0.05)
        {
            m_CurrentPosition = Vector2.Lerp(m_CurrentPosition, m_SelectPosition, Time.deltaTime * m_SelectSpeed);

            m_Transform.localPosition = m_CurrentPosition;
            yield return new WaitForEndOfFrame();
        }
        m_Transform.localPosition = m_SelectPosition;

        m_Animator.enabled = true;
        m_Animator.SetTrigger("Selected");
    }

    private IEnumerator Awaying()
    {
        Vector2 m_CurrentPosition = m_Transform.localPosition;
        while ((m_CurrentPosition - m_AwayPosition).sqrMagnitude > 0.05)
        {
            m_CurrentPosition = Vector2.Lerp(m_CurrentPosition, m_AwayPosition, Time.deltaTime * m_AwaySpeed);

            m_Transform.localPosition = m_CurrentPosition;
            yield return new WaitForEndOfFrame();
        }
        m_Transform.localPosition = m_AwayPosition;
    }
    #endregion
}
