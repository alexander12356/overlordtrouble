using System;
using System.Collections.Generic;

public enum EffectIds
{
    NONE = -1,
    Attack,
    AttackBuff,
    AttackDebuff,
    AttackRange,
    Defense,
    DefensePercent,
    DefenseDebuff,
    Healing,
    Restoring,
    Training,
    ClearSpecialStatus,
    EnemyHealing,
    StunPossibility,
    Poison,
    Acceleration,
    GeneralSpecialPointsBuff,
    SpeedBuff
}

public class EffectSystem : Singleton<EffectSystem>
{
    private Dictionary<BattleActor, List<Special>> m_RemovedSpecials = new Dictionary<BattleActor, List<Special>>();

    public BaseEffect CreateEffect(Special p_Special, EffectData p_EffectData)
    {
        BaseEffect l_Effect = null;

        switch (p_EffectData.type)
        {
            case EffectIds.Attack:
                float l_AttackValue = Convert.ToSingle(p_EffectData.parameters[0]);

                l_Effect = new AttackEffect(p_Special, l_AttackValue);
                break;
            case EffectIds.AttackRange:
                float[] l_AttackRangeValue = new float[2];
                l_AttackRangeValue[0] = Convert.ToSingle(p_EffectData.parameters[0]);
                l_AttackRangeValue[1] = Convert.ToSingle(p_EffectData.parameters[1]);

                l_Effect = new AttackRangeEffect(p_Special, l_AttackRangeValue);
                break;
            case EffectIds.Defense:
                float l_DefenseValue = Convert.ToSingle(p_EffectData.parameters[0]);
                int l_Duration = Convert.ToInt32(p_EffectData.parameters[1]);

                l_Effect = new DefenseEffect(p_Special, l_DefenseValue, l_Duration);
                break;
            case EffectIds.DefensePercent:
                int l_DefensePercentChance = Convert.ToInt32(p_EffectData.parameters[0]);
                int l_DefensePercentValue = Convert.ToInt32(p_EffectData.parameters[1]);
                int l_DefensePercentDuration = Convert.ToInt32(p_EffectData.parameters[2]);

                l_Effect = new DefensePercent(p_Special, l_DefensePercentChance, l_DefensePercentValue, l_DefensePercentDuration);
                break;
            case EffectIds.DefenseDebuff:
                float l_DefenseDebuffValue = Convert.ToSingle(p_EffectData.parameters[0]);
                int l_DefenseDebuffDuration = Convert.ToInt32(p_EffectData.parameters[1]);

                l_Effect = new DefenseDebuffEffect(p_Special, l_DefenseDebuffValue, l_DefenseDebuffDuration);
                break;
            case EffectIds.Healing:
                float l_HealingValue = Convert.ToSingle(p_EffectData.parameters[0]);

                l_Effect = new HealingEffect(p_Special, l_HealingValue);
                break;
            case EffectIds.ClearSpecialStatus:
                l_Effect = new ClearSpecialStatusEffect(p_Special);
                break;
            case EffectIds.AttackBuff:
                float l_AttackBuffValue = Convert.ToSingle(p_EffectData.parameters[0]);
                float l_AttackBuffDuration = Convert.ToSingle(p_EffectData.parameters[1]);

                l_Effect = new AttackBuffEffect(p_Special, l_AttackBuffValue, l_AttackBuffDuration);
                break;
            case EffectIds.Training:
                int l_ExperienceValue = Convert.ToInt32(p_EffectData.parameters[0]);

                l_Effect = new TrainingEffect(p_Special, l_ExperienceValue);
                break;
            case EffectIds.Restoring:
                float l_RestoringValue = Convert.ToSingle(p_EffectData.parameters[0]);

                l_Effect = new RestoringEffect(p_Special, l_RestoringValue);
                break;
            case EffectIds.EnemyHealing:
                float l_EnemyHealingValue = Convert.ToSingle(p_EffectData.parameters[0]);

                l_Effect = new EnemyHealing(p_Special, l_EnemyHealingValue);
                break;
            case EffectIds.StunPossibility:
                int l_StunChance = Convert.ToInt32(p_EffectData.parameters[0]);

                l_Effect = new StunPossibility(p_Special, l_StunChance);
                break;
            case EffectIds.AttackDebuff:
                int l_DefenseDebuffPercentChance = Convert.ToInt32(p_EffectData.parameters[0]);
                int l_DefenseDebuffPercentValue = Convert.ToInt32(p_EffectData.parameters[1]);
                int l_DefenseDebuffPercentDuration = Convert.ToInt32(p_EffectData.parameters[2]);

                l_Effect = new AttackDebuff(p_Special, l_DefenseDebuffPercentChance, l_DefenseDebuffPercentValue, l_DefenseDebuffPercentDuration);
                break;
            case EffectIds.Poison:
                int l_PoisonChance = Convert.ToInt32(p_EffectData.parameters[0]);
                int l_PoisonDuration = Convert.ToInt32(p_EffectData.parameters[1]);

                l_Effect = new PoisonEffect(p_Special, l_PoisonChance, l_PoisonDuration);
                break;
            case EffectIds.Acceleration:
                int l_AccelerationChance = Convert.ToInt32(p_EffectData.parameters[0]);
                int l_AccelerationRepeatCount = Convert.ToInt32(p_EffectData.parameters[1]);
                int l_AccelerationDuration = Convert.ToInt32(p_EffectData.parameters[2]);

                l_Effect = new Acceleration(p_Special, l_AccelerationChance, l_AccelerationRepeatCount, l_AccelerationDuration);
                break;
            case EffectIds.GeneralSpecialPointsBuff:
                int l_GeneralSpecialPointsBuffChance = Convert.ToInt32(p_EffectData.parameters[0]);
                int l_GeneralSpecialPointsValue = Convert.ToInt32(p_EffectData.parameters[1]);
                int l_GeneralSpecialPointsDuration = Convert.ToInt32(p_EffectData.parameters[2]);

                l_Effect = new GeneralSpecialPointsBuff(p_Special, l_GeneralSpecialPointsBuffChance, l_GeneralSpecialPointsValue, l_GeneralSpecialPointsDuration);
                break;
            case EffectIds.SpeedBuff:
                int l_SpeedBuffChance = Convert.ToInt32(p_EffectData.parameters[0]);
                int l_SpeedBuffPointsValue = Convert.ToInt32(p_EffectData.parameters[1]);
                int l_SpeedBuffDuration = Convert.ToInt32(p_EffectData.parameters[2]);

                l_Effect = new SpeedBuff(p_Special, l_SpeedBuffChance, l_SpeedBuffPointsValue, l_SpeedBuffDuration);
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

    public List<EffectData> ParseEffect(JSONObject p_JsonObject)
    {
        List<EffectData> p_EffectList = new List<EffectData>();

        for (int i = 0; i < p_JsonObject.Count; i++)
        {
            EffectIds l_EffectType = (EffectIds)Enum.Parse(typeof(EffectIds), p_JsonObject.keys[i]);

            switch (l_EffectType)
            {
                case EffectIds.Attack:
                    float l_AttackValue = p_JsonObject[i]["AttackValue"].f;

                    EffectData l_AttackEffect = new EffectData(l_EffectType, new string[] { l_AttackValue.ToString() });
                    p_EffectList.Add(l_AttackEffect);
                    break;
                case EffectIds.AttackRange:
                    float l_AttackValue1 = p_JsonObject[i]["Value1"].f;
                    float l_AttackValue2 = p_JsonObject[i]["Value2"].f;

                    EffectData l_AttackRangeEffect = new EffectData(l_EffectType, new string[] { l_AttackValue1.ToString(), l_AttackValue2.ToString() });
                    p_EffectList.Add(l_AttackRangeEffect);
                    break;
                case EffectIds.Defense:
                    float l_DefenseValue = p_JsonObject[i]["Value"].f;
                    float l_Duration = p_JsonObject[i]["Duration"].f;

                    EffectData l_DefenseEffect = new EffectData(l_EffectType, new string[] { l_DefenseValue.ToString(), l_Duration.ToString() });
                    p_EffectList.Add(l_DefenseEffect);
                    break;
                case EffectIds.DefensePercent:
                    int l_DefensePercentChance = (int)p_JsonObject[i]["Chance"].i;
                    int l_DefensePercentValue = (int)p_JsonObject[i]["Value"].i;
                    int l_DefensePercentDuration = (int)p_JsonObject[i]["Duration"].i;

                    EffectData l_DefensePercentEffect = new EffectData(l_EffectType, new string[] { l_DefensePercentChance.ToString(), l_DefensePercentValue.ToString(), l_DefensePercentDuration.ToString() });
                    p_EffectList.Add(l_DefensePercentEffect);
                    break;
                case EffectIds.DefenseDebuff:
                    float l_DefenseDebuffValue = p_JsonObject[i]["Value"].f;
                    float l_DefenseDebuffDuration = p_JsonObject[i]["Duration"].f;

                    EffectData l_DefenseDebuffEffect = new EffectData(l_EffectType, new string[] { l_DefenseDebuffValue.ToString(), l_DefenseDebuffDuration.ToString() });
                    p_EffectList.Add(l_DefenseDebuffEffect);
                    break;
                case EffectIds.Healing:
                    float l_HealingValue = p_JsonObject[i]["Value"].f;

                    EffectData l_HealingEffect = new EffectData(l_EffectType, new string[] { l_HealingValue.ToString() });
                    p_EffectList.Add(l_HealingEffect);
                    break;
                case EffectIds.ClearSpecialStatus:
                    EffectData l_ClearSpecialStatusEffect = new EffectData(l_EffectType, new string[] { });
                    p_EffectList.Add(l_ClearSpecialStatusEffect);
                    break;
                case EffectIds.AttackBuff:
                    float l_AttackBuffStat = p_JsonObject[i]["Value"].f;
                    float l_AttackBuffDuration = p_JsonObject[i]["Duration"].f;

                    EffectData l_AttackBuffEffect = new EffectData(l_EffectType, new string[] { l_AttackBuffStat.ToString(), l_AttackBuffDuration.ToString() });
                    p_EffectList.Add(l_AttackBuffEffect);
                    break;
                case EffectIds.Training:
                    float l_TrainingValue = p_JsonObject[i]["Value"].f;

                    EffectData l_TrainingEffect = new EffectData(l_EffectType, new string[] { l_TrainingValue.ToString() });
                    p_EffectList.Add(l_TrainingEffect);
                    break;
                case EffectIds.Restoring:
                    float l_RestoringValue = p_JsonObject[i]["Value"].f;

                    EffectData l_RestoringEffect = new EffectData(l_EffectType, new string[] { l_RestoringValue.ToString() });
                    p_EffectList.Add(l_RestoringEffect);
                    break;
                case EffectIds.EnemyHealing:
                    float l_EnemyHealingValue = p_JsonObject[i]["Value"].f;

                    EffectData l_EnemyHealingEffect = new EffectData(l_EffectType, new string[] { l_EnemyHealingValue.ToString() });
                    p_EffectList.Add(l_EnemyHealingEffect);
                    break;
                case EffectIds.StunPossibility:
                    int l_StunChance = (int)p_JsonObject[i]["Chance"].i;

                    EffectData l_StunPossibilityEffect = new EffectData(l_EffectType, new string[] { l_StunChance.ToString() });
                    p_EffectList.Add(l_StunPossibilityEffect);
                    break;
                case EffectIds.AttackDebuff:
                    int l_DefenseDebuffPercentChance = (int)p_JsonObject[i]["Chance"].i;
                    int l_DefenseDebuffPercentValue = (int)p_JsonObject[i]["Value"].i;
                    int l_DefenseDebuffPercentDuration = (int)p_JsonObject[i]["Duration"].i;

                    EffectData l_EffectDefenseDebufPercent = new EffectData(l_EffectType, new string[] { l_DefenseDebuffPercentChance.ToString(), l_DefenseDebuffPercentValue.ToString(), l_DefenseDebuffPercentDuration.ToString() });
                    p_EffectList.Add(l_EffectDefenseDebufPercent);
                    break;
                case EffectIds.Poison:
                    int l_PoisonEffect = (int)p_JsonObject[i]["Chance"].i;
                    int l_PoisonDuration = (int)p_JsonObject[i]["Duration"].i;

                    EffectData l_PoisonData = new EffectData(l_EffectType, new string[] { l_PoisonEffect.ToString(), l_PoisonDuration.ToString() });
                    p_EffectList.Add(l_PoisonData);
                    break;
                case EffectIds.Acceleration:
                    int l_AccelerationChance = (int)p_JsonObject[i]["Chance"].i;
                    int l_AccelerationRepeatCount = (int)p_JsonObject[i]["RepeatCount"].i;
                    int l_AccelerationDuration = (int)p_JsonObject[i]["Duration"].i;

                    EffectData l_AccelerationData = new EffectData(l_EffectType, new string[] { l_AccelerationChance.ToString(), l_AccelerationRepeatCount.ToString(), l_AccelerationDuration.ToString() });
                    p_EffectList.Add(l_AccelerationData);
                    break;
                case EffectIds.GeneralSpecialPointsBuff:
                    int l_GeneralSpecialPointsBuffChance = (int)p_JsonObject[i]["Chance"].i;
                    int l_GeneralSpecialPointsValue = (int)p_JsonObject[i]["Value"].i;
                    int l_GeneralSpecialPointsDuration = (int)p_JsonObject[i]["Duration"].i;

                    EffectData l_GeneralSpecialPointsBuffData = new EffectData(l_EffectType, new string[] { l_GeneralSpecialPointsBuffChance.ToString(), l_GeneralSpecialPointsValue.ToString(), l_GeneralSpecialPointsDuration.ToString() });
                    p_EffectList.Add(l_GeneralSpecialPointsBuffData);
                    break;
                case EffectIds.SpeedBuff:
                    int l_SpeedBuffChance = (int)p_JsonObject[i]["Chance"].i;
                    int l_SpeedBuffValue = (int)p_JsonObject[i]["Value"].i;
                    int l_SpeedBuffDuration = (int)p_JsonObject[i]["Duration"].i;

                    EffectData l_SpeedBuffData = new EffectData(l_EffectType, new string[] { l_SpeedBuffChance.ToString(), l_SpeedBuffValue.ToString(), l_SpeedBuffDuration.ToString() });
                    p_EffectList.Add(l_SpeedBuffData);
                    break;
            }
        }

        return p_EffectList;
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

    private void Reset()
    {
        m_RemovedSpecials.Clear();
    }
}
