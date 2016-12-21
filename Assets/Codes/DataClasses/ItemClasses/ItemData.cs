using System.Collections.Generic;

public enum ItemType
{
    SingleUse,
    MultipleUse,
    Equipment,
    Weapon,
    Key
}

public struct ItemData
{
    public string id;
    public string imagePath;
    public ItemType itemType;
    public List<EffectData> effectData;

    public ItemData(string p_Id, string p_ImagePath, ItemType p_ItemType, List<EffectData> p_EffectData)
    {
        id        = p_Id;
        imagePath = p_ImagePath;
        itemType = p_ItemType;
        effectData = p_EffectData;
    }
}
