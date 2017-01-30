using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jackalope : BattleEnemy
{
    private static Jackalope m_Prefab = null;
    private int m_MadnessChance = 10;
    private float m_AdditionalAttackValue = 4.0f;

    public static Jackalope prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<Jackalope>("Prefabs/BattleEnemies/Jackalope");
            }
            return m_Prefab;
        }
    }

    public override void InitStats()
    {
        base.InitStats();

        m_MadnessChance = System.Convert.ToInt32(m_EnemyData.property[0]);
        m_AdditionalAttackValue = System.Convert.ToSingle(m_EnemyData.property[1]);
    }

    public override void Attack(BattleActor p_Actor)
    {
        System.Random l_Random = new System.Random();
        int l_CurrentMadnessChance = l_Random.Next(0, 100);

        if (m_MadnessChance > l_CurrentMadnessChance)
        {
            DamageSystem.GetInstance().AddDamageValue(this, p_Actor, m_AdditionalAttackValue, Element.Physical);
        }

        base.Attack(p_Actor);
    }
}
