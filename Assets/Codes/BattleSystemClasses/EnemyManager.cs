using System.Collections.Generic;

public class EnemyManager : Singleton<EnemyManager>
{
    #region Interface
    //TODO Kostil
    public List<Enemy> GetEnemy()
    {
        List<Enemy> l_EnemyList = new List<Enemy>();
        l_EnemyList.Add(Enemy.GetInstance());

        return l_EnemyList;
    }
    #endregion
}