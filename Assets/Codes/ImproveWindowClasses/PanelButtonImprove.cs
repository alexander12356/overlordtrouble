using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class PanelButtonImprove : PanelButton
{
    private Image m_SelectedBackground;
    private Image m_ImproveAvatarImage;
    private ImproveData m_ImproveData;
    private Transform m_Transform = null;
    private Animator  m_Animator  = null;
    private bool m_Chosen = false;
    private PanelActionHandler m_ShowProfileAction = null;

    [SerializeField]
    private Vector2 m_ChoosePosition = Vector2.zero;

    [SerializeField]
    private float m_ChooseSpeed = 5.0f;

    [SerializeField]
    private Vector2 m_AwayPosition = Vector2.zero;

    [SerializeField]
    private float m_AwaySpeed = 8.0f;

    public ImproveData improveData
    {
        get { return m_ImproveData; }
        set
        {
            m_ImproveData = value;
            title = m_ImproveData.id;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_SelectedBackground = transform.FindChild("ImproveImageBackground").GetComponent<Image>();
        m_ImproveAvatarImage = transform.FindChild("ImproveMask").GetComponentInChildren<Image>();
        m_Animator = GetComponent<Animator>();
        m_Transform = transform;
    }

    public override void Select(bool p_Value)
    {
        base.Select(p_Value);

        if (m_Chosen)
        {
            return;
        }

        if (p_Value)
        {
            m_SelectedBackground.sprite = Resources.Load<Sprite>("Sprites/GUI/ImproveClass/SelectedClass");
        }
        else
        {
            m_SelectedBackground.sprite = Resources.Load<Sprite>("Sprites/GUI/ImproveClass/DeselectClass");
        }
    }

    public void Choose()
    {
        m_Chosen = true;
        m_SelectedBackground.sprite = Resources.Load<Sprite>("Sprites/GUI/ImproveClass/СhosenСlass");
        StartCoroutine(Selecting());
    }

    public void Away()
    {
        StartCoroutine(Awaying());
    }

    // Called from Animation
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

    private IEnumerator Selecting()
    {
        Vector2 m_CurrentPosition = m_Transform.localPosition;
        while ((m_CurrentPosition - m_ChoosePosition).sqrMagnitude > 0.05)
        {
            m_CurrentPosition = Vector2.Lerp(m_CurrentPosition, m_ChoosePosition, Time.deltaTime * m_ChooseSpeed);

            m_Transform.localPosition = m_CurrentPosition;
            yield return new WaitForEndOfFrame();
        }
        m_Transform.localPosition = m_ChoosePosition;

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
}
