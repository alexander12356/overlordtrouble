public class TurnSystem : Singleton<TurnSystem>
{
    // флажок хода
    private bool m_IsPlayerTurn = true;

    public TurnSystem()
    {
    }

    public void EndTurn()
    {
        // Переключение флажка
        m_IsPlayerTurn = !m_IsPlayerTurn;

        // Если ход игрока
        if (m_IsPlayerTurn)
        {
            //  Запуск игрока
            if (!Player.GetInstance().isDied)
            {
                Player.GetInstance().Run();
            }
        }
        else
        {
            //  Запуск ИИ
            if (!Enemy.GetInstance().isDied)
            {
                Enemy.GetInstance().Run();
            }            
        }
    }
}