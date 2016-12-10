using System;
using System.Collections.Generic;

public enum Element
{
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

    public void MonstyleAttack(BattleActor p_Sender, BattleActor p_Target, List<SpecialList> p_MonstyleList)
    {
        m_Sender = p_Sender;
        m_Target = p_Target;
        string l_SenderName = m_Sender.actorName;
        string l_TargetName = m_Target.actorName;
        string l_MonstyleNames = string.Empty;

        List<AttackEffect> l_AttackEffectList = new List<AttackEffect>();
        for (int i = 0; i < p_MonstyleList.Count; i++)
        {
            for (int j = 0; j < p_MonstyleList[i].count; j++)
            {
                if (p_MonstyleList[i][j].type == EffectType.Attack)
                {
                    float l_AttackValue = Convert.ToSingle(p_MonstyleList[i][j].parameters[0]);
                    Element l_Element = (Element)Enum.Parse(typeof(Element), p_MonstyleList[i][j].parameters[1]);

                    AttackEffect l_AttackEffect = new AttackEffect(l_AttackValue, l_Element);

                    l_AttackEffectList.Add(l_AttackEffect);
                }
                l_MonstyleNames += LocalizationDataBase.GetInstance().GetText("" + p_MonstyleList[i].monstyleData.id);
            }
        }

        m_StatisticText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:UsingMonstyle", new string[] { l_SenderName, l_TargetName, l_MonstyleNames, m_DamageValue.ToString() });
        p_Target.Damage(l_AttackEffectList);
    }

    public void AttackSuccess()
    {
        TextPanel l_TextPanel = UnityEngine.Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { m_StatisticText });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);

        BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_Step);

        m_Target.CheckDeath();
    }

    public void AttackSuccess(float p_Damage)
    {
        TextPanel l_TextPanel = UnityEngine.Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { m_StatisticText });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);

        BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_Step);

        m_Target.CheckDeath();
    }

    public void AttackFail()
    {
        string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:AttackFail");

        TextPanel l_TextPanel = UnityEngine.Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { l_Text });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);

        BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_Step);
    }

    public void Reset()
    {
    }
}
