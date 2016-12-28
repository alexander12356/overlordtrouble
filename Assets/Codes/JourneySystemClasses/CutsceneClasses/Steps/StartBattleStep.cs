using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattleStep : BaseStep
{
    [SerializeField]
    private JourneyEnemy m_Enemy = null;

    [SerializeField]
    private string m_BattleId;

    [SerializeField]
    private string m_SceneId;

    public override void StartStep()
    {
        base.StartStep();

        BattleStarter.GetInstance().InitBattle(m_Enemy, m_BattleId);
        JourneySystem.GetInstance().AddScene(m_SceneId);

        EndStep();
    }
}
