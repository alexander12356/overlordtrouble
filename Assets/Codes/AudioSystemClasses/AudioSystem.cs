using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    private static AudioSystem m_Instance = null;
    private Dictionary<string, AudioObject> m_AudioList = new Dictionary<string, AudioObject>();
    private string m_ThemeId = string.Empty;

    public static AudioSystem GetInstance()
    {
        return m_Instance;
    }

    public void Awake()
    {
        m_Instance = this;
    }

    public void PlayMusic(string p_Id)
    {
        AudioClip l_AudioClip = AudioDataBase.GetInstance().GetAudioClip(p_Id);
        if (l_AudioClip == null)
        {
            return;
        }

        AudioObject l_AudioObject = Instantiate(AudioObject.prefab);
        l_AudioObject.transform.SetParent(transform);
        l_AudioObject.Play(l_AudioClip);
        m_AudioList.Add(p_Id, l_AudioObject);


        Debug.Log("Play Music:" + p_Id);
    }

    public void PlayTheme()
    {
        if (!m_AudioList.ContainsKey(m_ThemeId))
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
        m_AudioList[p_Id].ChangeVolume(p_Value);
    }

    public void StopMusic(string p_Id)
    {
        m_AudioList[p_Id].Stop();
        m_AudioList.Remove(p_Id);
    }

    public void SetTheme(string p_ThemeId)
    {
        m_ThemeId = p_ThemeId;
    }

    public void ChangeThemeVolume(float p_Volume)
    {
        m_AudioList[m_ThemeId].ChangeVolume(p_Volume);
    }

    public void PlaySound(string p_Id)
    {

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
}
