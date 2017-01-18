using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryView 
{
    public InventoryPanel parent { get; set; }
    public abstract void Init();
    public abstract bool Confrim();
    public virtual void Disable() { }
    public virtual void ShowDescription() { }
    public virtual void ClearDescription() { }
    public virtual void AddItem(InventoryItemData pInventoryItemData) { }
    public virtual void CancelAction() { }
    public virtual void SelectItem() { }
    public virtual void UpdateKey() { }
}
