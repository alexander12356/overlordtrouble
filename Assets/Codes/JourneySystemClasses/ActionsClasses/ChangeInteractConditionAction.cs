using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ConditionPair
{
    public string m_SenderBehaviorId;
    public string m_TargetBehaviorId;
}

public class ChangeInteractConditionAction : MonoBehaviour
{
    [SerializeField]
    private JourneyActor m_TargetActor = null;

    [SerializeField]
    List<ConditionPair> m_ConditionPair = null;

    public void Run()
    {
        for (int i = 0; i < m_ConditionPair.Count; i++)
        {
            if (m_ConditionPair[i].m_SenderBehaviorId == m_TargetActor.interactBehaviorId)
            {
                m_TargetActor.ChangeInteractionBehavior(m_ConditionPair[i].m_TargetBehaviorId);
                break;
            }
        }
    }
}
