using System.Collections.Generic;
using UnityEngine;

public class Willwhishp : BattleEnemy
{
    private static Willwhishp m_Prefab = null;
    private float m_ElementKickbackValue = 4.0f;

    public static Willwhishp prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<Willwhishp>("Prefabs/BattleEnemies/Willwhishp");
            }
            return m_Prefab;
        }
    }

    public override void Attack(BattleActor p_Actor)
    {
        EnemyAttackData l_AttackData = m_EnemyData.attackList[Random.Range(0, m_EnemyData.attackList.Count)];

        if (l_AttackData.id == "HealingLight")
        {
            if (IsAllEnemiesFullHp())
            {
                while(l_AttackData.id == "HealingLight")
                {
                    l_AttackData = m_EnemyData.attackList[Random.Range(0, m_EnemyData.attackList.Count)];
                }
            }
        }

        UsingAttack(p_Actor, l_AttackData);
    }

    public override float ElementKickback(Element p_Element)
    {
        base.ElementKickback(p_Element);

        if (p_Element == Element.Physical)
        {
            return m_ElementKickbackValue;
        }
        return 0.0f;
    }

    public override void InitStats()
    {
        base.InitStats();

        m_ElementKickbackValue = System.Convert.ToSingle(m_EnemyData.property[0]);
    }

    private bool IsAllEnemiesFullHp()
    {
        List<BattleEnemy> l_EnemyList = BattleSystem.GetInstance().GetEnemyList();
        for (int i = 0; i < l_EnemyList.Count; i++)
        {
            if (l_EnemyList[i] != this && l_EnemyList[i].health < l_EnemyList[i].baseHealth)
            {
                return false;
            }
        }
        return true;
    }
}
