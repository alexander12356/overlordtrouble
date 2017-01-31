using UnityEngine;

public class StartBattleBehavior : BaseCollideBehavior
{
    [SerializeField]
    private string m_CurrentBattleId;

    [SerializeField]
    private string[] m_BattleIds;

    public void Start()
    {
        m_CurrentBattleId = GetRandomBattle();
    }

    public override void RunAction(JourneyActor p_Sender)
    {
        base.RunAction(p_Sender);

        m_JourneyActor.ApplyTo(p_Sender);
        m_JourneyActor.StopLogic();

        Debug.Log("StartBattle id: " + m_CurrentBattleId);

        if (m_CurrentBattleId == "BossLeshii")
        {
            AudioSystem.GetInstance().StopTheme();
            JourneySystem.GetInstance().AddScene("BossBattleSystem");
            JourneySystem.GetInstance().SetControl(ControlType.StartBattle);
        }
        else
        {
            BattleStarter.GetInstance().InitBattle(m_JourneyActor as JourneyEnemy, m_CurrentBattleId);
            JourneySystem.GetInstance().StartBattle();
        }
    }

    private string GetRandomBattle()
    {
        int l_RandomBattle = Random.Range(0, m_BattleIds.Length);
        return m_BattleIds[l_RandomBattle];
    }
}
