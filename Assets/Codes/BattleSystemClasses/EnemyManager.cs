
public class EnemyManager : Singleton<EnemyManager>
{
    #region Interface
    public Enemy GetEnemy()
    {
        return Enemy.GetInstance();
    }
    #endregion
}