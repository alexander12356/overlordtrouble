using System.Collections.Generic;

using UnityEngine;

public struct ActorState
{
    public string interactBehavior;
    public string movementBehavior;
    public Vector3 position;

    public ActorState(string p_InteractBehavior, string p_MovementBehavior, Vector3 p_Position)
    {
        interactBehavior = p_InteractBehavior;
        movementBehavior = p_MovementBehavior;
        position = p_Position;
    }
}

public class LocationSave
{
    private string m_Id = string.Empty;
    private Dictionary<string, JourneyActor> m_Actors = new Dictionary<string, JourneyActor>();
    private Dictionary<string, FrontDoor> m_Doors = new Dictionary<string, FrontDoor>();
    private Dictionary<string, GameEventTrigger> m_GameEventTriggers = new Dictionary<string, GameEventTrigger>();
    private Dictionary<string, CheckCounterAction> m_CheckCounterActions = new Dictionary<string, CheckCounterAction>();
    private Dictionary<string, ActorState> m_ActorsState = new Dictionary<string, ActorState>();
    private Dictionary<string, bool> m_DoorState = new Dictionary<string, bool>();
    private List<string> m_DieActorsIds = new List<string>();
    private List<string> m_DestroyedTriggers = new List<string>();
    private Dictionary<string, int> m_CheckCounterValues = new Dictionary<string, int>();

    public LocationSave(string p_Id)
    {
        m_Id = p_Id;
    }

    public void ClearCache()
    {
        m_Actors.Clear();
        m_Doors.Clear();
        m_GameEventTriggers.Clear();
        m_CheckCounterActions.Clear();
    }

    public void Save()
    {
        SaveActorStates();
        SaveDoorState();
    }

    public void Load()
    {
        LoadActorStates();
        LoadDiesActors();
        LoadDoorState();
        LoadDestroyedTriggers();
        LoadCheckCounterValues();
    }

    public void AddActor(JourneyActor p_JourneyActor)
    {
        m_Actors.Add(p_JourneyActor.actorId, p_JourneyActor);
    }

    public void AddDoor(FrontDoor p_FrontDoor)
    {
        m_Doors.Add(p_FrontDoor.id, p_FrontDoor);
    }

    public void AddTrigger(GameEventTrigger p_GameEventTrigger)
    {
        m_GameEventTriggers.Add(p_GameEventTrigger.id, p_GameEventTrigger);
    }

    public void AddCheckCounter(CheckCounterAction p_CheckCounterAction)
    {
        m_CheckCounterActions.Add(p_CheckCounterAction.id, p_CheckCounterAction);
    }

    public void ActorDie(string p_Id)
    {
        m_DieActorsIds.Add(p_Id);
        m_Actors.Remove(p_Id);
    }

    public void GameEventDestroy(string p_Id)
    {
        m_DestroyedTriggers.Add(p_Id);
    }

    public void SetCheckCounterValue(string p_Id, int p_Value)
    {
        if (m_CheckCounterValues.ContainsKey(p_Id))
        {
            m_CheckCounterValues[p_Id] = p_Value;
        }
        else
        {
            m_CheckCounterValues.Add(p_Id, p_Value);
        }
    }

    public void LoadFromFile(JSONObject p_LocationJson)
    {
        if (p_LocationJson.HasField("ActorsState"))
        {
            LoadFromFileActorStates(p_LocationJson["ActorsState"]);
        }
        if (p_LocationJson.HasField("DieActors"))
        {
            LoadFromFileDieActors(p_LocationJson["DieActors"]);
        }
        if (p_LocationJson.HasField("DoorsState"))
        {
            LoadFromFileDoorStates(p_LocationJson["DoorsState"]);
        }
        if (p_LocationJson.HasField("DestroyedTriggers"))
        {
            LoadFromFileDestroyedTriggers(p_LocationJson["DestroyedTriggers"]);
        }
        if (p_LocationJson.HasField("CheckCounterValues"))
        {
            LoadFromFileCheckCounterValues(p_LocationJson["CheckCounterValues"]);
        }
    }

