using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogBattleCollideBehaviors : DialogCollideBehaviors
{
    [SerializeField]
    private string m_BattleId = "TestBattle";

    public override void EndDialog()
    {
        BattleStarter.GetInstance().InitBattle(m_BattleId);
        PanelManager.GetInstance().ChangeScene("BattleSystem");//, LoadSceneMode.Additive);
        JourneySystem.GetInstance().SetControl(ControlType.None);
    }
}
