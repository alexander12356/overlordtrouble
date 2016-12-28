using System.Collections.Generic;

public struct DialogNode
{
    public static DialogNode zero = new DialogNode("", new List<string>(), new List<string>());

    public string id;
    public List<string> textList;
    public List<string> questionList;

    public DialogNode(string p_Id, List<string> p_TextList, List<string> p_QuestionList)
    {
        id = p_Id;
        textList = p_TextList;
        questionList = p_QuestionList;
    }

    public void Init()
    {
        for (int i = 0; i < textList.Count; i++)
        {
            textList[i] = LocalizationDataBase.GetInstance().GetText(textList[i]);
            if (textList[i].Contains("%PlayerName"))
            {
                textList[i] = textList[i].Replace("%PlayerName", PlayerData.GetInstance().GetPlayerName());
            }
        }
    }
}

public class DialogData
{
    private string m_Id = string.Empty;
    private string m_StartNodeId = string.Empty;
    private string m_AvatarImagePath = string.Empty;
    private Dictionary<string, DialogNode> m_DialogNodes = new Dictionary<string, DialogNode>();

    public string id
    {
        get { return m_Id; }
    }
    public string avatarImagePath
    {
        get { return m_AvatarImagePath;  }
    }

    public DialogData(string p_Id, string p_StartNodeId, string p_AvatarImagePath)
    {
        m_Id = p_Id;
        m_StartNodeId = p_StartNodeId;
        m_AvatarImagePath = p_AvatarImagePath;
    }

    public void AddDialogNode(string p_Id, DialogNode p_DialogNode)
    {
        m_DialogNodes.Add(p_Id, p_DialogNode);
    }

    public DialogNode GetDialogNode(string p_Id)
    {
        return m_DialogNodes[p_Id];
    }

    public DialogNode GetStartDialogNode()
    {
        return m_DialogNodes[m_StartNodeId];
    }

    public bool HasDialogNode(string p_Id)
    {
        return m_DialogNodes.ContainsKey(p_Id);
    }
}
