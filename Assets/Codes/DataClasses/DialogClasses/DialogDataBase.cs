using System.Collections.Generic;
using System.Xml;
using UnityEngine;



public class DialogDataBase : Singleton<DialogDataBase>
{
    private string m_PathFile = "Data/DialogList";
    private Dictionary<string, DialogData> m_DialogList = new Dictionary<string, DialogData>();

    public DialogDataBase ()
    {
        Parse();
    }

    public DialogData GetDialog(string p_Id)
    {
        return m_DialogList[p_Id];
    }

    private void Parse()
    {
        TextAsset l_TextAsset = (TextAsset)Resources.Load(m_PathFile);
        XmlDocument l_XmlDocument = new XmlDocument();
        l_XmlDocument.InnerXml = l_TextAsset.text;
        XmlNodeList l_DialogListNode = l_XmlDocument.GetElementsByTagName("Dialog");

        foreach (XmlNode l_DialogXml in l_DialogListNode)
        {
            string l_DialogId = l_DialogXml.Attributes[0].Value;
            string l_StartDialogNode = l_DialogXml.Attributes[1].Value;
            string l_AvatarImagePath = l_DialogXml.Attributes[2].Value;
            DialogData l_DialogData = new DialogData(l_DialogId, l_StartDialogNode, l_AvatarImagePath);

            foreach (XmlNode l_DialogNodeXml in l_DialogXml)
            {
                DialogNode l_DialogNode = ParseDialogNode(l_DialogNodeXml);
                //l_DialogNode.Init();

                l_DialogData.AddDialogNode(l_DialogNode.id, l_DialogNode);
            }

            m_DialogList.Add(l_DialogData.id, l_DialogData);
        }
    }

    private DialogNode ParseDialogNode(XmlNode p_DialogNodeXml)
    {
        string l_Id = p_DialogNodeXml.Attributes[0].Value;
        List<string> l_TextList = new List<string>();
        List<string> l_QuestionList = new List<string>();

        foreach (XmlNode l_Xml in p_DialogNodeXml)
        {
            switch (l_Xml.Name)
            {
                case "Question":
                    l_QuestionList = ParseQuestion(l_Xml);
                    break;
                default:
                    l_TextList.Add(l_Xml.InnerText);
                    break;
            }
        }

        return new DialogNode(l_Id, l_TextList, l_QuestionList);
    }

    private List<string> ParseQuestion(XmlNode p_DialogNodeXml)
    {
        List<string> l_QuestionList = new List<string>();

        foreach (XmlNode l_Xml in p_DialogNodeXml)
        {
            l_QuestionList.Add(l_Xml.Attributes[0].Value);
        }

        return l_QuestionList;
    }
}
