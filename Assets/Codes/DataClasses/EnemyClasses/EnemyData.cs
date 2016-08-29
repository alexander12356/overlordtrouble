using System.Collections.Generic;

public struct EnemyData
{
    public string id;
    public float health;
    public List<int> damageValue;

    public EnemyData(string p_Id, float p_Health, List<int> p_DamageValue)
    {
        id = p_Id;
        health = p_Health;
        damageValue = p_DamageValue;
    }
}
