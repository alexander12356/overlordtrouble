using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryView 
{
    public InventoryPanel parent { get; set; }
    public abstract void Init();
    public abstract void Disable();
    public abstract void ShowDescription();
    public abstract void ClearDescription();
    public abstract void AddItem(InventoryItemData pInventoryItemData);
    public abstract void Confrim();
    public abstract void CancelAction();
    public abstract void SelectItem();
    public abstract void UpdateKey();
}
