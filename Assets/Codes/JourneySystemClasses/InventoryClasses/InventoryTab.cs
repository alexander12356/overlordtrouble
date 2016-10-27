using UnityEngine;
using UnityEngine.UI;

public abstract class InventoryTab : MonoBehaviour
{
    public abstract void Confrim();
    public abstract void CancelAction();
    public abstract void SelectItem();
    public abstract void DeselectItem();
    public abstract void UpdateKey();
}