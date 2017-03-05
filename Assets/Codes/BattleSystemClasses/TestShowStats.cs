using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShowStats : MonoBehaviour
{
    public void Awake()
    {
        if (!GameManager.GetInstance().isTesting)
        {
            enabled = false;
            gameObject.SetActive(false);
        }
    }

    public void OnGUI()
    {
        PrintActorStats(BattlePlayer.GetInstance(), 10);

        List<BattleEnemy> l_EnemyList = BattleSystem.GetInstance().GetEnemyList();

        for (int i = 0; i < l_EnemyList.Count; i++)
        {
            PrintActorStats(l_EnemyList[i], 170 * (i + 1));
        }
    }

    private void PrintActorStats(BattleActor p_BattleActor, int p_X)
    {
        GUI.Box(new Rect(p_X, 10, 150, 150), p_BattleActor.actorName + " stats");

        GUI.Label(new Rect(p_X + 10, 30, 150, 90), "HP: " + p_BattleActor.health);
        GUI.Label(new Rect(p_X + 10, 60, 150, 90), "SP: " + p_BattleActor.specialPoints);
        GUI.Label(new Rect(p_X + 10, 90, 150, 90), "Attack stats: " + p_BattleActor.attackStat);
        GUI.Label(new Rect(p_X + 10, 120, 150, 90), "Defense stats: " + p_BattleActor.defenseStat);
    }
}
