using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGenerate : MonoBehaviour
{
    public void Awake()
    {
        RoomEnemyGenerator m_RoomEnemyGenerator = GetComponent<RoomEnemyGenerator>();

        SaveSystem.GetInstance().AddRoomGenerator(m_RoomEnemyGenerator);
    }
	
}
