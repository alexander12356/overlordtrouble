using UnityEngine;
using System.Collections;

public class MoveAlphaTransition : BaseTransition {

    [SerializeField]
    private float m_ShowingSpeed = 1400.0f;

    [SerializeField]
    private Vector3 m_StartShowPosition = new Vector3(1500.0f, 0.0f);

    [SerializeField]
    private Vector3 m_DestShowPosition = new Vector3(0.0f, 0.0f);

    [SerializeField]
    private Vector3 m_HideDestPosition = new Vector3(-1180.0f, 0.0f);

    private CanvasGroup m_CanvasGroup = null;

    public override void Awake()
    {
        base.Awake();

        m_CanvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Show()
    {
        StartCoroutine(Showing());
    }

    public override void Hide()
    {
        StartCoroutine(Hiding());
    }

    private IEnumerator Showing()
    {
        m_IsMoving = true;

        m_PanelTransform.localPosition = m_StartShowPosition;
        float l_StartDistance = (m_PanelTransform.localPosition - m_DestShowPosition).sqrMagnitude;

        while ((m_PanelTransform.localPosition - m_DestShowPosition).sqrMagnitude >= 0.05)
        {
            m_CanvasGroup.alpha = 1 - (m_PanelTransform.localPosition - m_DestShowPosition).sqrMagnitude / l_StartDistance;

            m_PanelTransform.localPosition = Vector2.MoveTowards(m_PanelTransform.localPosition, m_DestShowPosition, m_ShowingSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        m_PanelTransform.localPosition = m_DestShowPosition;

        m_IsMoving = false;
        EndShowing();
    }

    private IEnumerator Hiding()
    {
        m_IsMoving = true;

        m_PanelTransform.localPosition = m_DestShowPosition;
        float l_StartDistance = (m_PanelTransform.localPosition - m_HideDestPosition).sqrMagnitude;

        while ((m_PanelTransform.localPosition - m_HideDestPosition).sqrMagnitude >= 0.05)
        {
            m_CanvasGroup.alpha = (m_PanelTransform.localPosition - m_HideDestPosition).sqrMagnitude / l_StartDistance;

            m_PanelTransform.localPosition = Vector2.MoveTowards(m_PanelTransform.localPosition, m_HideDestPosition, m_ShowingSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        m_PanelTransform.localPosition = m_HideDestPosition;

        m_IsMoving = false;
        EndHiding();
    }
}
