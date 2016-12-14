using System.Collections.Generic;

[System.Serializable]
public struct SpecialData
{
    public string id;
    public float  sp;
    public string element;
    public bool isAoe;
    public List<EffectData> effectsData;

    public SpecialData(string p_Id, float p_Sp, string p_Element, bool p_isAoe, List<EffectData> p_EffectsData)
    {
        id = p_Id;
        sp = p_Sp;
        element = p_Element;
        isAoe = p_isAoe;
        effectsData = p_EffectsData;
    }

    public Special CreateSpecial()
    {
        Special l_Special = new Special(id, sp, element, isAoe);

        List<BaseEffect> l_EffectList = new List<BaseEffect>();
        for (int i = 0; i < effectsData.Count; i++)
        {
            l_EffectList.Add(EffectSystem.GetInstance().CreateEffect(l_Special, effectsData[i]));
        }
        l_Special.SetEffects(l_EffectList);

        return l_Special;
    }
}
