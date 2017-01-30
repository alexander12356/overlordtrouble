using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre : BattleEnemy
{
    private static Ogre m_Prefab = null;
    private float l_DefenseModif = 0.0f;

    public static Ogre prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<Ogre>("Prefabs/BattleEnemies/Ogre");
            }
            return m_Prefab;
        }
    }

    public override void InitStats()
    {
        base.InitStats();

        l_DefenseModif = System.Convert.ToSingle(m_EnemyData.property[0]);

        SetModif(Element.Fire, l_DefenseModif);
    }
}
