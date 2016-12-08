using UnityEngine;

using System.Collections.Generic;

public class MonstyleDataBase : Singleton<MonstyleDataBase>
{
    private string m_PathFile = "Data/MonstyleList";
    private Dictionary<string, MonstyleData> m_SkillDictionary = new Dictionary<string, MonstyleData>();

    public MonstyleDataBase()
    {
        Parse();
    }

    public MonstyleData GetSkillData(string p_Id)
    {
        try
        {
            return m_SkillDictionary[p_Id];
        }
        catch
        {
            Debug.LogError("Cannot find Skill for id: " + p_Id);
            return new MonstyleData();
        }
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

        for (int i = 0; i < l_JSONObject.Count; i++)
        {
            string l_SkillId = l_JSONObject.keys[i];
            float l_Attack = l_JSONObject[i]["Attack"].f;
            float l_Mana = l_JSONObject[i]["Sp"].f;
            string l_Element = l_JSONObject[i]["Element"].str;
            string l_DescriptionId = l_JSONObject[i]["DescriptionId"].str;

            MonstyleData l_SkillData = new MonstyleData(l_SkillId, l_Attack, l_Mana, l_Element, l_DescriptionId);
            m_SkillDictionary.Add(l_SkillId, l_SkillData);
        }
    }
}