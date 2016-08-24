using UnityEngine;
using System.Collections;

public class ScaleTransition : BaseTransition
{
    private float m_ShowingTime = 0.6f;
    private float m_Timer = 0.0f;

    public override void Show()
    {
        StartCoroutine(Showing());
    }

    private IEnumerator Showing()
    {
        m_IsMoving = true;
        
        while (m_Timer < m_ShowingTime)
        {
            m_Timer += Time.deltaTime;

            float l_Scale = m_Timer / m_ShowingTime;
            m_PanelTransform.localScale = new Vector3(l_Scale, l_Scale, l_Scale);

            yield return new WaitForEndOfFrame();
        }
        m_Timer = 0.0f;
        m_IsMoving = false;

        EndShowing();
    }
}
