using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DialogSystem : Singleton<DialogSystem>
{
    private string m_PathFile = "Data/DialogList";
    private bool isDialogStarting = false;
    private Dictionary<string, List<string>> m_DialogList = new Dictionary<string, List<string>>();

    public DialogSystem ()
    {
        Parse();
    }

    public List<string> GetDialog(string p_Id)
    {
        return m_DialogList[p_Id];
    }

    private void Parse()
    {
        TextAsset l_TextAsset = (TextAsset)Resources.Load(m_PathFile);
        XmlDocument l_XmlDocument = new XmlDocument();
        l_XmlDocument.InnerXml = l_TextAsset.text;
        XmlNodeList l_DialogList = l_XmlDocument.GetElementsByTagName("Dialog");
        
        foreach (XmlNode l_Dialog in l_DialogList)
        {
            List<string> l_TextList = new List<string>();

            foreach (XmlNode l_Text in l_Dialog.ChildNodes)
            {
                l_TextList.Add(l_Text.InnerText);
            }

            m_DialogList.Add(l_Dialog.Attributes[0].Value, l_TextList);
        }
    }
}
