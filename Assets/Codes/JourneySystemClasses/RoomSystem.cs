using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct Room
{
    public string id;
    public BoxCollider2D cameraBounds;
    public int sortingOrder;
}

public class RoomSystem : MonoBehaviour
{
    private static RoomSystem m_Instance;
    private Dictionary<string, Room> m_RoomDictionary = new Dictionary<string, Room>();

    [SerializeField]
    private string m_CurrentRoom = "HeroHome";

    [SerializeField]
    CameraFollow m_CamerFollow = null;

    [SerializeField]
    private List<Room> m_RoomList = null;

    public static RoomSystem GetInstance()
    {
        return m_Instance;
    }
    public string currentRoomId
    {
        get { return m_CurrentRoom; }
    }

    public void Awake()
    {
        m_Instance = this;

        for (int i = 0; i < m_RoomList.Count; i++)
        {
            m_RoomDictionary.Add(m_RoomList[i].id, m_RoomList[i]);
        }
    }

    public int GetSortingOrderBound(Transform p_PivotTransform)
    {
        return m_RoomDictionary[m_CurrentRoom].sortingOrder - (int)(p_PivotTransform.position.y * 10);
    }

    public void ChangeRoom(string p_TargetId)
    {
        m_CurrentRoom = p_TargetId;
        m_CamerFollow.SetCameraBounds(m_RoomDictionary[m_CurrentRoom].cameraBounds);

        JourneySystem.GetInstance().EnemyGenerate(p_TargetId);
    }
}
