public class TurnSystem : Singleton<TurnSystem>
{
    // флажок хода
    private bool m_IsPlayerTurn = true;

    public TurnSystem()
    {
    }

    public void EndTurn()
    {
        m_IsPlayerTurn = !m_IsPlayerTurn;
        
        if (m_IsPlayerTurn)
        {
        }
        else
        {
            //  Запуск ИИ
            if (!Enemy.GetInstance().isDead)
            {
                Enemy.GetInstance().Run();
            }            
        }
    }
}