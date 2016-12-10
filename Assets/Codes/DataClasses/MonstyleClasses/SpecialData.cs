[System.Serializable]
public struct SpecialData
{
    public string id;
    public float  sp;
    public string element;
    public bool isAoe;

    public SpecialData(string p_Id, float p_Sp, string p_Element, bool p_isAoe)
    {
        id = p_Id;
        sp = p_Sp;
        element = p_Element;
        isAoe = p_isAoe;
    }
}
