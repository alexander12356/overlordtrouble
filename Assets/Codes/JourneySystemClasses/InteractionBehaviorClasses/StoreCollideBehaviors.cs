using UnityEngine;
using System.Collections;

public class StoreCollideBehaviors : BaseCollideBehavior
{
    public override void RunAction(JourneyActor p_Sender)
    {
        base.RunAction(p_Sender);

        OpenStore();
    }

    public override void StopAction()
    {
        base.StopAction();

        CloseStore();
    }

    private void OpenStore()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Panel);

        StorePanel l_StorePanel = Instantiate(StorePanel.prefab);
        l_StorePanel.AddPopAction(CloseStore);
        l_StorePanel.AddTalkingAnimator(m_JourneyActor.myAnimator);
        JourneySystem.GetInstance().ShowPanel(l_StorePanel);
    }

    private void CloseStore()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Player);
    }
}
