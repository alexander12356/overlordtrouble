using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player m_Instance = null;

    // Жизнь
    public float health = 5;

    // Мана
    public float mana = 10;

    // Дамаг
    public float damageValue = 2;

    public bool isDied = false;

    private void Awake()
    {
        m_Instance = this;
    }

    static public Player GetInstance()
    {
        return m_Instance;
    }

    // Запуск игрока
    public void Run()
    {
        // Разблокировка игрока
        Unblock();
    }

    public void EndTurn()
    {
        Block();
        TurnSystem.GetInstance().EndTurn();
    }

    // Блокировка Игрока
    private void Block()
    {
        //  Блокировка панели
        MainPanel.GetInstance().Block();
    }

    // Разблокировка игрока
    private void Unblock()
    {
        //  Разблокировка панели
        MainPanel.GetInstance().Unblock();
    }

    // Атака
    public void Attack()
    {
        // Взять врага из BattleSystem
        // Нанести повреждения
        Enemy.GetInstance().Damage(damageValue);

        EndTurn();
    }

    // Нанесение повреждения(кол-во дамага)
    public void Damage(float p_Value)
    {
        health -= p_Value;

        Debug.Log("Player health: " + health);

        if (health <= 0.0f)
        {
            Died();

            Debug.Log("Player is died");
        }
    }

    // Смерть
    private void Died()
    {
        BattleSystem.GetInstance().Died(BattleSystem.ActorID.Player);
        isDied = true;
    }

    public void SpecialAttack(List<Special> m_SpecialList)
    {
        EndTurn();
    }
}
