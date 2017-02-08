using System.IO;

using UnityEngine;

using BattleSystemClasses.Bosses.Leshii;

using System.Collections.Generic;

public struct LeshiiData
{
    public int level;
    public int attackStat;
    public int defenseStat;
    public Dictionary<OrganType, float> organHealthValue;
    public Dictionary<OrganType, float> handsAttackValue;
    public Dictionary<OrganType, float> handsEffectChance;
    public float rightHandHealingValue;
    public float specialAttackValue;
    public int specialAttackChargeCount;
    public int summonHandsCount;
    public float criticalHealthValue;
}

public class LeshiiDataBase : Singleton<LeshiiDataBase>
{
    private string m_PathFile = "Data/Bosses/Leshii";

    private LeshiiData m_SimpleLeshiiData;
    private LeshiiData m_SpecialLeshiiData;

    public LeshiiDataBase()
    {
        Parse();
    }

    public LeshiiData GetSimpleLeshiiData()
    {
        return m_SimpleLeshiiData;
    }

    public LeshiiData GetSpecialLeshiiData()
    {
        return m_SpecialLeshiiData;
    }

    private void Parse()
    {
        m_SimpleLeshiiData = ParseLeshiiData("Simple");
        m_SpecialLeshiiData = ParseLeshiiData("Special");
    }

    private LeshiiData ParseLeshiiData(string p_Id)
    {
        string l_DecodedString = "";

        if (File.Exists(Application.streamingAssetsPath + "/" + m_PathFile + "_" + p_Id +".json"))
        {
            l_DecodedString = File.ReadAllText(Application.streamingAssetsPath + "/" + m_PathFile + "_" + p_Id + ".json");
        }
        else
        {
            try
            {
                TextAsset l_TextAsset = (TextAsset)Resources.Load(m_PathFile + "_" + p_Id);
                l_DecodedString = l_TextAsset.ToString();
            }
            catch
            {
                Debug.LogError("CANNOT READ FOR " + GetType());
            }
        }

        JSONObject l_LeshiiDataJson = new JSONObject(l_DecodedString);

        LeshiiData l_LeshiiData;

        l_LeshiiData.level = (int)l_LeshiiDataJson["Level"].i;
        l_LeshiiData.attackStat = (int)l_LeshiiDataJson["Attack"].i;
        l_LeshiiData.defenseStat = (int)l_LeshiiDataJson["Defense"].i;

        l_LeshiiData.handsAttackValue = new Dictionary<OrganType, float>();
        l_LeshiiData.handsAttackValue.Add(OrganType.LeftHand, l_LeshiiDataJson["LeftHand"]["DamageValue"].f);
        l_LeshiiData.handsAttackValue.Add(OrganType.RightHand, l_LeshiiDataJson["RightHand"]["DamageValue"].f);

        l_LeshiiData.organHealthValue = new Dictionary<OrganType, float>();
        l_LeshiiData.organHealthValue.Add(OrganType.LeftHand, l_LeshiiDataJson["LeftHand"]["Health"].f);
        l_LeshiiData.organHealthValue.Add(OrganType.RightHand, l_LeshiiDataJson["RightHand"]["Health"].f);
        l_LeshiiData.organHealthValue.Add(OrganType.Body, l_LeshiiDataJson["Body"]["Health"].f);

        l_LeshiiData.handsEffectChance = new Dictionary<OrganType, float>();
        l_LeshiiData.handsEffectChance.Add(OrganType.LeftHand, l_LeshiiDataJson["LeftHand"]["EffectChanse"].f);
        l_LeshiiData.handsEffectChance.Add(OrganType.RightHand, l_LeshiiDataJson["RightHand"]["EffectChanse"].f);

        l_LeshiiData.specialAttackValue = l_LeshiiDataJson["SpecialAttack"]["DamageValue"].f;
        l_LeshiiData.specialAttackChargeCount = (int)l_LeshiiDataJson["SpecialAttack"]["ChargeCount"].i;

        l_LeshiiData.rightHandHealingValue = l_LeshiiDataJson["RightHandHealingValue"].f;
        l_LeshiiData.criticalHealthValue = l_LeshiiDataJson["CriticalHealthValue"].f;
        l_LeshiiData.summonHandsCount = (int)l_LeshiiDataJson["HandSummonCount"].i;

        return l_LeshiiData;
    }
}
