public struct InventorySlotData
{
    public string slotId;
    public eSlotType slotType;
    public string itemId;
    
    public InventorySlotData(string pSlotId, eSlotType pSlotType, string pItemId)
    {
        slotId = pSlotId;
        slotType = pSlotType;
        itemId = pItemId;
    }
}