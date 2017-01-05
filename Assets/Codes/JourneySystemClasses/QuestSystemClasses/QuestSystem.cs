using System.Collections.Generic;

public class QuestSystem : Singleton<QuestSystem>
{
    private Dictionary<string, BaseQuest> m_Quests = new Dictionary<string, BaseQuest>();

    public void AddQuest(BaseQuest p_Quest)
    {
        m_Quests.Add(p_Quest.id, p_Quest);
    }

    public bool HasQuest(string p_QuestId)
    {
        return m_Quests.ContainsKey(p_QuestId);
    }

    public void CompleteQuest(string p_QuestId)
    {
        m_Quests[p_QuestId].Complete();
    }

    public void RemoveQuest(string p_QuestId)
    {
        m_Quests.Remove(p_QuestId);
    }

    public bool HasCompleted(string p_QuestId)
    {
        return m_Quests[p_QuestId].complete;
    }
}
