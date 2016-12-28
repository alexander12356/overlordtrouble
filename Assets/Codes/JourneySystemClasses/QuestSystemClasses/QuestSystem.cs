using System.Collections.Generic;

public class QuestSystem : Singleton<QuestSystem>
{
    private Dictionary<string, bool> m_Quests = new Dictionary<string, bool>();

    public void AddQuest(string p_QuestId)
    {
        m_Quests.Add(p_QuestId, true);
    }

    public bool HasQuest(string p_QuestId)
    {
        return m_Quests.ContainsKey(p_QuestId);
    }

    public void CompleteQuest(string p_QuestId)
    {
        m_Quests.Remove(p_QuestId);
    }
}
