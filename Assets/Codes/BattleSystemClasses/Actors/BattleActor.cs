using UnityEngine;

public class BattleActor : MonoBehaviour
{
    #region Variables
    private float m_Health;
    private float m_BaseHealth;
    private float m_Mana;
    private float m_BaseMana;

    private bool m_IsDead;

    private string m_ActorName = "Actor";
    #endregion

    #region Interface
    public float health
    {
        get { return m_Health;  }
        set
        {
            m_Health = value;
            ChangeHealthValue();
        }
    }
    public float mana
    {
        get { return m_Mana;  }
        set
        {
            m_Mana = value;
            ChangeManaValue();
        }
    }
    public float baseHealth
    {
        get { return m_BaseHealth;  }
        set { m_BaseHealth = value; }
    }
    public float baseMana
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
    }

    public virtual void Attack(BattleActor p_Actor)
    {
    }

    public virtual void Damage(float p_DamageValue, string p_AttackType)
    {
    }

    public virtual void Died()
    {
        isDead = true;
    }

    public virtual void RunTurn()
    {
        if (m_Health <= 0)
        {
            Died();
        }
    }

    public virtual void EndTurn()
    {
        BattleSystem.GetInstance().EndTurn();
    }

    public virtual void InitStats()
    {
    }

    public virtual void ChangeManaValue()
    {
    }

    public virtual void ChangeHealthValue()
    {
    }
    #endregion
}
