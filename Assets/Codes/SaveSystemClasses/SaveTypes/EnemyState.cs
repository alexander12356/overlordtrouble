using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : ActorState
{
    private string m_WinBehaviorId;
    private string m_LoseBehaviorId;
    
    public string winBehavior
    {
        get { return m_WinBehaviorId; }
        set { m_WinBehaviorId = value; }
    }
    public string loseBehavior
    {
        get { return m_LoseBehaviorId; }
        set { m_LoseBehaviorId = value; }
    }

    public EnemyState(string p_Id, string p_InteractBehavior, string p_MovementBehavior, Vector3 p_Position, string p_WinBehavior, string p_LoseBehavior) : base (p_Id, p_InteractBehavior, p_MovementBehavior, p_Position)
    {
        winBehavior = p_WinBehavior;
        loseBehavior = p_LoseBehavior;
    }

    public override JSONObject GetJson()
    {
        JSONObject l_EnemyStateJson = base.GetJson();

        l_EnemyStateJson.AddField("WinBehavior", winBehavior);
        l_EnemyStateJson.AddField("LoseBehavior", loseBehavior);

        return l_EnemyStateJson;
    }
}
