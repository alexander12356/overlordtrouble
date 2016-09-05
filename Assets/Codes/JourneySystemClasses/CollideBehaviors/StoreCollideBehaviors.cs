using UnityEngine;
using System.Collections;

public class StoreCollideBehaviors : BaseCollideBehaviors
{
    public override void EnterAction(JourneyPlayer p_JourneyPlayer)
    {
        base.EnterAction(p_JourneyPlayer);
        
        p_JourneyPlayer.AddActiveButtonAction(OpenStore);
        p_JourneyPlayer.AddDisactiveButtonAction(CloseStore);
    }

    public override void ExitAction(JourneyPlayer p_JourneyPlayer)
    {
        base.ExitAction(p_JourneyPlayer);

        p_JourneyPlayer.RemoveActiveButtonAction(OpenStore);
        p_JourneyPlayer.RemoveDisactiveButtonAction(CloseStore);
    }

    private void OpenStore()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Panel);

        StorePanel l_StorePanel = Instantiate(StorePanel.prefab);
        l_StorePanel.AddPopAction(CloseStore);
        l_StorePanel.AddTalkingAnimator(m_JourneyActor.myAnimator);
        PanelManager.GetInstance().ShowPanel(l_StorePanel);
    }

    private void CloseStore()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Player);
    }
}
