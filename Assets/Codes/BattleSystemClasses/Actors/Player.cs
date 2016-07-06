using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    private static Player m_Instance = null;
    private float m_Health = 20;
    private float m_Mana   = 10;
    private int[] m_DamageValue = new int[2] { 5, 8};
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
    
    public void Attack()
    {
        int l_Damage = Random.Range(m_DamageValue[0], m_DamageValue[1]);

        EnemyManager.GetInstance().GetEnemy().Damage(l_Damage);

        TextPanel l_NewTextPanel = Instantiate(PanelManager.GetInstance().textPanelPrefab);
        l_NewTextPanel.AddPopAction(EndTurn);
        l_NewTextPanel.SetText("Игрок нанес " + l_Damage + " урона врагу");
        PanelManager.GetInstance().ShowPanel(l_NewTextPanel);
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

    private void EndTurn()
    {
        TurnSystem.GetInstance().EndTurn();
    }
    #endregion
}
