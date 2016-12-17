using UnityEngine;
using System.Collections;

public class DataLoader : Singleton<DataLoader>
{
    public DataLoader()
    {
        LoadData();
    }

    public void LoadData()
    {
        AudioDataBase.GetInstance();
        EnemyDataBase.GetInstance();
        ItemDataBase.GetInstance();
        DialogDataBase.GetInstance();
        ImproveDataBase.GetInstance();
        LocalizationDataBase.GetInstance();
        SpecialDataBase.GetInstance();
        StoreDataBase.GetInstance();
        BattleDataBase.GetInstance();
    }
}
