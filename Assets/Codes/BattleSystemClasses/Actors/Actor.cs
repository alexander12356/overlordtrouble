using UnityEngine;

public class Actor : MonoBehaviour
{
    #region Variables
    private int m_Health;
    private int m_BaseHealth;
    private int m_Mana;
    private int m_BaseMana;

    private bool m_IsDead;

    private string m_ActorName = "Actor";
    #endregion

    #region Interface
    public int health
    {
        get { return m_Health;  }
        set { m_Health = value; }
    }
    public int mana
    {
        get { return m_Mana;  }
        set { m_Mana = value; }
    }
    public int baseHealth
    {
        get { return m_BaseHealth;  }
        set { m_BaseHealth = value; }
    }
    public int baseMana
    {
        get { return m_BaseMana;  }
        set { m_BaseMana = value; }
    }
    public bool isDead
    {
        get { return m_IsDead;  }
        set { m_IsDead = value; }
    }
    public string actorName
    {
        get { return m_ActorName;  }
        set { m_ActorName = value; }
    }

    public virtual void Awake()
    {
        InitStats();
    }

    public virtual void Attack(Actor p_Actor)
    {
    }

    public virtual void Damage(int p_DamageValue)
    {
    }

    public virtual void Died()
    {
        isDead = true;
    }

    public virtual void EndTurn()
    {
        TurnSystem.GetInstance().EndTurn();
    }

    public virtual void InitStats()
    {
    }
    #endregion
}
