using System.Collections.Generic;

public struct EnemyAttackData
{
    public string id;
    public List<int> damageValue;

    public EnemyAttackData(string p_Id, List<int> p_DamageValue)
    {
        id = p_Id;
        damageValue = p_DamageValue;
    }
}

public struct EnemyData
{
    public string id;
    public float health;
    public string elemental;
    public int experience;
    public List<EnemyAttackData> attackList;

    public EnemyData(string p_Id, float p_Health, string p_Elemental, int p_Experience, List<EnemyAttackData> p_AttackList)
    {
        id = p_Id;
        health = p_Health;
        elemental = p_Elemental;
        experience = p_Experience;
        attackList = p_AttackList;
    }
}
