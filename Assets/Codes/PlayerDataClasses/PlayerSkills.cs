﻿using UnityEngine;
using System.Collections.Generic;

public class PlayerSkills
{
    private List<SpecialData> m_SkillList = new List<SpecialData>();
    private Dictionary<string, SpecialData> m_SelectedSkills = new Dictionary<string, SpecialData>();

    public PlayerSkills()
    {

    }

    public void SelectSkill(string p_Id)
    {
        m_SelectedSkills.Add(p_Id, SpecialDataBase.GetInstance().GetSpecialData(p_Id));
    }

    public void UnselectSkill(string p_Id)
    {
        m_SelectedSkills.Remove(p_Id);
    }

    public Dictionary<string, SpecialData> GetSelectedSkills()
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

    public List<SpecialData> GetSkills()
    {
        return m_SkillList;
    }

    public void ResetData()
    {
        m_SkillList = new List<SpecialData>();
    }
}
