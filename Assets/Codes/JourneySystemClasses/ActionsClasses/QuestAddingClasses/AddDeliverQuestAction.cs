using UnityEngine;

public class AddDeliverQuestAction : MonoBehaviour
{
    [SerializeField]
    private string p_QuestId = string.Empty;

    public void AddQuest()
    {
        DeliverQuest m_Quest = new DeliverQuest(p_QuestId);
        QuestSystem.GetInstance().AddQuest(m_Quest);
    }
}
