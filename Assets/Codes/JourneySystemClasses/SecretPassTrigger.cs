using System.Collections;

using UnityEngine;

public class SecretPassTrigger : MonoBehaviour
{
    private CheckCollide m_CheckCollide = null;

    [SerializeField]
    private float m_TargetAplha = 0.2f;

    [SerializeField]
    private SpriteRenderer[] m_TargetRenderer = null;
	
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
        while (GetSpriteAlpha() > m_TargetAplha)
        {
            float l_Alpha = GetSpriteAlpha();
            l_Alpha = Mathf.MoveTowards(l_Alpha, m_TargetAplha, Time.deltaTime);

            SetSpriteAlpha(l_Alpha);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Fading()
    {
        while (GetSpriteAlpha() < 1.0f)
        {
            float l_Alpha = GetSpriteAlpha();
            l_Alpha = Mathf.MoveTowards(l_Alpha, 1.0f, Time.deltaTime);

            SetSpriteAlpha(l_Alpha);

            yield return new WaitForEndOfFrame();
        }
    }

    private float GetSpriteAlpha()
    {
        return m_TargetRenderer[0].color.a;
    }

    private void SetSpriteAlpha(float p_AlphaValue)
    {
        for (int i = 0; i < m_TargetRenderer.Length; i++)
        {
            Color l_Color = m_TargetRenderer[i].color;
            l_Color.a = p_AlphaValue;
            m_TargetRenderer[i].color = l_Color;
        }
    }
}
