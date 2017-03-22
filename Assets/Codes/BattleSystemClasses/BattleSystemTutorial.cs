using System.Collections.Generic;

public class BattleSystemTutorial : BattleSystemMobs
{
    public override void Lose()
    {
        m_IsLose = true;
        SetVisibleAvatarPanel(false);

        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Lose", new string[] { BattlePlayer.GetInstance().actorName, BattlePlayer.GetInstance().actorName }) });
        l_TextPanel.AddButtonAction(Retreat);

        BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_Step);
    }
}
