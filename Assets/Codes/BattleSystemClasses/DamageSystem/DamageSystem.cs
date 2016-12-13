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
    private string m_StatisticText = string.Empty;
    private Dictionary<BattleActor, float> m_DamageValues = new Dictionary<BattleActor, float>();
    private Dictionary<BattleActor, List<Special>> m_AoeSpecials = new Dictionary<BattleActor, List<Special>>();
    private List<BattleActor> m_DeathActorList = new List<BattleActor>();

    public void Attack(BattleActor p_Sender, BattleActor p_Target, float p_DamageValue)
    {
        m_Sender = p_Sender;
        m_Target = p_Target;
        AddDamageValue(p_Target, p_DamageValue);

        string l_SenderName = m_Sender.actorName;
        string l_TargetName = m_Target.actorName;
        m_StatisticText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:PlayerAttack", new string[] { l_SenderName, l_TargetName, m_DamageValues[p_Target].ToString() });

        if (p_Target.IsCanDamage(m_DamageValues[p_Target]))
        {
            p_Target.Damage(m_DamageValues[p_Target]);
        }

        ShowResult();
    }

    public void MonstyleAttack(BattleActor p_Sender, BattleActor p_Target, List<Special> p_SpecialList)
    {
        m_Target = p_Target;
        
        MonstyleSystem.GetInstance().UsingMonstyle(p_Sender, p_Target, p_SpecialList);
        
        string l_UsesSpecialNames = MonstyleSystem.GetInstance().usesSpecialNames;
        string l_SenderName = p_Sender.actorName;
        string l_TargetName = p_Target.actorName;

        m_StatisticText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:MonstyleUsing", new string[] { l_SenderName, l_TargetName, l_UsesSpecialNames });
        
        if (m_DamageValues.ContainsKey(p_Target))
        {
            if (m_DamageValues[p_Target] > 0.01f)
            {
                m_StatisticText += LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:MonstyleDamage", new string[] { m_DamageValues[p_Target].ToString() });

                p_Target.Damage(m_DamageValues[p_Target]);
            }
        }

        ShowResult();
    }

    public void AttackSuccess()
    {
    }

    public void AttackFail()
    {
    }
    
    public void AddDamageValue(BattleActor p_Target, float p_Value)
    {
        if (!m_DamageValues.ContainsKey(p_Target))
        {
            m_DamageValues.Add(p_Target, p_Value);
        }
        else
        {
            m_DamageValues[p_Target] += p_Value;
        }
    }
    
    public void AddAoeSpecial(BattleActor p_Target, Special p_Special)
    {
        if (!m_AoeSpecials.ContainsKey(p_Target))
        {
            m_AoeSpecials.Add(p_Target, new List<Special>());
        }
        m_AoeSpecials[p_Target].Add(p_Special);
    }

    //  Добавление бафф/дебаффа
    //      Если врага нет в словаре используемых баффов
    //          добавить врага к словарю
    //      добавить в словарь по врагу спешл

    //  Добавление хиллки
    //      Если врага нет в словаре используемых хилок
    //          добавить врага к словарю
    //      добавить в словарь по врагу спешл

    //  Добавление иммунитета
    //      Если врага нет в словаре имунных к спешлам врага
    //          добавить врага к словарю
    //      добавить в словарь по врагу спешл

    public void AddDeathActor(BattleActor p_Target)
    {
        m_DeathActorList.Add(p_Target);
    }

    private void ShowResult()
    {
        TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { m_StatisticText });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);

        BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_Step);
        
        if (m_AoeSpecials.Count > 0)
        {
            foreach(BattleActor l_Actor in m_AoeSpecials.Keys)
            {
                ShowAoeText(l_Actor, m_AoeSpecials[l_Actor]);
            }
        }

        //  Если словарь иммунитетов не пуст
        //      Вывести текст(айди текста, словарь)
        //  Если словарь Баффов/дебаффов не пуст
        //      Вывести текст(айди текста, словарь)
        //  Если словарь Хилок не пуст
        //      Вывести текст(айди текста, словарь)
        //  Если словарь Бонусов не пуст
        //      Вывести текст(айди текста, словарь)
        //  Если словарь Результатов не пуст
        //      Вывести текст(айди текста, словарь)

        BattleCheckDeathStep l_CheckDeathStep = new BattleCheckDeathStep();
        ResultSystem.GetInstance().AddStep(l_CheckDeathStep);

        Reset();
    }

    private void ShowAoeText(BattleActor p_Target, List<Special> p_SpecialList)
    {
        string l_SpecialList = GetSpecialNames(p_SpecialList);
        string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:AoeUsing", new string[] { p_Target.actorName, l_SpecialList });

        if (m_DamageValues.ContainsKey(p_Target))
        {
            l_Text += LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:AoeDamage", new string[] { m_DamageValues[p_Target].ToString() });

            p_Target.Damage(m_DamageValues[p_Target]);
        }

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
            string l_SpecialName = LocalizationDataBase.GetInstance().GetText("Special:" + p_SpecialList[i].id);
            l_Text += l_SpecialName;
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

    private void Reset()
    {
        m_DamageValues.Clear();
        m_AoeSpecials.Clear();
        m_DeathActorList.Clear();
        m_StatisticText = string.Empty;
    }
}
