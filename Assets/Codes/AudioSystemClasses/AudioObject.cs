using System.Collections;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
    private static AudioObject m_Prefab = null;
    private AudioSource m_AudioSource = null;
    private float m_Speed = 1.0f;

    public static AudioObject prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<AudioObject>("Prefabs/AudioObject");
            }
            return m_Prefab;
        }
    }

    public void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip p_AudioClip)
    {
        StopAllCoroutines();
        m_AudioSource.clip = p_AudioClip;
        m_AudioSource.volume = 0.0f;
        m_AudioSource.Play();
        StartCoroutine(Appearance());
    }

    public void ChangeVolume(float p_Value)
    {
        StopAllCoroutines();
        StartCoroutine(ChangingVolume(p_Value));
    }

    public void Stop()
    {
        StopAllCoroutines();
        StartCoroutine(Fade());
    }

    private IEnumerator Appearance()
    {
        while (m_AudioSource.volume < 1)
        {
            m_AudioSource.volume += m_Speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Fade()
    {
        while (m_AudioSource.volume > 0)
        {
            m_AudioSource.volume -= m_Speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    private IEnumerator ChangingVolume(float p_Value)
    {
        while (m_AudioSource.volume != p_Value)
        {
            m_AudioSource.volume = Mathf.MoveTowards(m_AudioSource.volume, p_Value, m_Speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
