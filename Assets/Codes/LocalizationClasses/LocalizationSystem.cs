using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LocalizationDataBase : Singleton<LocalizationDataBase>
{
    #region Variables
    private Dictionary<string, string> texts = null;
	private const string pathFile = "Data/Localization";
    #endregion

    #region Interface
    public LocalizationDataBase()
	{
		string langId = DetectLanguage();
		Parse(langId);
	}

    public string GetText(string ID)
    {
        if (!texts.ContainsKey(ID))
        {
            Debug.LogWarning("Cannot find localization of " + ID);
            return ID;
        }
        return texts[ID];
    }
    #endregion

    #region Private
    private string DetectLanguage()
	{
		string langId = "en";
		switch (Application.systemLanguage)
		{
			case SystemLanguage.English:
				langId = "en";
				break;
			case SystemLanguage.Russian:
				langId = "ru";
				break;
		}
		return langId;
	}

	private void Parse(string langId)
	{
		texts = null;
		texts = new Dictionary<string, string>();

		TextAsset _lta = (TextAsset)Resources.Load(pathFile + "_" + langId);
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

			texts.Add(textID, curText.InnerText);
		}
	}
    #endregion
}
