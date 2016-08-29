using System.Collections.Generic;

[System.Serializable]
public struct ImproveData
{
    public string id;
    public string profileImagePath;
    public string elementalId;
    public List<SkillData> skills;

    public ImproveData(string p_Id, string p_ProfileImagePath, string p_ElementalId, List<SkillData> p_SkillList)
    {
        id = p_Id;
        profileImagePath = p_ProfileImagePath;
        elementalId = p_ElementalId;
        skills = p_SkillList;
    }
}
