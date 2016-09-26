using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LocalizationDataBase : Singleton<LocalizationDataBase>
{
    #region Variables
    private Dictionary<string, string> m_Texts = null;
	private const string m_PathFile = "Data/Localization";
    #endregion

    #region Interface
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

    public string GetText(string p_Id, string p_Value)
    {
        if (!m_Texts.ContainsKey(p_Id))
        {
            Debug.LogWarning("Cannot find localization of " + p_Id);
            return p_Id;
        }

        string l_Text = m_Texts[p_Id];

        l_Text = l_Text.Replace("%d", p_Value);
        return l_Text;
    }
    #endregion

    #region Private
    private string DetectLanguage()
	{
		string l_LangId = "en";
		switch (Application.systemLanguage)
		{
			case SystemLanguage.English:
				l_LangId = "en";
				break;
			case SystemLanguage.Russian:
				l_LangId = "ru";
				break;
		}
		return l_LangId;
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

			m_Texts.Add(textID, curText.InnerText);
		}
	}
    #endregion
}
