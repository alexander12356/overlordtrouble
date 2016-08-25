using UnityEngine;
using System.Collections.Generic;

public class PlayerSkills : Singleton<PlayerSkills>
{
    private List<SkillData> m_SkillList = new List<SkillData>();

    public List<SkillData> skillList
    {
        get { return m_SkillList; }
    }

    public PlayerSkills()
    {

    }

    public void AddSkills(List<SkillData> p_SkillList)
    {
        for (int i = 0; i < p_SkillList.Count; i++)
        {
            m_SkillList.Add(p_SkillList[i]);
        }        
    }
}
