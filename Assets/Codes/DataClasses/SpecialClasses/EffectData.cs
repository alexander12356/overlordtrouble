public struct EffectData
{
    public EffectIds type;
    public string[] parameters;

    public EffectData(EffectIds p_Type, string[] p_Parameters )
    {
        type = p_Type;
        parameters = p_Parameters;
    }
}
