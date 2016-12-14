﻿using UnityEngine;

using System.Collections.Generic;

public class BattleActor : MonoBehaviour
{
    #region Variables
    private float m_Health;
    private float m_BaseHealth;
    private float m_Mana;
    private float m_BaseMana;
    private float m_Defense;
    private bool m_IsDead;
    private string m_ActorName = "Actor";
    private bool m_IsAoeAttack;
    private Dictionary<string, List<BaseEffect>> m_EffectList = new Dictionary<string, List<BaseEffect>>();
    private List<string> m_DeleteSpecials = new List<string>();

    protected SpriteRenderer m_SpriteRenderer = null;

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
    public float defense
    {
        get { return m_Defense;  }
        set { m_Defense = value; }
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
    public SpriteRenderer spriteRenderer
    {
        get { return m_SpriteRenderer; }
    }
    public bool isAoeAttack
    {
        get { return m_IsAoeAttack; }
        set { m_IsAoeAttack = value; }
    }

    public virtual void Awake()
    {
    }

    public virtual void Attack(BattleActor p_Actor)
    {
        p_Actor.CheckPrevAttack();
    }

    public virtual void Damage(float p_DamageValue)
    {
        if (p_DamageValue < defense)
        {
            return;
        }
        health -= (p_DamageValue - defense);
    }

    public virtual bool IsCanDamage(float p_Damage)
    {
        return true;
    }

    public virtual bool CheckDeath()
    {
        if (health <= 0)
        {
            return true;
        }
        return false;
    }

    public virtual void Die()
    {
        isDead = true;
    }

    public virtual void Died()
    {
    }

    public virtual void RunTurn()
    {
        CheckEffects();
        RunningEffect();
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

    public virtual void PlayHitSound()
    {
    }

    public virtual void CheckPrevAttack()
    {
    }

    public virtual void CheckEffects()
    {
    }

    public void AddEffect(string p_SpecialId, BaseEffect l_Effect)
    {
        if (!m_EffectList.ContainsKey(p_SpecialId))
        {
            m_EffectList.Add(p_SpecialId, new List<BaseEffect>());
            m_EffectList[p_SpecialId].Add(l_Effect);
            return;
        }

        for (int i = 0; i < m_EffectList[p_SpecialId].Count; i++)
        {
            if (m_EffectList[p_SpecialId][i].id == l_Effect.id)
            {
                m_EffectList[p_SpecialId][i].Stack(l_Effect);
            }
        }
    }

    private void RunningEffect()
    {
        foreach (string l_Id in m_EffectList.Keys)
        {
            for (int i = 0; i < m_EffectList[l_Id].Count; i++)
            {
                m_EffectList[l_Id][i].Effective();
                if (m_EffectList[l_Id][i].CheckEnd())
                {
                    m_EffectList[l_Id].RemoveAt(i);
                    i--;
                }
            }
            if (m_EffectList[l_Id].Count == 0)
            {
                m_DeleteSpecials.Add(l_Id);
            }
        }

        for (int i = 0; i < m_DeleteSpecials.Count; i++)
        {
            m_EffectList.Remove(m_DeleteSpecials[i]);
        }
        m_DeleteSpecials.Clear();
    }
    #endregion
}
