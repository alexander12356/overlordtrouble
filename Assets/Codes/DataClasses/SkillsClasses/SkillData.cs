[System.Serializable]
public struct SkillData
{
    public string id;
    public float  damage;
    public float  mana;
    public string element;
    public string descriptionId;

    public SkillData(string p_Id, float p_Damage, float p_Mana, string p_Element, string p_DescriptionId)
    {
        id     = p_Id;
        damage = p_Damage;
        mana   = p_Mana;
        element = p_Element;
        descriptionId = p_DescriptionId;
    }
}
