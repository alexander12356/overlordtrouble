using UnityEngine;
using UnityEngine.UI;

public class InventoryEquipmentButton : PanelButton
{
    private static InventoryEquipmentButton mPrefab = null;
    private string mItemId = string.Empty;
    private bool mEquipped = false;
    [SerializeField]
    private GameObject mEquipmentText = null;

    public static InventoryEquipmentButton prefab
    {
        get
        {
            if(mPrefab == null)
            {
                mPrefab = Resources.Load<InventoryEquipmentButton>("Prefabs/Button/InventoryEquipmentButton");
            }
            return mPrefab;
        }
    }

    public string itemId
    {
        get
        {
            return mItemId;
        }
        set
        {
            mItemId = value;
        }
    }

    public bool equipped
    {
        get
        {
            return mEquipped;
        }
        set
        {
            mEquipped = value;
            mEquipmentText.SetActive(mEquipped);
        }
    }
}