    public JSONObject GetJson()
    {
        Save();

        JSONObject l_LocationSaveJson = new JSONObject();

        l_LocationSaveJson.AddField("Id", m_Id);
        l_LocationSaveJson.AddField("ActorsState", ActorsStateGetJson());
        l_LocationSaveJson.AddField("DieActors", DieActorsGetJson());
        l_LocationSaveJson.AddField("DoorsState", DoorsStateGetJson());
        l_LocationSaveJson.AddField("DestroyedTriggers", DestroyedTriggersGetJson());
        l_LocationSaveJson.AddField("CheckCounterValues", CheckCounterValuesGetJson());

        return l_LocationSaveJson;
    }

    private void SaveDoorState()
    {
        foreach (FrontDoor l_Door in m_Doors.Values)
        {
            if (m_DoorState.ContainsKey(l_Door.id))
            {
                m_DoorState[l_Door.id] = l_Door.closed;
            }
            else
            {
                m_DoorState.Add(l_Door.id, l_Door.closed);
            }
        }
    }

    private void SaveActorStates()
    {
        foreach (JourneyActor l_Actor in m_Actors.Values)
        {
            ActorState l_ActorState = new ActorState(l_Actor.interactBehaviorId, l_Actor.movementBehaviorId, l_Actor.myTransform.position);

            if (m_ActorsState.ContainsKey(l_Actor.actorId))
            {
                m_ActorsState[l_Actor.actorId] = l_ActorState;
            }
            else
            {
                m_ActorsState.Add(l_Actor.actorId, l_ActorState);
            }
        }
    }

    private void LoadDoorState()
    {
        foreach (string l_DoorId in m_DoorState.Keys)
        {
            m_Doors[l_DoorId].closed = m_DoorState[l_DoorId];
        }
    }

    private void LoadDiesActors()
    {
        for (int i = 0; i < m_DieActorsIds.Count; i++)
        {
            Object.Destroy(m_Actors[m_DieActorsIds[i]].gameObject);
            m_Actors.Remove(m_DieActorsIds[i]);
        }
    }

    private void LoadActorStates()
    {
        foreach (string l_ActorId in m_ActorsState.Keys)
        {
            ActorState l_ActorState = m_ActorsState[l_ActorId];
            m_Actors[l_ActorId].ChangeInteractionBehavior(l_ActorState.interactBehavior);
            m_Actors[l_ActorId].ChangeMovementBehavior(l_ActorState.movementBehavior);
            m_Actors[l_ActorId].myTransform.position = l_ActorState.position;
            m_Actors[l_ActorId].UpdateSortingLayer();
        }
    }

    private void LoadDestroyedTriggers()
    {
        for (int i = 0; i < m_DestroyedTriggers.Count; i++)
        {
            Object.Destroy(m_GameEventTriggers[m_DestroyedTriggers[i]].gameObject);
            m_GameEventTriggers.Remove(m_DestroyedTriggers[i]);
        }
    }

    private void LoadCheckCounterValues()
    {
        foreach (string l_Id in m_CheckCounterValues.Keys)
        {
            m_CheckCounterActions[l_Id].counter = m_CheckCounterValues[l_Id];
        }
    }

    private void LoadFromFileActorStates(JSONObject p_ActorStatesJson)
    {
        for (int i = 0; i < p_ActorStatesJson.Count; i++)
        {
            string l_InteractBehavior = p_ActorStatesJson[i]["InteractBehavior"].str;
            string l_MovementBehavior = p_ActorStatesJson[i]["MovementBehavior"].str;
            float l_X = p_ActorStatesJson[i]["Position"]["X"].f;
            float l_Y = p_ActorStatesJson[i]["Position"]["Y"].f;
            Vector3 l_Position = new Vector3(l_X, l_Y);
            ActorState l_ActorState = new ActorState(l_InteractBehavior, l_MovementBehavior, l_Position);
            m_ActorsState.Add(p_ActorStatesJson[i]["Id"].str, l_ActorState);
        }
    }

