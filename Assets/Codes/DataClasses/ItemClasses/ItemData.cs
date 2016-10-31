public enum ItemType
{
    SingleUse,
    MultipleUse,
    Equipment,
    Weapon,
    Crucial
}

public struct ItemData
{
    public string id;
    public string imagePath;
    public string action;
    public ItemType itemType;

    public ItemData(string p_Id, string p_ImagePath, string p_Action, ItemType p_ItemType)
    {
        id        = p_Id;
        imagePath = p_ImagePath;
        action    = p_Action;
        itemType = p_ItemType;
    }
}
