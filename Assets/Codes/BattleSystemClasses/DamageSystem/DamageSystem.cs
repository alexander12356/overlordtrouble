using System.Collections.Generic;

using UnityEngine;

public class DamageSystem : Singleton<DamageSystem>
{
    private enum AttackType
    {
        BaseAttack,
        SpecialAttack
    }

    private BattleActor m_Target = null;
    private BattleActor m_Sender = null;
    private string m_AttackNames = string.Empty;
    private float m_DamageValue = 0.0f;
    private AttackType m_AttackType = AttackType.BaseAttack;
    private bool m_IsBadAttack = false;

    public void Attack(BattleActor p_Sender, BattleActor p_Target, float p_DamageValue)
    {
        m_Sender = p_Sender;
        m_Target = p_Target;
        m_DamageValue = p_DamageValue;
        m_AttackType = AttackType.BaseAttack;

        p_Target.Damage(p_DamageValue);
    }

    public void SpecialAttack(BattleActor p_Sender, BattleActor p_Target, float p_DamageValue, bool p_IsBadAttack, string p_AttackNames = "")
    {
        m_Sender = p_Sender;
        m_Target = p_Target;
        m_DamageValue = p_DamageValue;
        m_AttackNames = p_AttackNames;
        m_IsBadAttack = p_IsBadAttack;
        m_AttackType = AttackType.SpecialAttack;

        p_Target.Damage(p_DamageValue);
    }

    public void AttackSuccess()
    {
        string l_SenderName = m_Sender.actorName;
        string l_TargetName = m_Target.actorName;

        string l_StatisticText = string.Empty;

        switch (m_AttackType)
        {
            case AttackType.BaseAttack:
                l_StatisticText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:PlayerAttack", new string[] { l_SenderName, l_TargetName, m_DamageValue.ToString() });
                break;
            case AttackType.SpecialAttack:
                if (m_IsBadAttack)
                {
                    l_StatisticText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:BadAttack", new string[] { m_DamageValue.ToString(), l_TargetName });
                }
                else
                {
                    l_StatisticText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:SpecialAttack", new string[] { l_SenderName, l_TargetName, m_AttackNames, m_DamageValue.ToString() });
                }
                break;
        }

        TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { l_StatisticText });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);

        BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_Step);

        m_Target.CheckDeath();
    }

    public void AttackFail()
    {
        string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:AttackFail");

        TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { l_Text });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);

        BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_Step);
    }

    public void Reset()
    {
        m_AttackNames = "";
    }
}
