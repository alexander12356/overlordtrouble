using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private static Enemy m_Instance = null;

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

    static public Enemy GetInstance()
    {
        return m_Instance;
    }

    // Запуск ИИ
    public void Run()
    {
        // Запуск с кулдавном(Атака)
        StartCoroutine(Attack(1));
    }

    // Кулдавн(указатель на функцию)
    private IEnumerator Attack(int seconds = 0)
    {
        if (seconds > 0)
        {
            yield return new WaitForSeconds(seconds);
        }

        Player.GetInstance().Damage(damageValue);
        
        //  Конец хода
        EndTurn();
    }

    private void EndTurn()
    {
        TurnSystem.GetInstance().EndTurn();
    }

    public void Damage(float p_Value)
    {
        health -= p_Value;

        Debug.Log("Enemy health: " + health);

        if (health <= 0.0f)
        {
            Died();

            Debug.Log("Enemy is died");
        }
    }

    // Смерть
    private void Died()
    {
        BattleSystem.GetInstance().Died(BattleSystem.ActorID.Enemy);
        isDied = true;
    }
}