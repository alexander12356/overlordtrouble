using UnityEngine;

using System;
using System.Collections;

public class MoveTransition : BaseTransition
{
    [SerializeField]
    private float m_ShowingSpeed = 1400.0f;

    [SerializeField]
    private Vector3 m_StartShowPosition = new Vector3(1500.0f, 0.0f);

    [SerializeField]
    private Vector3 m_DestShowPosition = new Vector3(0.0f, 0.0f);

    [SerializeField]
    private Vector3 m_HideDestPosition = new Vector3(-1180.0f, 0.0f);

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
        while ((m_PanelTransform.localPosition - m_DestShowPosition).sqrMagnitude >= 0.05)
        {
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
        while ((m_PanelTransform.localPosition - m_HideDestPosition).sqrMagnitude >= 0.05)
        {
            m_PanelTransform.localPosition = Vector2.MoveTowards(m_PanelTransform.localPosition, m_HideDestPosition, m_ShowingSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        m_PanelTransform.localPosition = m_HideDestPosition;

        m_IsMoving = false;
        EndHiding();
    }
}
