using System.Collections.Generic;

public struct EnemyAttackData
{
    public string id;
    public Element element;
    public EnemyAttackTarget targetId;
    public List<EffectData> effectList;

    public EnemyAttackData(string p_Id, Element p_Element, EnemyAttackTarget p_TargetId, List<EffectData> p_EffectList)
    {
        id = p_Id;
        element = p_Element;
        targetId = p_TargetId;
        effectList = p_EffectList;
    }
}

public struct EnemyLootData
{
    public string id;
    public int count;
    public float chance;

    public EnemyLootData(string p_Id, int p_Count, float p_Chance)
    {
        id = p_Id;
        count = p_Count;
        chance = p_Chance;
    }
}

public struct EnemyData
{
    public string id;
    public float attackStat;
    public float defenseStat;
    public float speedStat;
    public int level;
    public float health;
    public Element element;
    public List<EnemyAttackData> attackList;
    public int experience;
    public List<EnemyLootData> lootList;
    public string[] property;

    public EnemyData(string p_Id, float p_AttackStat, float p_DefenseStat, float p_SpeedStat, int p_Level, float p_Health, Element p_Element, List<EnemyAttackData> p_AttackList, int p_Experience, List<EnemyLootData> p_LootList, string[] p_Property)
    {
        id = p_Id;
        attackStat = p_AttackStat;
        defenseStat = p_DefenseStat;
        speedStat = p_SpeedStat;
        level = p_Level;
        health = p_Health;
        element = p_Element;
        attackList = p_AttackList;
        experience = p_Experience;
        lootList = p_LootList;
        property = p_Property;
    }
}
