using System;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    private Animator m_Animator = null;
    private Transform m_Renderer = null;
    private AudioSource m_AudioSource = null;

    public Animator myAnimator
    {
        get
        {
            if (m_Animator == null)
            {
                m_Animator = GetComponent<Animator>();
            }
            return m_Animator;
        }
    }
    public Transform enemyRendererTransform
    {
        get
        {
            if (m_Renderer == null)
            {
                m_Renderer = transform.FindChild("Renderer");
            }
            return m_Renderer;
        }
    }
    public AudioSource audioSource
    {
        get
        {
            if (m_AudioSource == null)
            {
                m_AudioSource = GetComponent<AudioSource>();
            }
            return m_AudioSource;
        }
    }

    public void Awake()
    {
        m_Animator = myAnimator;
        m_Renderer = enemyRendererTransform;
        m_AudioSource = audioSource;
    }

    public void EndAnimation()
    {
        AttackEffectsSystem.GetInstance().EndAnimation();
        Destroy(gameObject);
    }

    public void PlayEffect()
    {
        myAnimator.SetTrigger("Start");

        //m_AudioSource.PlayOneShot();
    }
}
