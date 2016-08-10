using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public struct Dialog
{
    public string avatarImagePath;
    public List<string> phrases;
}

public class DialogSystem : Singleton<DialogSystem>
{
    private string m_PathFile = "Data/DialogList";
    private bool isDialogStarting = false;
    private Dictionary<string, Dialog> m_DialogList = new Dictionary<string, Dialog>();

    public DialogSystem ()
    {
        Parse();
    }

    public Dialog GetDialog(string p_Id)
    {
        return m_DialogList[p_Id];
    }

    private void Parse()
    {
        TextAsset l_TextAsset = (TextAsset)Resources.Load(m_PathFile);
        XmlDocument l_XmlDocument = new XmlDocument();
        l_XmlDocument.InnerXml = l_TextAsset.text;
        XmlNodeList l_DialogListNode = l_XmlDocument.GetElementsByTagName("Dialog");
        
        foreach (XmlNode l_DialogNode in l_DialogListNode)
        {
            List<string> l_TextList = new List<string>();

            foreach (XmlNode l_TextNode in l_DialogNode.ChildNodes)
            {
                l_TextList.Add(l_TextNode.InnerText);
            }
            
            string l_DialogId    = l_DialogNode.Attributes[0].Value;
            string l_AvatarImage = l_DialogNode.Attributes[1].Value;
            Dialog l_Dialog;
            l_Dialog.avatarImagePath = l_AvatarImage;
            l_Dialog.phrases = l_TextList;

            m_DialogList.Add(l_DialogId, l_Dialog);
        }
    }
}
