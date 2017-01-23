using System.Collections.Generic;

public enum ItemType
{
    SingleUse,
    MultipleUse,
    Bling,
    Weapon,
    Key
}

public struct ItemData
{
    public string id;
    public string imagePath;
    public ItemType itemType;
    public List<EffectData> effectsData;

    public ItemData(string p_Id, string p_ImagePath, ItemType p_ItemType, List<EffectData> p_EffectData)
    {
        id        = p_Id;
        imagePath = p_ImagePath;
        itemType = p_ItemType;
        effectsData = p_EffectData;
    }

    public Item CreateItem()
    {
        Item l_Item = new Item(id);

        List<BaseEffect> l_EffectList = new List<BaseEffect>();
        for (int i = 0; i < effectsData.Count; i++)
        {
            l_EffectList.Add(EffectSystem.GetInstance().CreateEffect(null, effectsData[i]));
        }
        l_Item.SetEffects(l_EffectList);
        l_Item.itemName = LocalizationDataBase.GetInstance().GetText("Item:" + id);

        return l_Item;
    }
}
