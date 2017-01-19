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
    private Dictionary<string, JourneyActor> m_Actors = new Dictionary<string, JourneyActor>();
    private Dictionary<string, FrontDoor> m_Doors = new Dictionary<string, FrontDoor>();
    private Dictionary<string, GameEventTrigger> m_GameEventTriggers = new Dictionary<string, GameEventTrigger>();
    private Dictionary<string, CheckCounterAction> m_CheckCounterActions = new Dictionary<string, CheckCounterAction>();
    private Dictionary<string, ActorState> m_ActorsState = new Dictionary<string, ActorState>();
    private Dictionary<string, bool> m_DoorState = new Dictionary<string, bool>();
    private List<string> m_DieActorsIds = new List<string>();
    private List<string> m_DestroyedTriggers = new List<string>();
    private Dictionary<string, int> m_CheckCounterValues = new Dictionary<string, int>();

    public LocationSave()
    {
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
}
