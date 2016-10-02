using UnityEngine;

using System.Xml;
using System.Collections.Generic;

public class AudioDataBase : Singleton<AudioDataBase>
{
    private string m_PathFile = "Data/AudioList";
    private Dictionary<string, AudioClip> m_AudioClips = new Dictionary<string, AudioClip>();

    public AudioDataBase()
    {
        Parse();
    }

    public AudioClip GetAudioClip(string p_AudioId)
    {
        try
        {
            return m_AudioClips[p_AudioId];
        }
        catch
        {
            Debug.LogWarning("Cannot find AudioClip, id: " + p_AudioId);
            return null;
        }
    }

    private void Parse()
    {
        TextAsset l_TextAsset = (TextAsset)Resources.Load(m_PathFile);
        XmlDocument l_XmlDocument = new XmlDocument();
        l_XmlDocument.InnerXml = l_TextAsset.text;
        XmlNodeList l_AudioList = l_XmlDocument.GetElementsByTagName("Audio");

        foreach (XmlNode l_CurrentAudio in l_AudioList)
        {
            string l_TextID = "";
            string l_Path = "";
            foreach (XmlAttribute l_Attribute in l_CurrentAudio.Attributes)
            {
                switch (l_Attribute.Name.ToLower())
                {
                    case "id":
                        l_TextID = l_Attribute.Value;
                        break;
                    case "path":
                        l_Path = l_Attribute.Value;
                        break;
                }
            }
            m_AudioClips.Add(l_TextID, Resources.Load<AudioClip>(l_Path));
        }
    }
}
