using UnityEngine;
using System.Collections.Generic;

public class PlayerSkills
{
    private List<SkillData> m_SkillList = new List<SkillData>();
    private Dictionary<string, SkillData> m_SelectedSkills = new Dictionary<string, SkillData>();

    public PlayerSkills()
    {

    }

    public void SelectSkill(string p_Id)
    {
        m_SelectedSkills.Add(p_Id, SkillDataBase.GetInstance().GetSkillData(p_Id));
    }

    public void UnselectSkill(string p_Id)
    {
        m_SelectedSkills.Remove(p_Id);
    }

    public Dictionary<string, SkillData> GetSelectedSkills()
    {
        return m_SelectedSkills;
    }

    public void AddSkills(List<SkillData> p_SkillList)
    {
        for (int i = 0; i < p_SkillList.Count; i++)
        {
            m_SkillList.Add(p_SkillList[i]);
        }        
    }

    public List<SkillData> GetSkills()
    {
        return m_SkillList;
    }

    public void ResetData()
    {
        m_SkillList = new List<SkillData>();
    }
}
