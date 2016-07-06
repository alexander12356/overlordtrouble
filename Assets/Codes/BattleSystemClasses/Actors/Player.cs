using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    private static Player m_Instance = null;
    private float m_Health = 5;
    private float m_Mana   = 10;
    private float m_DamageValue = 2;
    private bool  m_IsDied = false;
    #endregion

    #region Interface
    static public Player GetInstance()
    {
        return m_Instance;
    }
    
    public void Run()
    {
        Unblock();
    }

    public void EndTurn()
    {
        Block();
        TurnSystem.GetInstance().EndTurn();
    }
    
    public void Attack()
    {
        EnemyManager.GetInstance().GetEnemy().Damage(m_DamageValue);

        EndTurn();
    }

    public void Damage(float p_Value)
    {
        m_Health -= p_Value;

        Debug.Log("Player health: " + m_Health);

        if (m_Health <= 0.0f)
        {
            Died();

            Debug.Log("Player is died");
        }
    }

    public void SpecialAttack(List<Special> m_SpecialList)
    {
        EndTurn();
    }
    #endregion

    #region Private
    private void Awake()
    {
        m_Instance = this;
    }
    
    private void Block()
    {
    }
    
    private void Unblock()
    {
    }
    
    private void Died()
    {
        BattleSystem.GetInstance().Died(BattleSystem.ActorID.Player);
        m_IsDied = true;
    }
    #endregion
}
