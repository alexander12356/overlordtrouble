using System.Collections.Generic;

using UnityEngine;

public enum Element
{
    NONE = -1,
    Physical,
    Water,
    Fire,
    Earth
}

public class DamageSystem : Singleton<DamageSystem>
{
    private enum AttackType
    {
        BaseAttack,
        SpecialAttack
    }

    private BattleActor m_Target = null;
    private BattleActor m_Sender = null;
    private float m_DamageValue = 0.0f;
    private bool m_IsBadAttack = false;
    private string m_StatisticText = string.Empty;

    public void Attack(BattleActor p_Sender, BattleActor p_Target, float p_DamageValue)
    {
        m_Sender = p_Sender;
        m_Target = p_Target;
        m_DamageValue = p_DamageValue;

        string l_SenderName = m_Sender.actorName;
        string l_TargetName = m_Target.actorName;

        m_StatisticText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:PlayerAttack", new string[] { l_SenderName, l_TargetName, m_DamageValue.ToString() });

        p_Target.Damage(p_DamageValue);
    }

    public void MonstyleAttack(BattleActor p_Sender, BattleActor p_Target, float p_DamageValue, bool p_IsBadAttack, string p_AttackNames = "")
    {
        m_Sender = p_Sender;
        m_Target = p_Target;
        m_DamageValue = p_DamageValue;
        m_IsBadAttack = p_IsBadAttack;

        p_Target.Damage(p_DamageValue);
    }

    public void MonstyleAttack(BattleActor p_Sender, BattleActor p_Target, List<Special> p_MonstyleList)
    {
        m_Sender = p_Sender;
        m_Target = p_Target;
        string l_SenderName = m_Sender.actorName;
        string l_TargetName = m_Target.actorName;
        string l_MonstyleNames = string.Empty;
        string l_DamageText = string.Empty;

        for (int i = 0; i < p_MonstyleList.Count; i++)
        {
            l_MonstyleNames += LocalizationDataBase.GetInstance().GetText("Special:" + p_MonstyleList[i].id) + ", ";
            p_MonstyleList[i].Run(m_Sender, m_Target);

            string l_PrefabPath = "Prefabs/BattleEffects/Monstyle/" + p_MonstyleList[i].id + "Monstyle";

            VisualEffect l_AttackEffect = Object.Instantiate(Resources.Load<VisualEffect>(l_PrefabPath));
            l_AttackEffect.Init(p_Target, p_Target.spriteRenderer.transform);
            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_AttackEffect);

            ResultSystem.GetInstance().AddStep(l_Step);
        }

        m_StatisticText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:MonstyleUsing", new string[] { l_SenderName, l_TargetName, l_MonstyleNames });

        if (m_DamageValue >= 0.001f)
        {
            l_DamageText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:MonstyleDamage", new string[] { m_DamageValue.ToString() });
            m_StatisticText += l_DamageText;

            p_Target.Damage(m_DamageValue);
        }
    }

    public void AttackSuccess()
    {
        TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { m_StatisticText });
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

    public void AddDamageValue(float l_Damage)
    {
        m_DamageValue += l_Damage;
    }

    public void Reset()
    {
        m_StatisticText = string.Empty;
        m_DamageValue = 0.0f;
    }
}
