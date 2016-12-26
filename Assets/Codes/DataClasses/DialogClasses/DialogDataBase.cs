using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public struct Dialog
{
    public string avatarImagePath;
    public List<SubDialog> subDialogs;
}

public struct SubDialog
{
    public List<string> phrases;

    public SubDialog(List<string> p_Phrases)
    {
        phrases = p_Phrases;
    }

    public void Init()
    {
        for (int i = 0; i < phrases.Count; i++)
        {
            if (phrases[i].Contains("%PlayerName"))
            {
                phrases[i] = phrases[i].Replace("%PlayerName", PlayerData.GetInstance().GetPlayerName());
            }
        }
    }
}

public class DialogDataBase : Singleton<DialogDataBase>
{
    private string m_PathFile = "Data/DialogList";
    private Dictionary<string, Dialog> m_DialogList = new Dictionary<string, Dialog>();

    public DialogDataBase ()
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

            string l_DialogId = l_DialogNode.Attributes[0].Value;
            string l_AvatarImage = l_DialogNode.Attributes[1].Value;
            Dialog l_Dialog;
            l_Dialog.avatarImagePath = l_AvatarImage;
            l_Dialog.subDialogs = new List<SubDialog>();

            if (l_DialogNode.ChildNodes[0].Name.ToLower() == "subdialog")
            {
                foreach (XmlNode l_Node in l_DialogNode.ChildNodes)
                {
                    l_TextList = ParsePhrases(l_Node);

                    SubDialog l_Subdialog = new SubDialog(l_TextList);
                    l_Dialog.subDialogs.Add(l_Subdialog);
                }
            }
            else
            {
                l_TextList = ParsePhrases(l_DialogNode);

                SubDialog l_Subdialog = new SubDialog(l_TextList);
                l_Dialog.subDialogs.Add(l_Subdialog);
            }

            m_DialogList.Add(l_DialogId, l_Dialog);
        }
    }

    private List<string> ParsePhrases(XmlNode p_SubdialogNode)
    {
        List<string> l_TextList = new List<string>();

        foreach (XmlNode l_TextNode in p_SubdialogNode.ChildNodes)
        {
            l_TextList.Add(LocalizationDataBase.GetInstance().GetText(l_TextNode.InnerText));
        }
        return l_TextList;
    }
}
