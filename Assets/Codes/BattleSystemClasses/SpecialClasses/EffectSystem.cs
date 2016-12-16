using System;
using System.Collections.Generic;

public class EffectSystem : Singleton<EffectSystem>
{
    private Dictionary<BattleActor, List<Special>> m_RemovedSpecials = new Dictionary<BattleActor, List<Special>>();

    public BaseEffect CreateEffect(Special p_Special, EffectData p_EffectData)
    {
        BaseEffect l_Effect = null;

        switch (p_EffectData.type)
        {
            case EffectType.Attack:
                float l_AttackValue = Convert.ToSingle(p_EffectData.parameters[0]);

                l_Effect = new AttackEffect(p_Special, l_AttackValue);
                break;
            case EffectType.Defense:

                float l_DefenseValue = Convert.ToSingle(p_EffectData.parameters[0]);
                int l_Duration = Convert.ToInt32(p_EffectData.parameters[1]);

                l_Effect = new DefenseEffect(p_Special, l_DefenseValue, l_Duration);
                break;
        }

        return l_Effect;
    }

    public void CheckEffects()
    {
        BattlePlayer.GetInstance().RunningEffect();
        List<BattleEnemy> l_EnemyList = BattleSystem.GetInstance().GetEnemyList();
        for (int i = 0; i < l_EnemyList.Count; i++)
        {
            l_EnemyList[i].RunningEffect();
        }

        if (m_RemovedSpecials.Count > 0)
        {
            foreach (BattleActor l_Actor in m_RemovedSpecials.Keys)
            {
                ShowRemovedSpecials(l_Actor, m_RemovedSpecials[l_Actor]);
            }
        }
        Reset();
    }

    public void AddRemoveEffectSpecial(BattleActor p_Target, Special p_Special)
    {
        if (!m_RemovedSpecials.ContainsKey(p_Target))
        {
            m_RemovedSpecials.Add(p_Target, new List<Special>());
        }
        m_RemovedSpecials[p_Target].Add(p_Special);
    }

    private void ShowRemovedSpecials(BattleActor p_Target, List<Special> p_SpecialList)
    {
        string l_SpecialList = GetSpecialNames(p_SpecialList);
        string l_Text = string.Empty;

        if (p_SpecialList.Count > 0)
        {
            l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:RemoveManyEffect", new string[] { l_SpecialList, p_Target.actorName });
        }
        else
        {
            l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:RemoveEffect", new string[] { l_SpecialList, p_Target.actorName });
        }

        TextPanel l_TextPanel = UnityEngine.Object.Instantiate(TextPanel.prefab);
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
        m_RemovedSpecials.Clear();
    }
}
