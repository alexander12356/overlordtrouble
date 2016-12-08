[System.Serializable]
public struct MonstyleData
{
    public string id;
    public float  damage;
    public float  sp;
    public string element;
    public string descriptionId;

    public MonstyleData(string p_Id, float p_Damage, float p_Sp, string p_Element, string p_DescriptionId)
    {
        id     = p_Id;
        damage = p_Damage;
        sp   = p_Sp;
        element = p_Element;
        descriptionId = p_DescriptionId;
    }
}
