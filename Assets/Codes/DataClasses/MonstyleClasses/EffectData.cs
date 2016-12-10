public struct EffectData
{
    public EffectType type;
    public string[] parameters;

    public EffectData(EffectType p_Type, string[] p_Parameters )
    {
        type = p_Type;
        parameters = p_Parameters;
    }
}
