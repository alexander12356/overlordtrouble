public struct InventorySlotData
{
    public string slotId;
    public Slot slotType;
    public string itemId;
    
    public InventorySlotData(string pSlotId, Slot pSlot, string pItemId)
    {
        slotId = pSlotId;
        slotType = pSlot;
        itemId = pItemId;
    }
}