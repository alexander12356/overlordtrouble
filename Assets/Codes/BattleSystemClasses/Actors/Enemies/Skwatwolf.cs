using System;
using UnityEngine;

public class Skwatwolf : BattleEnemy
{
    private static Skwatwolf m_Prefab = null;
    private float l_MissTurnChance = 0.0f;

    public static Skwatwolf prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<Skwatwolf>("Prefabs/BattleEnemies/Skwatwolf");
            }
            return m_Prefab;
        }
    }

    public override void InitStats()
    {
        base.InitStats();

        float l_Modif = Convert.ToSingle(m_EnemyData.property[0]);

        SetModif(Element.Physical, l_Modif);
    }
}
