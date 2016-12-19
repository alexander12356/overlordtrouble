using System.Collections.Generic;

using UnityEngine;

using BattleSystemClasses.Bosses.Leshii;

public class LeshiiDataBase : Singleton<LeshiiDataBase>
{
    private string m_PathFile = "Data/Bosses/Leshii";
    private int m_Level = 0;
    private float m_AttackStat = 0.0f;
    private float m_DefenseStat = 0.0f;
    private Dictionary<OrganType, float> m_Healths = new Dictionary<OrganType, float>();
    private Dictionary<OrganType, float> m_DamageValue = new Dictionary<OrganType, float>();
    private Dictionary<OrganType, float> m_EffectChanse = new Dictionary<OrganType, float>();
    private float m_SpecialAttackValue = 0.0f;
    private float m_HealthValue = 0.0f;

    public LeshiiDataBase()
    {
        Parse();
    }

    public int GetLevel()
    {
        return m_Level;
    }
    
    public float GetAttackStat()
    {
        return m_AttackStat;
    }

    public float GetDefenseStat()
    {
        return m_DefenseStat;
    }
    
    public float GetHealth(OrganType p_Type)
    {
        return m_Healths[p_Type];
    }

    public float GetDamageValue(OrganType p_Type)
    {
        return m_DamageValue[p_Type];
    }

    public float GetEffectChanse(OrganType p_Type)
    {
        return m_EffectChanse[p_Type];
    }

    public float GetSpecialAttackValue()
    {
        return m_SpecialAttackValue;
    }

    public float GetHealthValue()
    {
        return m_HealthValue;
    }

    private void Parse()
    {
        string l_DecodedString = "";
        try
        {
            TextAsset l_TextAsset = (TextAsset)Resources.Load(m_PathFile);
            l_DecodedString = l_TextAsset.ToString();
        }
        catch
        {
            Debug.LogError("CANNOT READ FOR " + GetType());
        }

        JSONObject l_JSONObject = new JSONObject(l_DecodedString);

        m_Level = (int)l_JSONObject["Level"].f;
        m_AttackStat = l_JSONObject["Attack"].f;
        m_DefenseStat = l_JSONObject["Defense"].f;

        m_DamageValue.Add(OrganType.LeftHand, l_JSONObject["LeftHand"]["DamageValue"].f);
        m_DamageValue.Add(OrganType.RightHand, l_JSONObject["RightHand"]["DamageValue"].f);

        m_Healths.Add(OrganType.LeftHand, l_JSONObject["LeftHand"]["Health"].f);
        m_Healths.Add(OrganType.RightHand, l_JSONObject["RightHand"]["Health"].f);
        m_Healths.Add(OrganType.Body, l_JSONObject["Body"]["Health"].f);

        m_EffectChanse.Add(OrganType.LeftHand, l_JSONObject["LeftHand"]["EffectChanse"].f);
        m_EffectChanse.Add(OrganType.RightHand, l_JSONObject["RightHand"]["EffectChanse"].f);

        m_SpecialAttackValue = l_JSONObject["SpecialAttack"]["DamageValue"].f;

        m_HealthValue = l_JSONObject["RightHand"]["HealthValue"].f;
    }
}
