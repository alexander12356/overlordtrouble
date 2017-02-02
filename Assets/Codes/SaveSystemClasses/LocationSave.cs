using System.Collections.Generic;

using UnityEngine;

public class LocationSave
{
    //TODO отрефакторить нахрен!

    private string m_Id = string.Empty;
    private Dictionary<string, JourneyActor> m_Actors = new Dictionary<string, JourneyActor>();
    private Dictionary<string, FrontDoor> m_Doors = new Dictionary<string, FrontDoor>();
    private Dictionary<string, GameEventTrigger> m_GameEventTriggers = new Dictionary<string, GameEventTrigger>();
    private Dictionary<string, CheckCounterAction> m_CheckCounterActions = new Dictionary<string, CheckCounterAction>();
    private Dictionary<string, Warp> m_Warps = new Dictionary<string, Warp>();
    private Dictionary<string, RoomEnemyGenerator> m_RoomGenerators = new Dictionary<string, RoomEnemyGenerator>();
    private Dictionary<string, AnimationObject> m_AnimationObjects = new Dictionary<string, AnimationObject>();
    private Dictionary<string, ActorState> m_ActorsState = new Dictionary<string, ActorState>();
    private Dictionary<string, bool> m_DoorState = new Dictionary<string, bool>();
    private List<string> m_DieActorsIds = new List<string>();
    private List<string> m_DestroyedTriggers = new List<string>();
    private Dictionary<string, int> m_CheckCounterValues = new Dictionary<string, int>();
    private Dictionary<string, string> m_WarpState = new Dictionary<string, string>();
    private Dictionary<string, bool> m_RoomGeneratorStates = new Dictionary<string, bool>();
    private Dictionary<string, string> m_AnimationObjectStates = new Dictionary<string, string>();

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
        m_Warps.Clear();
        m_RoomGenerators.Clear();
        m_AnimationObjects.Clear();
    }

    public void Save()
    {
        SaveActorStates();
        SaveDoorState();
        SaveWarpStates();
        SaveRoomGeneratorStates();
        SaveAnimationObjectStates();
    }

    public void Load()
    {
        LoadActorStates();
        LoadDiesActors();
        LoadDoorState();
        LoadDestroyedTriggers();
        LoadCheckCounterValues();
        LoadWarpStates();
        LoadRoomGeneratorStates();
        LoadAnimationObjectStates();
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

    public void AddWarp(Warp p_Warp)
    {
        m_Warps.Add(p_Warp.id, p_Warp);
    }

    public void AddRoomGenerator(RoomEnemyGenerator p_RoomGenerator)
    {
        m_RoomGenerators.Add(p_RoomGenerator.id, p_RoomGenerator);
    }

    public void AddAnimationObject(AnimationObject p_AnimationObject)
    {
        m_AnimationObjects.Add(p_AnimationObject.id, p_AnimationObject);
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
        if (p_LocationJson.HasField("WarpsState"))
        {
            LoadFromFileWarpsState(p_LocationJson["WarpsState"]);
        }
        if (p_LocationJson.HasField("RoomGenerators"))
        {
            LoadFromFileRoomGenerators(p_LocationJson["RoomGenerators"]);
        }
        if (p_LocationJson.HasField("AnimationObjects"))
        {
            LoadFromFileAnimationObjects(p_LocationJson["AnimationObjects"]);
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
        l_LocationSaveJson.AddField("WarpsState", WarpsStateGetJson());
        l_LocationSaveJson.AddField("RoomGenerators", RoomGeneratorsGetJson());
        l_LocationSaveJson.AddField("AnimationObjects", AnimationObjectsGetJson());

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
            ActorState l_ActorState = new ActorState(l_Actor.actorId, l_Actor.interactBehaviorId, l_Actor.movementBehaviorId, l_Actor.myTransform.position);

            if (m_ActorsState.ContainsKey(l_Actor.actorId))
            {
                m_ActorsState[l_Actor.actorId] = l_ActorState;
            }
            else
            {
                m_ActorsState.Add(l_Actor.actorId, l_ActorState);
            }

            if (l_Actor is JourneyEnemy)
            {
                JourneyEnemy l_JourneyEnemy = l_Actor as JourneyEnemy;
                EnemyState l_EnemyState = new EnemyState(l_Actor.actorId, l_Actor.interactBehaviorId, l_Actor.movementBehaviorId, l_Actor.myTransform.position, l_JourneyEnemy.winBehaviorId, l_JourneyEnemy.loseBehaviorId);
                m_ActorsState[l_JourneyEnemy.actorId] = l_EnemyState;
            }
        }
    }

    private void SaveWarpStates()
    {
        foreach (Warp l_Warp in m_Warps.Values)
        {
            if (m_WarpState.ContainsKey(l_Warp.id))
            {
                m_WarpState[l_Warp.id] = l_Warp.currentBehavior;
            }
            else
            {
                m_WarpState.Add(l_Warp.id, l_Warp.currentBehavior);
            }
        }
    }

    private void SaveRoomGeneratorStates()
    {
        foreach (RoomEnemyGenerator l_RoomGenerator in m_RoomGenerators.Values)
        {
            if (m_RoomGeneratorStates.ContainsKey(l_RoomGenerator.id))
            {
                m_RoomGeneratorStates[l_RoomGenerator.id] = l_RoomGenerator.generateEnable;
            }
            else
            {
                m_RoomGeneratorStates.Add(l_RoomGenerator.id, l_RoomGenerator.generateEnable);
            }
        }
    }

    private void SaveAnimationObjectStates()
    {
        foreach (AnimationObject l_AnimationObject in m_AnimationObjects.Values)
        {
            if (m_AnimationObjectStates.ContainsKey(l_AnimationObject.id))
            {
                m_AnimationObjectStates[l_AnimationObject.id] = l_AnimationObject.currentState;
            }
            else
            {
                m_AnimationObjectStates.Add(l_AnimationObject.id, l_AnimationObject.currentState);
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

    private void LoadWarpStates()
    {
        foreach (string l_WarpId in m_WarpState.Keys)
        {
            m_Warps[l_WarpId].ChangeBehavior(m_WarpState[l_WarpId]);
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

            if (l_ActorState is EnemyState)
            {
                EnemyState l_EnemyState = m_ActorsState[l_ActorId] as EnemyState;

                JourneyEnemy l_JourneyEnemy = m_Actors[l_ActorId] as JourneyEnemy;
                l_JourneyEnemy.ChangeWinBehavior(l_EnemyState.winBehavior);
                l_JourneyEnemy.ChangeLoseBehavior(l_EnemyState.loseBehavior);
            }

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

    private void LoadRoomGeneratorStates()
    {
        foreach (string l_Id in m_RoomGeneratorStates.Keys)
        {
            m_RoomGenerators[l_Id].generateEnable = m_RoomGeneratorStates[l_Id];
        }
    }

    private void LoadAnimationObjectStates()
    {
        foreach (string l_Id in m_AnimationObjectStates.Keys)
        {
            m_AnimationObjects[l_Id].SetState(m_AnimationObjectStates[l_Id]);
        }
    }

    private void LoadFromFileActorStates(JSONObject p_ActorStatesJson)
    {
        for (int i = 0; i < p_ActorStatesJson.Count; i++)
        {
            string l_Id = p_ActorStatesJson[i]["Id"].str;
            string l_InteractBehavior = p_ActorStatesJson[i]["InteractBehavior"].str;
            string l_MovementBehavior = p_ActorStatesJson[i]["MovementBehavior"].str;
            float l_X = p_ActorStatesJson[i]["Position"]["X"].f;
            float l_Y = p_ActorStatesJson[i]["Position"]["Y"].f;
            Vector3 l_Position = new Vector3(l_X, l_Y);

            ActorState l_ActorState;
            if (p_ActorStatesJson[i].HasField("WinBehavior"))
            {
                string l_WinBehavior = p_ActorStatesJson[i]["WinBehavior"].str;
                string l_LoseBehavior = p_ActorStatesJson[i]["LoseBehavior"].str;
                l_ActorState = new EnemyState(l_Id, l_InteractBehavior, l_MovementBehavior, l_Position, l_WinBehavior, l_LoseBehavior);
            }
            else
            {
                l_ActorState = new ActorState(l_Id, l_InteractBehavior, l_MovementBehavior, l_Position);
            }
            m_ActorsState.Add(l_Id, l_ActorState);
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

    private void LoadFromFileWarpsState(JSONObject p_WarpsStateJson)
    {
        for (int i = 0; i < p_WarpsStateJson.Count; i++)
        {
            m_WarpState.Add(p_WarpsStateJson[i]["Id"].str, p_WarpsStateJson[i]["BehaviorId"].str);
        }
    }

    private void LoadFromFileRoomGenerators(JSONObject p_RoomGeneratorJson)
    {
        for (int i = 0; i < p_RoomGeneratorJson.Count; i++)
        {
            m_RoomGeneratorStates.Add(p_RoomGeneratorJson[i]["Id"].str, p_RoomGeneratorJson[i]["CanGenerate"].b);
        }
    }

    private void LoadFromFileAnimationObjects(JSONObject p_AnimationObjectsJson)
    {
        for (int i = 0; i < p_AnimationObjectsJson.Count; i++)
        {
            m_AnimationObjectStates.Add(p_AnimationObjectsJson[i]["Id"].str, p_AnimationObjectsJson[i]["State"].str);
        }
    }

    private JSONObject ActorsStateGetJson()
    {
        JSONObject l_ActorsStates = new JSONObject();

        foreach (string l_ActorId in m_ActorsState.Keys)
        {
            JSONObject l_ActoStateJson = m_ActorsState[l_ActorId].GetJson();
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

    private JSONObject WarpsStateGetJson()
    {
        JSONObject l_WarpStates = new JSONObject();

        foreach (string l_WarpId in m_WarpState.Keys)
        {
            JSONObject l_WarpState = new JSONObject();
            l_WarpState.AddField("Id", l_WarpId);
            l_WarpState.AddField("BehaviorId", m_WarpState[l_WarpId]);

            l_WarpStates.Add(l_WarpState);
        }

        return l_WarpStates;
    }

    private JSONObject RoomGeneratorsGetJson()
    {
        JSONObject l_RoomGeneratorListJson = new JSONObject();

        foreach (string l_RoomId in m_RoomGeneratorStates.Keys)
        {
            JSONObject l_RoomGenerator = new JSONObject();
            l_RoomGenerator.AddField("Id", l_RoomId);
            l_RoomGenerator.AddField("CanGenerate", m_RoomGeneratorStates[l_RoomId]);

            l_RoomGeneratorListJson.Add(l_RoomGenerator);
        }

        return l_RoomGeneratorListJson;
    }

    private JSONObject AnimationObjectsGetJson()
    {
        JSONObject l_AnimationObjectsListJson = new JSONObject();

        foreach (string l_RoomId in m_AnimationObjectStates.Keys)
        {
            JSONObject l_AnimationGenerator = new JSONObject();
            l_AnimationGenerator.AddField("Id", l_RoomId);
            l_AnimationGenerator.AddField("State", m_AnimationObjectStates[l_RoomId]);

            l_AnimationObjectsListJson.Add(l_AnimationGenerator);
        }

        return l_AnimationObjectsListJson;
    }
}
