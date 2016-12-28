using System.Collections.Generic;

public class BattleSystemTutorial : BattleSystemMobs
{
    public override void Lose()
    {
        SetVisibleAvatarPanel(false);

        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Lose") });
        l_TextPanel.AddButtonAction(Retreat);

        ShowPanel(l_TextPanel);
    }
}
