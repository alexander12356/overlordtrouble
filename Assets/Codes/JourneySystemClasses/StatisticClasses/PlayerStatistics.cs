using UnityEngine;

public class PlayerStatistics : MonoBehaviour, IEffectInfluenced
{
    private float m_Health;
    private float m_SpecialPoints;

    public float health
    {
        get { return PlayerData.GetInstance().health; }
        set
        {
            m_Health = value;
            m_Health = m_Health > 0 ? baseHealth : m_Health;
            PlayerData.GetInstance().health = (int)m_Health;
        }
    }
    public float baseHealth
    {
        get { return PlayerData.GetInstance().GetStatValue("HealthPoints"); }
        set { PlayerData.GetInstance().GetStats()["HealthPoints"] = (int)value; }
    }
    public float specialPoints
    {
        get { return m_SpecialPoints; }
        set
        {
            m_SpecialPoints = value;
            m_SpecialPoints = m_SpecialPoints > baseSpecialPoints ? baseSpecialPoints : m_SpecialPoints;
            PlayerData.GetInstance().specialPoints = (int)m_SpecialPoints;
        }
    }
    public float baseSpecialPoints
    {
        get { return PlayerData.GetInstance().GetStatValue("MonstylePoints"); }
        set { PlayerData.GetInstance().GetStats()["MonstylePoints"] = (int)value; }
    }
    public float attackStat
    {
        get { return PlayerData.GetInstance().GetStatValue("Attack"); }
        set { PlayerData.GetInstance().GetStats()["Attack"] = (int)value; }
    }
    public float defenseStat
    {
        get { return PlayerData.GetInstance().GetStatValue("Defense"); }
        set { PlayerData.GetInstance().GetStats()["Defense"] = (int)value; }
    }
    public int level
    {
        get { return PlayerData.GetInstance().GetLevel(); }
        set { }
    }
    public float speedStat
    {
        get { return PlayerData.GetInstance().GetStatValue("Speed"); }
        set { PlayerData.GetInstance().GetStats()["Speed"] = (int)value; }
    }

    public void Awake()
    {
        m_Health = PlayerData.GetInstance().health;
        m_SpecialPoints = PlayerData.GetInstance().specialPoints;
    }
}
