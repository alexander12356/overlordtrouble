using UnityEngine;
using System.Collections.Generic;

public struct BattleData
{
    public string id;
    public List<string> enemyList;
    public Dictionary<string, int> playerSettings;

    public BattleData(string p_Id, List<string> p_EnemyList, Dictionary<string, int> p_PlayerSettings)
    {
        id = p_Id;
        enemyList = p_EnemyList;
        playerSettings = p_PlayerSettings;
    }
}
