using System;

using UnityEngine;

public class FeralSlime : BattleEnemy
{
    private static FeralSlime m_Prefab = null;
    public static FeralSlime prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<FeralSlime>("Prefabs/BattleEnemies/FeralSlime");
            }
            return m_Prefab;
        }
    }

    public override void InitStats()
    {
        base.InitStats();

        float l_Modif = Convert.ToSingle(m_EnemyData.property[0]);

        SetModif(Element.Water, l_Modif);
    }
}
