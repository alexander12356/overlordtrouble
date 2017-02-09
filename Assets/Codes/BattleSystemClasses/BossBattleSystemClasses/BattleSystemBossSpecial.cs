using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystemBossSpecial : BattleSystemBoss
{
    public override void RunBossIntro()
    {
        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetTalkingAnimator(m_Leshii.headAnimator, "Talking");

        string l_Text = LocalizationDataBase.GetInstance().GetText("Boss:Leshii:IntroSpecial");
        l_TextPanel.SetText(new List<string>() { l_Text });

        l_TextPanel.AddButtonAction(l_TextPanel.Close);
        l_TextPanel.AddButtonAction(StartCharge);

        ShowPanel(l_TextPanel);

        SetVisibleAvatarPanel(false);
    }

    private void StartCharge()
    {
        LeshiiAttackEffect l_LeshiiStartChargeEffect = Instantiate(LeshiiAttackEffect.prefab);
        l_LeshiiStartChargeEffect.AddPlayAction(PlayStartCharge);

        m_Leshii.endEffectChecker.AddAttackEffect(l_LeshiiStartChargeEffect);

        BattlePlayEffectStep l_PlayStep = new BattlePlayEffectStep(l_LeshiiStartChargeEffect);
        ResultSystem.GetInstance().AddStep(l_PlayStep);

        ResultSystem.GetInstance().ShowResult();
    }

    private void PlayStartCharge()
    {
        m_Leshii.bodyAnimator.SetTrigger("StartCharge");
    }
}
