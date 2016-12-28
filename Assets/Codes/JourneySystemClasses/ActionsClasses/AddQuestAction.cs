using UnityEngine;

public class AddQuestAction : MonoBehaviour
{
    [SerializeField]
    private string p_QuestId = string.Empty;

    public void AddQuest()
    {
        QuestSystem.GetInstance().AddQuest(p_QuestId);
    }
}
