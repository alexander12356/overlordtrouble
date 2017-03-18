using UnityEngine;

public class VisualEffect : MonoBehaviour
{
    private Animator m_Animator = null;
    private Transform m_Renderer = null;
    private AudioSource m_AudioSource = null;
    private BattleActor m_Target = null;
    private Transform m_TargetRenderer = null;
    private event PanelActionHandler m_EndAction;

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
        m_Renderer = transform.FindChild("Renderer");
        m_AudioSource = audioSource;
    }

    public void Init(BattleActor p_Target, Transform p_TargetRenderer)
    {
        m_Target = p_Target;
        m_TargetRenderer = p_TargetRenderer;
    }

    // Called from animation
    public virtual void EndAnimation()
    {
        m_TargetRenderer.SetParent(transform.parent);
        Destroy(gameObject);
        EndAction();
    }

    public virtual void PlayEffect()
    {
        transform.SetParent(m_TargetRenderer.parent);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        m_TargetRenderer.SetParent(m_Renderer);
        myAnimator.SetTrigger("Start");
    }

    // Called from animation
    public void PlaySound()
    {
        m_AudioSource.PlayOneShot(AudioDataBase.GetInstance().GetAudioClip(m_Id));
        //m_Target.PlayHitSound();
    }

    public void AddEndAnimationAction(PanelActionHandler p_Action)
    {
        m_EndAction += p_Action;
    }

    private void EndAction()
    {
        if (m_EndAction != null)
        {
            m_EndAction();
        }
    }
}
