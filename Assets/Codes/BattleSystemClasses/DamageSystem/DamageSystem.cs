using System.Collections.Generic;

using UnityEngine;

public enum BonusType
{
    Health,
    SpecialPoints
}

public enum RestorationType
{
    NONE = -1,
    Healing,
    DebuffClear,
    MegaHealing
}

public struct RestorationData
{
    public RestorationType type;
    public float value;

    public RestorationData(RestorationType p_Type, float p_Value)
    {
        type = p_Type;
        value = p_Value;
    }

    public void Upgrade(RestorationType p_Type, float p_Value)
    {
        if (type == RestorationType.Healing && p_Type == RestorationType.DebuffClear)
        {
            type = RestorationType.MegaHealing;
        }
        else if (type == RestorationType.DebuffClear && p_Type == RestorationType.Healing)
        {
            type = RestorationType.MegaHealing;
        }
        else if (type != RestorationType.MegaHealing)
        {
            type = p_Type;
        }

        value += p_Value;
    }
}

public class DamageSystem : Singleton<DamageSystem>
{    
    private string m_StatisticText = string.Empty;
    private Dictionary<BattleActor, float> m_DamageValues = new Dictionary<BattleActor, float>();
    private Dictionary<BattleActor, List<Special>> m_AoeSpecials = new Dictionary<BattleActor, List<Special>>();
    private Dictionary<BattleActor, List<Special>> m_ImmunitySpecials = new Dictionary<BattleActor, List<Special>>();
    private Dictionary<BattleActor, List<Special>> m_AddedSpecials = new Dictionary<BattleActor, List<Special>>();
    private Dictionary<BattleActor, RestorationData> m_RestorationValues = new Dictionary<BattleActor, RestorationData>();
    private Dictionary<BonusType, float> m_Bonuses = new Dictionary<BonusType, float>();
    private List<BattleBaseStep> m_VisualEffectSteps = new List<BattleBaseStep>();
    private List<BattleBaseStep> m_BeforeAttackSteps = new List<BattleBaseStep>();
    private List<BattleBaseStep> m_AfterAttackSteps  = new List<BattleBaseStep>();

    public void Attack(BattleActor p_Sender, BattleActor p_Target, Element l_AttackElement, float p_DamageValue, string p_Text = "")
    {
        string l_SenderName = p_Sender.actorName;
        string l_TargetName = p_Target.actorName;

        p_Target.CheckPrevAttack();
        if (p_Target.IsCanDamage(p_DamageValue))
        {
            AddDamageValue(p_Sender, p_Target, p_DamageValue, l_AttackElement);
            p_Target.Damage(m_DamageValues[p_Target]);

            string l_DamageText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Damage", new string[] { m_DamageValues[p_Target].ToString() });

            if (p_Text != "")
            {
                m_StatisticText = p_Text;
            }
            else
            {
                m_StatisticText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:PlayerAttack", new string[] { l_SenderName, l_TargetName });
            }

            m_StatisticText += " " + l_DamageText;
        }

        ShowResult();
    }

    public void MonstyleAttack(BattleActor p_Sender, BattleActor p_Target, List<Special> p_SpecialList)
    {        
        MonstyleSystem.GetInstance().UsingMonstyle(p_Sender, p_Target, p_SpecialList);
        
        string l_UsesSpecialNames = MonstyleSystem.GetInstance().usesSpecialNames;
        string l_SenderName = p_Sender.actorName;
        string l_TargetName = p_Target.actorName;

        m_StatisticText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:MonstyleUsing", new string[] { l_SenderName, l_UsesSpecialNames });
        
        if (m_DamageValues.ContainsKey(p_Target))
        {
            p_Target.CheckPrevAttack();

            if (p_Target.IsCanDamage(m_DamageValues[p_Target]))
            {
                m_StatisticText += LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:MonstyleDamage", new string[] { m_DamageValues[p_Target].ToString(), l_TargetName });

                p_Target.Damage(m_DamageValues[p_Target]);
            }
            else
            {
                m_StatisticText += LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:MonstyleDamage", new string[] { "0", l_TargetName });
            }
        }

        ShowResult();
    }

    public void EnemyAttack(BattleActor p_Sender, BattleActor p_Target, Special p_Special)
    {
        string l_SenderName = p_Sender.actorName;

        MonstyleSystem.GetInstance().RunSpecial(p_Sender, p_Target, p_Special);

        m_StatisticText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:EnemyUsing", new string[] { l_SenderName, p_Special.specialName });

        if (m_DamageValues.ContainsKey(p_Target))
        {
            p_Target.CheckPrevAttack();

            if (p_Target.IsCanDamage(m_DamageValues[p_Target]))
            {
                m_StatisticText += " " + LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Damage", new string[] { m_DamageValues[p_Target].ToString() });

                p_Target.Damage(m_DamageValues[p_Target]);
            }
        }

        ShowResult();
    }

