using System;
using System.Collections.Generic;

public class PlayerSkills
{
    private List<SpecialData> m_SkillList = new List<SpecialData>();
    private List<SpecialData> m_SelectedSkills = new List<SpecialData>();

    public PlayerSkills()
    {

    }

    public void SelectSkill(string p_Id)
    {
        m_SelectedSkills.Add(SpecialDataBase.GetInstance().GetSpecialData(p_Id));
    }

    public void UnselectSkill(string p_Id)
    {
        m_SelectedSkills.Remove(SpecialDataBase.GetInstance().GetSpecialData(p_Id));
    }

    public void RemoveFirstSelectedSkill()
    {
        m_SelectedSkills.RemoveAt(0);
    }

    public List<SpecialData> GetSelectedSkills()
    {
        return m_SelectedSkills;
    }

    public void AddSkills(List<SpecialData> p_SkillList)
    {
        for (int i = 0; i < p_SkillList.Count; i++)
        {
            m_SkillList.Add(p_SkillList[i]);
        }        
    }

    public void AddSelectedSkills(List<SpecialData> p_SkillList, bool overwrite = false)
    {
        if (overwrite)
            m_SelectedSkills.Clear();

        for (int i = 0; i < p_SkillList.Count; i++)
        {
            m_SelectedSkills.Add(p_SkillList[i]);
        }
    }

    public List<SpecialData> GetSkills()
    {
        return m_SkillList;
    }

    public void Clear()
    {
        m_SkillList = new List<SpecialData>();
    }

    public void ClearSelectedSkills()
    {
        m_SelectedSkills = new List<SpecialData>();
    }

    public void DefaultSkillSelection()
    {
        m_SelectedSkills.AddRange(m_SkillList);
    }

    public JSONObject GetSkillsJson()
    {
        JSONObject l_SpecialsJson = new JSONObject();

        for (int i = 0; i < m_SkillList.Count; i++)
        {
            l_SpecialsJson.Add(m_SkillList[i].id);
        }

        return l_SpecialsJson;
    }

    public JSONObject GetSelectedSkillsJson()
    {
        JSONObject l_SpecialsJson = new JSONObject();

        for (int i = 0; i < m_SelectedSkills.Count; i++)
        {
            l_SpecialsJson.Add(m_SelectedSkills[i].id);
        }

        return l_SpecialsJson;
    }
}
