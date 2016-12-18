using System.Collections.Generic;

[System.Serializable]
public struct SpecialData
{
    public string id;
    public float  sp;
    public Element element;
    public bool isAoe;
    public bool myself;
    public List<EffectData> effectsData;

    public SpecialData(string p_Id, float p_Sp, Element p_Element, bool p_isAoe, bool p_Myelf, List<EffectData> p_EffectsData)
    {
        id = p_Id;
        sp = p_Sp;
        element = p_Element;
        isAoe = p_isAoe;
        myself = p_Myelf;
        effectsData = p_EffectsData;
    }

    public Special CreateSpecial()
    {
        Special l_Special = new Special(id, element, isAoe, myself);

        List<BaseEffect> l_EffectList = new List<BaseEffect>();
        for (int i = 0; i < effectsData.Count; i++)
        {
            l_EffectList.Add(EffectSystem.GetInstance().CreateEffect(l_Special, effectsData[i]));
        }
        l_Special.SetEffects(l_EffectList);
        l_Special.specialName = LocalizationDataBase.GetInstance().GetText("Special:" + id);

        return l_Special;
    }
}
