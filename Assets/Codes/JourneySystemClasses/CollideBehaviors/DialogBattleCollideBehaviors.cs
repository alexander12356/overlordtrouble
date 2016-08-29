using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogBattleCollideBehaviors : DialogCollideBehaviors
{
    public override void EndDialog()
    {
        BattleStarter.GetInstance().AddEnemy(((JourneyNPC)m_JourneyActor).npcId);
        SceneManager.LoadScene("BattleSystem");
    }
}
