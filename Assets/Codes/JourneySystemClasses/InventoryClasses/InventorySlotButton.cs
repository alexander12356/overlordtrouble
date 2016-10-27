using UnityEngine;
using UnityEngine.UI;

public enum eSlotType
{
    normal,
    weapon,
    universal
}

public class InventorySlotButton : PanelButton
{
    private static InventorySlotButton mPrefab = null;
    private InventorySlotData mSlotData;
    private bool mIsFull = false;

    public static InventorySlotButton prefab
    {
        get
        {
            if(mPrefab == null)
            {
                mPrefab = Resources.Load<InventorySlotButton>("Prefabs/Button/InventorySlotButton");
            }
            return mPrefab;
        }
    }

    public string itemId
    {
        get
        {
            return mSlotData.itemId;
        }
    }

    public eSlotType slotType
    {
        get
        {
            return mSlotData.slotType;
        }
        set
        {
            mSlotData.slotType = value;
        }
    }

    public bool IsFull
    {
        get
        {
            return mIsFull;
        }
    }

    public InventorySlotData slotData
    {
        get
        {
            return mSlotData;
        }
    }

    public void SelectItem(string pItemId)
    {
        mIsFull = true;
        mSlotData.itemId = pItemId;
    }

    public void DeselectItem()
    {
        mIsFull = false;
        mSlotData.itemId = string.Empty;
    }
}
