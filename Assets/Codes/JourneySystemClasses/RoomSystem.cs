using UnityEngine;
using System.Collections.Generic;

public class RoomSystem : MonoBehaviour
{
    private static RoomSystem m_Instance;
    private Dictionary<string, int> m_BoundSortingOrder = new Dictionary<string, int>();
    private string m_CurrentRoom = "Town";

    public static RoomSystem GetInstance()
    {
        return m_Instance;
    }

    public void Awake()
    {
        m_Instance = this;

        m_BoundSortingOrder.Add("Town", 256);
    }

    public int GetSortingOrderBound(Transform p_PivotTransform)
    {
        return m_BoundSortingOrder[m_CurrentRoom] - (int)(p_PivotTransform.position.y * 10);
    }
}