    private void LoadFromFileDieActors(JSONObject p_DieActorsJson)
    {
        for (int i = 0; i < p_DieActorsJson.Count; i++)
        {
            m_DieActorsIds.Add(p_DieActorsJson[i].str);
        }
    }

    private void LoadFromFileDoorStates(JSONObject p_DoorStatesJson)
    {
        for (int i = 0; i < p_DoorStatesJson.Count; i++)
        {
            m_DoorState.Add(p_DoorStatesJson[i]["Id"].str, p_DoorStatesJson[i]["Closed"].b);
        }
    }

    private void LoadFromFileDestroyedTriggers(JSONObject p_DestroyedTriggersJson)
    {
        for (int i = 0; i < p_DestroyedTriggersJson.Count; i++)
        {
            m_DestroyedTriggers.Add(p_DestroyedTriggersJson[i].str);
        }
    }

    private void LoadFromFileCheckCounterValues(JSONObject p_CheckCounterValuesJson)
    {
        for (int i = 0; i < p_CheckCounterValuesJson.Count; i++)
        {
            m_CheckCounterValues.Add(p_CheckCounterValuesJson[i]["Id"].str, (int)p_CheckCounterValuesJson[i]["Value"].i);
        }
    }

    private JSONObject ActorsStateGetJson()
    {
        JSONObject l_ActorsStates = new JSONObject();

        foreach (string l_ActorId in m_ActorsState.Keys)
        {
            JSONObject l_ActoStateJson = new JSONObject();
            l_ActoStateJson.AddField("Id", l_ActorId);
            l_ActoStateJson.AddField("InteractBehavior", m_ActorsState[l_ActorId].interactBehavior);
            l_ActoStateJson.AddField("MovementBehavior", m_ActorsState[l_ActorId].movementBehavior);

            JSONObject l_ActorPositionJson = new JSONObject();
            l_ActorPositionJson.AddField("X", m_ActorsState[l_ActorId].position.x);
            l_ActorPositionJson.AddField("Y", m_ActorsState[l_ActorId].position.y);
            l_ActoStateJson.AddField("Position", l_ActorPositionJson);

            l_ActorsStates.Add(l_ActoStateJson);
        }

        return l_ActorsStates;
    }

    private JSONObject DieActorsGetJson()
    {
        JSONObject l_DieActorsJson = new JSONObject();

        for (int i = 0; i < m_DieActorsIds.Count; i++)
        {
            l_DieActorsJson.Add(m_DieActorsIds[i]);
        }

        return l_DieActorsJson;
    }

    private JSONObject DoorsStateGetJson()
    {
        JSONObject l_DoorStateListJson = new JSONObject();

        foreach (string l_DoorId in m_DoorState.Keys)
        {
            JSONObject l_DoorStateJson = new JSONObject();

            l_DoorStateJson.AddField("Id", l_DoorId);
            l_DoorStateJson.AddField("Closed", m_DoorState[l_DoorId]);

            l_DoorStateListJson.Add(l_DoorStateJson);
        }

        return l_DoorStateListJson;
    }

    private JSONObject DestroyedTriggersGetJson()
    {
        JSONObject l_DestroyedTriggers = new JSONObject();

        for (int i = 0; i < m_DestroyedTriggers.Count; i++)
        {
            l_DestroyedTriggers.Add(m_DestroyedTriggers[i]);
        }

        return l_DestroyedTriggers;
    }

    private JSONObject CheckCounterValuesGetJson()
    {
        JSONObject l_CheckCounterListJson = new JSONObject();

        foreach (string l_CheckCounterId in m_CheckCounterValues.Keys)
        {
            JSONObject l_CheckCounterJson = new JSONObject();

            l_CheckCounterJson.AddField("Id", l_CheckCounterId);
            l_CheckCounterJson.AddField("Value", m_CheckCounterValues[l_CheckCounterId]);

            l_CheckCounterListJson.Add(l_CheckCounterJson);
        }

        return l_CheckCounterListJson;
    }
}
