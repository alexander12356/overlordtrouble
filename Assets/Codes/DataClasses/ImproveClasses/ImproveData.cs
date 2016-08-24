using System.Collections.Generic;

[System.Serializable]
public struct ImproveData
{
    public string id;
    public string profileImagePath;
    public List<SkillData> skills;

    public ImproveData(string p_Id, string p_ProfileImagePath, List<SkillData> p_SkillList)
    {
        id = p_Id;
        profileImagePath = p_ProfileImagePath;
        skills = p_SkillList;
    }
}
