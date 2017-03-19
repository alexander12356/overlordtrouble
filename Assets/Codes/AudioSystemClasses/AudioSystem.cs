using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    private static AudioSystem m_Instance = null;
    private Dictionary<string, AudioObject> m_MusicList = new Dictionary<string, AudioObject>();
    private Dictionary<string, AudioObject> m_SoundList = new Dictionary<string, AudioObject>();
    private string m_ThemeId = string.Empty;
    private float m_MusicVolume = 1.0f;
    private float m_SoundVolume = 1.0f;
    private AudioSource m_SoundAudioSource = null;

    public static AudioSystem GetInstance()
    {
        return m_Instance;
    }
    public float musicVolume
    {
        get { return m_MusicVolume; }
        set { m_MusicVolume = value; }
    }
    public float soundVolume
    {
        get { return m_SoundVolume; }
        set { m_SoundVolume = value; }
    }

    public void Awake()
    {
        m_Instance = this;

        m_SoundAudioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(string p_Id, bool p_IsLooped = true)
    {
        AudioClip l_AudioClip = AudioDataBase.GetInstance().GetAudioClip(p_Id);
        if (l_AudioClip == null || m_MusicList.ContainsKey(p_Id))
        {
            return;
        }

        AudioObject l_AudioObject = Instantiate(AudioObject.prefab);
        l_AudioObject.transform.SetParent(transform);
        l_AudioObject.Play(l_AudioClip, p_IsLooped);
        m_MusicList.Add(p_Id, l_AudioObject);

        Debug.Log("Play Music:" + p_Id);
    }

    public void PlayTheme()
    {
        if (!m_MusicList.ContainsKey(m_ThemeId))
        {
            PlayMusic(m_ThemeId);
        }
    }

    public void StopTheme()
    {
        StopMusic(m_ThemeId);
    }

    public void ChangeVolume(string p_Id, float p_Value)
    {
        m_MusicList[p_Id].ChangeVolume(p_Value);
    }

    public void StopMusic(string p_Id)
    {
        m_MusicList[p_Id].Stop();
        m_MusicList.Remove(p_Id);
    }

    public void SetTheme(string p_ThemeId)
    {
        m_ThemeId = p_ThemeId;
    }

    public void ChangeThemeVolume(float p_Volume)
    {
        m_MusicList[m_ThemeId].ChangeVolume(p_Volume);
    }

    public void PlaySound(string p_Id)
    {
        AudioClip l_AudioClip = AudioDataBase.GetInstance().GetAudioClip(p_Id);
        if (l_AudioClip == null)
        {
            return;
        }
        m_SoundAudioSource.PlayOneShot(l_AudioClip);

        Debug.Log("Play Sound:" + p_Id);
    }

    public void ResumeMainTheme()
    {
        if (RoomSystem.GetInstance().currentRoomIsMain)
        {
            ChangeThemeVolume(1.0f);
        }
        else
        {
            ChangeThemeVolume(0.5f);
        }
    }

    public void ChangeSoundVolume(float p_Value)
    {
    }

    public void ChangeMusicVolume(float p_Value)
    {
        if (m_MusicVolume == 0)
        {

        }
        foreach (AudioObject l_Audio in m_MusicList.Values)
        {
            float l_Coeff = l_Audio.volume / m_MusicVolume;
            if (p_Value == 0.0f)
            {
                l_Audio.mute = true;
            }
            else
            {
                l_Audio.mute = false;
                l_Audio.volume = l_Coeff * p_Value;
            }
        }
        m_MusicVolume = p_Value > 0 ? p_Value : m_MusicVolume;
    }
}
