using UnityEngine;

public class StartBattleAction : MonoBehaviour
{
    [SerializeField]
    private JourneyEnemy m_Enemy = null;

    [SerializeField]
    private string m_BattleId;

    [SerializeField]
    private string m_SceneId;

    public void Run()
    {
        if (m_BattleId == "BossLeshii")
        {
            JourneySystem.GetInstance().AddScene("BossBattleSystem");
        }
        else
        {
            BattleStarter.GetInstance().InitBattle(m_Enemy, m_BattleId);
            JourneySystem.GetInstance().AddScene(m_SceneId);
        }

        JourneySystem.GetInstance().SetControl(ControlType.StartBattle);
        AudioSystem.GetInstance().StopTheme();
    }
}
