public struct ItemData
{
    public string id;
    public string imagePath;
    public string action;

    public ItemData(string p_Id, string p_ImagePath, string p_Action)
    {
        id        = p_Id;
        imagePath = p_ImagePath;
        action    = p_Action;
    }
}
