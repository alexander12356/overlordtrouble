using System.Collections.Generic;

public struct SpecialList
{
    private SpecialData m_MonstyleData;
    private List<EffectData> m_EffectList;

    public EffectData this[int i]
    {
        get { return m_EffectList[i]; }
    }
    public int count
    {
        get { return m_EffectList.Count; }
    }
    public SpecialData monstyleData
    {
        get { return m_MonstyleData; }
    }

    public SpecialList(SpecialData p_MonstyleData, List<EffectData> p_EffectList)
    {
        m_MonstyleData = p_MonstyleData;
        m_EffectList = p_EffectList;
    }
}