    public void AddBeforeAttackSteps(BattleBaseStep l_Step)
    {
        m_BeforeAttackSteps.Add(l_Step);
    }

    public void AddVisualEffectStep(BattleBaseStep l_Step)
    {
        m_VisualEffectSteps.Add(l_Step);
    }
    
    public void AddDamageValue(BattleActor p_Sender, BattleActor p_Target, float p_Value, Element p_AttackElement)
    {
        float l_DamageValue = CalculateDamage(p_Sender, p_Target, p_Value, p_AttackElement);

        if (!m_DamageValues.ContainsKey(p_Target))
        {
            m_DamageValues.Add(p_Target, l_DamageValue);
        }
        else
        {
            m_DamageValues[p_Target] += l_DamageValue;
        }
    }
    
    public void AddAoeSpecial(BattleActor p_Target, Special p_Special)
    {
        if (!m_AoeSpecials.ContainsKey(p_Target))
        {
            m_AoeSpecials.Add(p_Target, new List<Special>());
        }
        m_AoeSpecials[p_Target].Add(p_Special);
        p_Target.isAoeAttack = true;
    }
    
    public void AddEffectSpecial(BattleActor p_Target, Special p_Special)
    {
        if (!m_AddedSpecials.ContainsKey(p_Target))
        {
            m_AddedSpecials.Add(p_Target, new List<Special>());
        }
        m_AddedSpecials[p_Target].Add(p_Special);
    }
    
    public void AddRestoration(BattleActor p_Target, RestorationType l_RestorationType, float l_Value)
    {
        RestorationData l_RestorationData;

        if (!m_RestorationValues.ContainsKey(p_Target))
        {
            l_RestorationData = new RestorationData(l_RestorationType, l_Value);

            m_RestorationValues.Add(p_Target, l_RestorationData);
            return;
        }
        m_RestorationValues[p_Target].Upgrade(l_RestorationType, l_Value);
    }

    public void AddBonuses(BonusType p_Type, float p_Value)
    {
        if (!m_Bonuses.ContainsKey(p_Type))
        {
            m_Bonuses.Add(p_Type, 0.0f);
        }
        m_Bonuses[p_Type] += p_Value;
    }

    public void AddAfterAttackStep(BattleBaseStep l_Step)
    {
        m_AfterAttackSteps.Add(l_Step);
    }

    public void AddImmunity(BattleActor p_Target, Special p_Special)
    {
        if (!m_ImmunitySpecials.ContainsKey(p_Target))
        {
            m_ImmunitySpecials.Add(p_Target, new List<Special>());
        }
        m_ImmunitySpecials[p_Target].Add(p_Special);
    }

    private void ShowResult()
    {
        RunBeforeAttackSteps();
        
        PlayVisualEffects();

        if (m_StatisticText != string.Empty)
        {
            TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
            l_TextPanel.SetText(new List<string>() { m_StatisticText });
            l_TextPanel.AddButtonAction(l_TextPanel.Close);

            BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
            ResultSystem.GetInstance().AddStep(l_Step);
        }
        
        if (m_AoeSpecials.Count > 0)
        {
            foreach(BattleActor l_Actor in m_AoeSpecials.Keys)
            {
                ShowAoeText(l_Actor, m_AoeSpecials[l_Actor]);
            }
        }

        if (m_ImmunitySpecials.Count > 0)
        {
            foreach(BattleActor l_Actor in m_ImmunitySpecials.Keys)
            {
                ShowImmunityText(l_Actor, m_ImmunitySpecials[l_Actor]);
            }
        }
        
        if (m_AddedSpecials.Count > 0)
        {
            foreach(BattleActor l_Actor in m_AddedSpecials.Keys)
            {
                ShowAddedSpecials(l_Actor, m_AddedSpecials[l_Actor]);
            }
        }

        if (m_RestorationValues.Count > 0)
        {
            foreach (BattleActor l_Actor in m_RestorationValues.Keys)
            {
                ShowRestoration(l_Actor, m_RestorationValues[l_Actor]);
            }
        }
        //      Вывести текст(айди текста, словарь)
        //  Если словарь Бонусов не пуст
        //      Вывести текст(айди текста, словарь)

        BattleCheckDeathStep l_CheckDeathStep = new BattleCheckDeathStep();
        ResultSystem.GetInstance().AddStep(l_CheckDeathStep);

        RunAfterAttackSteps();

        Reset();
    }
    
    private void RunBeforeAttackSteps()
    {
        for (int i = 0; i < m_BeforeAttackSteps.Count; i++)
        {
            ResultSystem.GetInstance().AddStep(m_BeforeAttackSteps[i]);
        }
    }

