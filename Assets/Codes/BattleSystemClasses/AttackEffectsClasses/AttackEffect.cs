using System;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    private Animator m_Animator = null;
    private Transform m_Renderer = null;
    private AudioSource m_AudioSource = null;
    private BattleActor m_BattleActor = null;
    private AttackEffectType m_Type = AttackEffectType.Instance;

    [SerializeField]
    private string m_Id = string.Empty;

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
    public AttackEffectType type
    {
        get { return m_Type;  }
        set { m_Type = value; }
    }

    public void Awake()
    {
        m_Animator = myAnimator;
        m_Renderer = enemyRendererTransform;
        m_AudioSource = audioSource;
    }

    // Called from animation
    public virtual void EndAnimation()
    {
        AttackEffectsSystem.GetInstance().EndAnimation();
        Destroy(gameObject);
    }

    public virtual void PlayEffect()
    {
        myAnimator.SetTrigger("Start");
    }

    public void SetTarget(BattleActor p_Target)
    {
        m_BattleActor = p_Target;
    }

    // Called from animation
    public void PlaySound()
    {
        m_AudioSource.PlayOneShot(AudioDataBase.GetInstance().GetAudioClip(m_Id));
        m_BattleActor.PlayHitSound();
    }
}
