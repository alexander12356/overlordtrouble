using System.Collections.Generic;

public struct EnemyData
{
    public string id;
    public float health;
    public int experience;
    public List<int> damageValue;

    public EnemyData(string p_Id, float p_Health, int p_Experience, List<int> p_DamageValue)
    {
        id = p_Id;
        health = p_Health;
        experience = p_Experience;
        damageValue = p_DamageValue;
    }
}
