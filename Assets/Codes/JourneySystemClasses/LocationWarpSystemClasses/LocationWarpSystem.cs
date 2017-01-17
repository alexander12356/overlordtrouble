using UnityEngine;

using System.Collections.Generic;

[System.Serializable]
public struct LocationWarpPosition
{
    public string id;
    public Transform myTransform;
}

public class LocationWarpSystem : MonoBehaviour
{
    private static LocationWarpSystem m_Instance;

    [SerializeField]
    private JourneyPlayer m_JourneyPlayer = null;

    [SerializeField]
    private List<LocationWarpPosition> m_Positions;

    public void Awake()
    {
        m_Instance = this;
    }

    public static LocationWarpSystem GetInstance()
    {
        return m_Instance;
    }

    public void SetPlayerPos()
    {
        string l_SenderLocationId = PlayerPrefs.GetString("SenderLocation");
        string l_TargetRoomId = PlayerPrefs.GetString("TargetRoomId");

        for (int i = 0; i < m_Positions.Count; i++)
        {
            if (l_SenderLocationId == m_Positions[i].id)
            {
                m_JourneyPlayer.myTransform.position = m_Positions[i].myTransform.position;
                RoomSystem.GetInstance().ChangeRoom(l_TargetRoomId);
                CameraFollow.GetInstance().InitPos();
                break;
            }
        }
    }
}
