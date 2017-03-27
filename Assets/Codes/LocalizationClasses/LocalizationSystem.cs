using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LocalizationDataBase : Singleton<LocalizationDataBase>
{
    #region Variables
    private Dictionary<string, string> m_Texts = null;
	private const string m_PathFile = "Data/Localizations/Localization";
    private string m_CurrentLanguageId = string.Empty;
    #endregion

    #region Interface
    public string currentLanguage
    {
        get { return m_CurrentLanguageId; }
    }

    public LocalizationDataBase()
	{
		string l_LangId = DetectLanguage();
		Parse(l_LangId);
	}

    public string GetText(string p_Id)
    {
        if (!m_Texts.ContainsKey(p_Id))
        {
            Debug.LogWarning("Cannot find localization of " + p_Id);
            return p_Id;
        }
        return m_Texts[p_Id];
    }

    public string GetText(string p_Id, string[] p_Value)
    {
        if (!m_Texts.ContainsKey(p_Id))
        {
            Debug.LogWarning("Cannot find localization of " + p_Id);
            return p_Id;
        }

        try
        {
            string l_Text = m_Texts[p_Id];

            for (int i = 0; i < p_Value.Length; i++)
            {
                int l_Index = l_Text.IndexOf("%d");
                l_Text = l_Text.Remove(l_Index, 2);
                l_Text = l_Text.Insert(l_Index, p_Value[i]);
            }

            return l_Text;
        }
        catch
        {
            Debug.LogError("id: " + p_Id + " !ERROR!");
            return "id: " + p_Id + " !ERROR!";
        }
    }

    public void ChangeLanguage(string p_LangId)
    {
        m_CurrentLanguageId = p_LangId;
        Parse(m_CurrentLanguageId);
    }

    public List<string> GetLanguages()
    {
        return new List<string>() { "en", "ru" };
    }
    #endregion

    #region Private
    private string DetectLanguage()
	{
		switch (Application.systemLanguage)
		{
			case SystemLanguage.English:
                m_CurrentLanguageId = "en";
				break;
			case SystemLanguage.Russian:
                m_CurrentLanguageId = "ru";
				break;
            default:
                m_CurrentLanguageId = "en";
                break;
		}
        return m_CurrentLanguageId;
	}

	private void Parse(string langId)
	{
		m_Texts = null;
		m_Texts = new Dictionary<string, string>();

		TextAsset _lta = (TextAsset)Resources.Load(m_PathFile + "_" + langId);
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.InnerXml = _lta.text;
		XmlNodeList _texts = xmlDoc.GetElementsByTagName("Text");

		foreach (XmlNode curText in _texts)
        {
			string textID = "";
			foreach (XmlAttribute curTextAttr in curText.Attributes)
			{
				switch (curTextAttr.Name.ToLower())
				{
					case "id":
					{
						textID = curTextAttr.Value;
						break;
					}
				}
			}

            m_Texts.Add(textID, curText.InnerXml.Replace("\\n", Environment.NewLine));
		}
	}
    #endregion
}
