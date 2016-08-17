using UnityEngine;

using System.Collections.Generic;

public class SkillDataBase : Singleton<SkillDataBase>
{
    private string m_PathFile = "Data/SkillList";
    private Dictionary<string, SkillData> m_SkillDictionary = new Dictionary<string, SkillData>();

    public SkillDataBase()
    {
        Parse();
    }

    public SkillData GetSkillData(string p_Id)
    {
        try
        {
            return m_SkillDictionary[p_Id];
        }
        catch
        {
            Debug.LogError("Cannot find Skill for id: " + p_Id);
            return new SkillData();
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
            float l_Mana = l_JSONObject[i]["Mana"].f;
            string l_DescriptionId = l_JSONObject[i]["DescriptionId"].str;

            SkillData l_SkillData = new SkillData(l_SkillId, l_Attack, l_Mana, l_DescriptionId);
            m_SkillDictionary.Add(l_SkillId, l_SkillData);
        }
    }
}