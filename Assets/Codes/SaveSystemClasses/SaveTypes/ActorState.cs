using UnityEngine;

public class ActorState
{
    private string m_Id;
    private string m_InteractBehavior = string.Empty;
    private string m_MovementBehavior = string.Empty;
    private Vector3 m_Position = Vector3.zero;

    public string id
    {
        get { return m_Id; }
        set { m_Id = value; }
    }
    public string interactBehavior
    {
        get { return m_InteractBehavior; }
        set { m_InteractBehavior = value; }
    }
    public string movementBehavior
    {
        get { return m_MovementBehavior; }
        set { m_MovementBehavior = value; }
    }
    public Vector3 position
    {
        get { return m_Position; }
        set { m_Position = value; }
    }

    public ActorState(string p_Id, string p_InteractBehavior, string p_MovementBehavior, Vector3 p_Position)
    {
        id = p_Id;
        interactBehavior = p_InteractBehavior;
        movementBehavior = p_MovementBehavior;
        position = p_Position;
    }

    public virtual JSONObject GetJson()
    {
        JSONObject l_ActoStateJson = new JSONObject();
        l_ActoStateJson.AddField("Id", id);
        l_ActoStateJson.AddField("InteractBehavior", interactBehavior);
        l_ActoStateJson.AddField("MovementBehavior", movementBehavior);

        JSONObject l_ActorPositionJson = new JSONObject();
        l_ActorPositionJson.AddField("X", position.x);
        l_ActorPositionJson.AddField("Y", position.y);
        l_ActoStateJson.AddField("Position", l_ActorPositionJson);

        return l_ActoStateJson;
    }
}