    private void PlayVisualEffects()
    {
        for (int i = 0; i < m_VisualEffectSteps.Count; i++)
        {
            ResultSystem.GetInstance().AddStep(m_VisualEffectSteps[i]);
        }
    }

    private void ShowAoeText(BattleActor p_Target, List<Special> p_SpecialList)
    {
        string l_SpecialList = GetSpecialNames(p_SpecialList);
        string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:AoeUsing", new string[] { p_Target.actorName, l_SpecialList });

        if (m_DamageValues.ContainsKey(p_Target))
        {
            if (p_Target.IsCanDamage(m_DamageValues[p_Target]))
            {
                p_Target.Damage(m_DamageValues[p_Target]);
                l_Text += LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:AoeDamage", new string[] { m_DamageValues[p_Target].ToString() });
            }
        }

        TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { l_Text });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);

        BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_Step);
    }

    private void ShowImmunityText(BattleActor p_Target, List<Special> p_SpecialList)
    {
        string l_SpecialList = GetSpecialNames(p_SpecialList);
        string l_Text = string.Empty;

        if (p_SpecialList.Count > 1)
        {
            l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Immunites", new string[] { p_Target.actorName, l_SpecialList });
        }
        else
        {
            l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Immunity", new string[] { p_Target.actorName, l_SpecialList });
        }

        TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { l_Text });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);

        BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_Step);
    }

    private void ShowRestoration(BattleActor l_BattleActor, RestorationData l_RestorationData)
    {
        string l_Text = string.Empty;

        switch (l_RestorationData.type)
        {
            case RestorationType.DebuffClear:
                l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:DebuffClear", new string[] { l_BattleActor.actorName });
                break;
            default:
                l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:" + l_RestorationData.type, new string[] { l_BattleActor.actorName, l_RestorationData.value.ToString() });
                break;
        }

        TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { l_Text });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);

        BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_Step);
    }

    private void ShowBonuses()
    {
        string l_Bonuses = string.Empty;

        if (m_Bonuses.ContainsKey(BonusType.Health))
        {
            l_Bonuses += LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:HpBonus", new string[] { m_Bonuses[BonusType.Health].ToString() });
        }
        if (m_Bonuses.ContainsKey(BonusType.SpecialPoints))
        {
            if (l_Bonuses != string.Empty)
            {
                l_Bonuses += " " + LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:And") + " ";
            }
            l_Bonuses += LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:SpBonus", new string[] { m_Bonuses[BonusType.SpecialPoints].ToString() });
        }

        string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Bonus", new string[] { BattlePlayer.GetInstance().actorName, l_Bonuses });
    }

    private void ShowAddedSpecials(BattleActor p_Target, List<Special> p_SpecialList)
    {
        string l_SpecialList = GetSpecialNames(p_SpecialList);
        string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:EffectUsing", new string[] { p_Target.actorName, l_SpecialList });

        TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { l_Text });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);

        BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_Step);
    }

    private string GetSpecialNames(List<Special> p_SpecialList)
    {
        string l_Text = string.Empty;

        for (int i = 0; i < p_SpecialList.Count; i++)
        {
            l_Text += p_SpecialList[i].specialName;
            if (i == p_SpecialList.Count - 2)
            {
                l_Text += " " + LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:And") + " ";
            }
            else if (i == p_SpecialList.Count - 1)
            {
                l_Text += "";
            }
            else
            {
                l_Text += ", ";
            }
        }

        return l_Text;
    }

    private void RunAfterAttackSteps()
    {
        for (int i = 0; i < m_AfterAttackSteps.Count; i++)
        {
            ResultSystem.GetInstance().AddStep(m_AfterAttackSteps[i]);
        }
    }

    private void Reset()
    {
        m_BeforeAttackSteps.Clear();
        m_VisualEffectSteps.Clear();
        m_DamageValues.Clear();
        m_AoeSpecials.Clear();
        m_ImmunitySpecials.Clear();
        m_AddedSpecials.Clear();
        m_RestorationValues.Clear();
        m_AfterAttackSteps.Clear();
        m_StatisticText = string.Empty;
    }

    private float CalculateDamage(BattleActor p_SenderActor, BattleActor p_TargetActor, float l_AttackDamage, Element l_AttackElement)
    {
        float l_SenderLevel = p_SenderActor.level;
        float l_TargetLevel = p_TargetActor.level;
        float l_SenderAttackStat = p_SenderActor.attackStat;
        float l_TargetDefenseStat = p_TargetActor.defenseStat;
        float l_Modif = ElementSystem.GetInstance().GetModif(l_AttackElement, p_TargetActor.element);
        l_Modif *= p_TargetActor.GetModif(l_AttackElement);

        float l_Damage = (l_SenderAttackStat / l_TargetDefenseStat) * (l_SenderLevel / l_TargetLevel) * l_AttackDamage * l_Modif;
        
        return l_Damage;
    }
}
