public struct StoreItemData
{
    public string id;
    public int cellCost;
    public int buyCost;

    public StoreItemData(string p_Id, int p_BuyCost, int p_CellCost)
    {
        id    = p_Id;
        buyCost = p_BuyCost;
        cellCost = p_CellCost;
    }
}
