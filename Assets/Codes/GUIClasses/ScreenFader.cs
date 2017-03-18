using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    private Animator m_Animator = null;
    private Image m_Image = null;

    private bool m_IsFading = false;
	
	private void Awake ()
    {
        m_Animator = GetComponent<Animator>();
        m_Image = GetComponent<Image>();
    }

    public IEnumerator FadeToClear()
    {
        m_IsFading = true;
        m_Animator.SetTrigger("FadeIn");

        while (m_IsFading)
        {
            yield return null;
        }
    }

    public IEnumerator FadeToBlack()
    {
        m_IsFading = true;
        m_Animator.SetTrigger("FadeOut");

        while (m_IsFading)
        {
            yield return null;
        }
    }

    // Called by Animation
    private void AnimationComplete()
    {
        m_IsFading = false;
    }

    public void ChangeColor(Color p_Color)
    {
        m_Image.color = p_Color;
    }
}
