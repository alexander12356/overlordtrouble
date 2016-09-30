using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogBattleCollideBehaviors : DialogCollideBehaviors
{
    [SerializeField]
    private string m_BattleId = "TestBattle";

    public override void StopAction()
    {
        BattleStarter.GetInstance().InitBattle(m_JourneyActor, m_BattleId);
        JourneySystem.GetInstance().AddScene("BattleSystem");
    }
}
