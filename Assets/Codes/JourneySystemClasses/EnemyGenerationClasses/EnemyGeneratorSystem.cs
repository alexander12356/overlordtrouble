using System.Collections.Generic;

using UnityEngine;

public class EnemyGeneratorSystem : MonoBehaviour
{
    private Dictionary<string, RoomEnemyGenerator> m_RoomGenerates = new Dictionary<string, RoomEnemyGenerator>();
	
    public void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            m_RoomGenerates.Add(transform.GetChild(i).name, transform.GetChild(i).GetComponent<RoomEnemyGenerator>());
        }
    }

    public void Generate(string p_RoomId)
    {
        if (!m_RoomGenerates.ContainsKey(p_RoomId))
        {
            return;
        }

        m_RoomGenerates[p_RoomId].Generate();
    }
}
