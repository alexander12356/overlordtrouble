using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogBattleCollideBehaviors : DialogCollideBehaviors
{
    [SerializeField]
    private string m_BattleId = "TestBattle";

    public override void EndDialog()
    {
        BattleStarter.GetInstance().InitBattle(m_BattleId);
        SceneManager.LoadScene("BattleSystem");
    }
}
