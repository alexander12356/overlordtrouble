using UnityEngine;
using System.Collections.Generic;

public class PlayerSkills
{
    private List<MonstyleData> m_SkillList = new List<MonstyleData>();
    private Dictionary<string, MonstyleData> m_SelectedSkills = new Dictionary<string, MonstyleData>();

    public PlayerSkills()
    {

    }

    public void SelectSkill(string p_Id)
    {
        m_SelectedSkills.Add(p_Id, MonstyleDataBase.GetInstance().GetSkillData(p_Id));
    }

    public void UnselectSkill(string p_Id)
    {
        m_SelectedSkills.Remove(p_Id);
    }

    public Dictionary<string, MonstyleData> GetSelectedSkills()
    {
        return m_SelectedSkills;
    }

    public void AddSkills(List<MonstyleData> p_SkillList)
    {
        for (int i = 0; i < p_SkillList.Count; i++)
        {
            m_SkillList.Add(p_SkillList[i]);
        }        
    }

    public List<MonstyleData> GetSkills()
    {
        return m_SkillList;
    }

    public void ResetData()
    {
        m_SkillList = new List<MonstyleData>();
    }
}
