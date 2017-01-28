using System.Collections;

using UnityEngine;

public class SecretPassTrigger : MonoBehaviour
{
    private CheckCollide m_CheckCollide = null;

    [SerializeField]
    private SpriteRenderer m_TargetRenderer = null;

    [SerializeField]
    private float m_TargetAplha = 0.2f;
	
    public void Awake()
    {
        m_CheckCollide = GetComponentInChildren<CheckCollide>();
    }

    public void Start()
    {
        m_CheckCollide.AddCollideEnterAction(StartAppear);
        m_CheckCollide.AddCollideExitAction(StartFade);
    }

    private void StartAppear(JourneyActor p_Actor)
    {
        StopAllCoroutines();
        StartCoroutine(Appearing());
    }

    private void StartFade(JourneyActor p_Actor)
    {
        StopAllCoroutines();
        StartCoroutine(Fading());
    }

    private IEnumerator Appearing()
    {
        while (m_TargetRenderer.color.a > m_TargetAplha)
        {
            float l_Alpha = m_TargetRenderer.color.a;
            l_Alpha = Mathf.MoveTowards(l_Alpha, m_TargetAplha, Time.deltaTime);

            Color l_Color = m_TargetRenderer.color;
            l_Color.a = l_Alpha;
            m_TargetRenderer.color = l_Color;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Fading()
    {
        while (m_TargetRenderer.color.a < 1.0f)
        {
            float l_Alpha = m_TargetRenderer.color.a;
            l_Alpha = Mathf.MoveTowards(l_Alpha, 1.0f, Time.deltaTime);

            Color l_Color = m_TargetRenderer.color;
            l_Color.a = l_Alpha;
            m_TargetRenderer.color = l_Color;

            yield return new WaitForEndOfFrame();
        }
    }
}
