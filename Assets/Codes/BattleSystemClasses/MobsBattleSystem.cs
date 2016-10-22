using UnityEngine;

public class MobsBattleSystem : BattleSystem
{
    [SerializeField]
    private Transform m_EnemyTransform = null;

    public override void Awake()
    {
        base.Awake();

        InitBattle();
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

        for (int i = 0; i < m_BattleData.enemyList.Count; i++)
        {
            BattleEnemy l_NewEnemy = Instantiate(BattleEnemy.prefab);
            l_NewEnemy.SetData(EnemyDataBase.GetInstance().GetEnemy(m_BattleData.enemyList[i]));
            l_NewEnemy.transform.SetParent(m_EnemyTransform);

            float l_X = 2.25f * (m_BattleData.enemyList.Count - 1);
            l_X = -l_X + (4.5f * i);
            Vector3 l_LocalPosition = Vector3.zero;
            l_LocalPosition.x = l_X;
            l_NewEnemy.transform.localPosition = l_LocalPosition;

            m_EnemyList.Add(l_NewEnemy);
        }

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

    public override void EndTurn()
    {
        base.EndTurn();

        if (m_EnemyList.Count == 0)
        {
            Win();
            return;
        }

        if (m_IsLose)
        {
            return;
        }

        m_CurrentTurn++;
        if (m_CurrentTurn >= m_EnemyList.Count)
        {
            m_CurrentTurn = -1;
        }

        if (m_CurrentTurn == -1)
        {
            SetVisibleAvatarPanel(true);
            BattlePlayer.GetInstance().RunTurn();
        }
        else
        {
            SetVisibleAvatarPanel(false);
            m_EnemyList[m_CurrentTurn].RunTurn();
        }
    }
}
