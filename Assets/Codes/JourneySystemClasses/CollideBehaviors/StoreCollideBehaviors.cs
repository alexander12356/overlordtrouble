using UnityEngine;
using System.Collections;

public class StoreCollideBehaviors : BaseCollideBehaviors
{
    public override void EnterAction(JourneyActor p_JourneyActor)
    {
        base.EnterAction(p_JourneyActor);

        JourneyPlayer l_JourneyPlayer = (JourneyPlayer)p_JourneyActor;
        l_JourneyPlayer.AddActiveButtonAction(OpenStore);
        l_JourneyPlayer.AddDisactiveButtonAction(CloseStore);
    }

    public override void ExitAction(JourneyActor p_JourneyActor)
    {
        base.ExitAction(p_JourneyActor);

        JourneyPlayer l_JourneyPlayer = (JourneyPlayer)p_JourneyActor;
        l_JourneyPlayer.RemoveActiveButtonAction(OpenStore);
        l_JourneyPlayer.RemoveDisactiveButtonAction(CloseStore);
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
