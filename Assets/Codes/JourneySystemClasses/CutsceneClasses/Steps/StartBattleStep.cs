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
        JourneySystem.GetInstance().SetControl(ControlType.StartBattle);
        AudioSystem.GetInstance().StopTheme();

        EndStep();
    }
}
