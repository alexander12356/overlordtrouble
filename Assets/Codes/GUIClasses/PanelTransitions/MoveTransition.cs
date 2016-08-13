using UnityEngine;
using System.Collections;

public class MoveTransition : BaseTransition
{
    private float m_ShowingSpeed = 1400.0f;

    public override void Show()
    {
        base.Show();

        StartCoroutine(Showing());
    }

    public override void Hide()
    {
        base.Hide();

        StartCoroutine(Hiding());
    }

    private IEnumerator Showing()
    {
        m_IsMoving = true;
        m_PanelTransform.localPosition = new Vector3(1500.0f, 0.0f, 0.0f);
        m_PanelTransform.localScale = Vector3.one;
        Vector3 l_Position = m_PanelTransform.localPosition;

        while (m_PanelTransform.localPosition.x >= 0)
        {
            l_Position.x -= m_ShowingSpeed * Time.deltaTime;
            m_PanelTransform.localPosition = l_Position;
            yield return new WaitForEndOfFrame();
        }
        m_PanelTransform.localPosition = Vector3.zero;
        m_IsMoving = false;

        EndShowing();
    }

    private IEnumerator Hiding()
    {
        m_IsMoving = true;
        Vector3 l_Position = m_PanelTransform.localPosition;
        while (m_PanelTransform.localPosition.x >= -1180)
        {
            l_Position.x -= m_ShowingSpeed * Time.deltaTime;
            m_PanelTransform.localPosition = l_Position;
            yield return new WaitForEndOfFrame();
        }

        m_IsMoving = false;
        m_PanelTransform.localPosition = new Vector3(-1180.0f, 0.0f, 0.0f);

        EndHiding();
    }
}
