using UnityEngine;

public class BattleSystemMobs : BattleSystem
{
    [SerializeField]
    private Transform m_EnemyTransform = null;

    public override void Awake()
    {
        base.Awake();

        InitBattle();
    }

    public override void Start()
    {
        base.Start();

        TurnSystem.GetInstance().RunGame();
    }

    public override void InitBattle()
    {
        base.InitBattle();

        m_BattleData = BattleStarter.GetInstance().GetBattle();
        if (m_BattleData.id == null)
        {
            BattleStarter.GetInstance().InitBattle(null, "TestBattleSkwatwolf");
            m_BattleData = BattleStarter.GetInstance().GetBattle();
        }

        InitLocationBackground(m_BattleData.locationBackground);

        InitEnemies();

        InitPlayerStats();
    }

    private void InitEnemies()
    {
        for (int i = 0; i < m_BattleData.enemyList.Count; i++)
        {
            BattleEnemy l_NewEnemy = GetEnemyPrefab(m_BattleData.enemyList[i]);
            l_NewEnemy.SetData(EnemyDataBase.GetInstance().GetEnemy(m_BattleData.enemyList[i]));
            l_NewEnemy.transform.SetParent(m_EnemyTransform);

            float l_X = 2.25f * (m_BattleData.enemyList.Count - 1);
            l_X = -l_X + (4.5f * i);
            Vector3 l_LocalPosition = Vector3.zero;
            l_LocalPosition.x = l_X;
            l_NewEnemy.transform.localPosition = l_LocalPosition;

            m_EnemyList.Add(l_NewEnemy);
            TurnSystem.GetInstance().AddEnemy(l_NewEnemy);
        }
    }

    private BattleEnemy GetEnemyPrefab(string p_EnemyId)
    {
        BattleEnemy l_BattleEnemy = null;

        switch (p_EnemyId)
        {
            case "Bokalisk":
                l_BattleEnemy = Instantiate(Bokalisk.prefab);
                break;
            case "Skwatwolf":
                l_BattleEnemy = Instantiate(Skwatwolf.prefab);
                break;
            case "Dualent":
                l_BattleEnemy = Instantiate(Dualent.prefab);
                break;
            default:
                l_BattleEnemy = Instantiate(BattleEnemy.prefab);
                break;
        }

        return l_BattleEnemy;
    }

    private void InitPlayerStats()
    {
        if (m_BattleData.playerSettings != null)
        {
            foreach (string l_Key in m_BattleData.playerSettings.Keys)
            {
                switch (l_Key)
                {
                    case "HP":
                        PlayerData.GetInstance().health = m_BattleData.playerSettings[l_Key];
                        break;
                    case "MP":
                        PlayerData.GetInstance().monstylePoints = m_BattleData.playerSettings[l_Key];
                        break;
                }
            }
        }
    }
}
