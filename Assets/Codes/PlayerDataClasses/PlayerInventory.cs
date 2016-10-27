using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : Singleton<PlayerInventory>
{
    #region Variables;
    private Dictionary<string, InventoryItemData> m_Items = new Dictionary<string, InventoryItemData>();
    private int m_Coins = 200;
    #endregion

    #region Interface
    public int coins
    {
        get { return m_Coins; }
        set { m_Coins = value; }
    }

    public Dictionary<string, InventoryItemData> GetInventoryItems()
    {
        return m_Items;
    }

    public PlayerInventory()
    {

    }

    public void AddItem(string p_ItemId, int p_Count)
    {
        if (m_Items.ContainsKey(p_ItemId))
        {
            InventoryItemData lInventoryItemData;
            lInventoryItemData.id = p_ItemId;
            lInventoryItemData.count = m_Items[p_ItemId].count + p_Count;
            m_Items[p_ItemId] = lInventoryItemData;
        }
        else
        {
            InventoryItemData lInventoryItemData;
            lInventoryItemData.id = p_ItemId;
            lInventoryItemData.count = p_Count;
            m_Items.Add(p_ItemId, lInventoryItemData);
        }
    }

    public int GetItemCount(string p_Id)
    {
        if (m_Items.ContainsKey(p_Id))
        {
            return m_Items[p_Id].count;
        }
        else
        {
            return 0;
        }
    }
    #endregion
}
