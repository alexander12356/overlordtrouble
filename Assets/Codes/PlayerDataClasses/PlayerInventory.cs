using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : Singleton<PlayerInventory>
{
    #region Variables;
    private Dictionary<string, int> m_Items = new Dictionary<string, int>();
    private int m_Coins = 200;
    #endregion

    #region Interface
    public int coins
    {
        get { return m_Coins; }
        set { m_Coins = value; }
    }

    public PlayerInventory()
    {

    }

    public void AddItem(string p_ItemId, int p_Count)
    {
        if (m_Items.ContainsKey(p_ItemId))
        {
            m_Items[p_ItemId] += p_Count;
        }
        else
        {
            m_Items.Add(p_ItemId, p_Count);
        }
    }

    public int GetItemCount(string p_Id)
    {
        if (m_Items.ContainsKey(p_Id))
        {
            return m_Items[p_Id];
        }
        else
        {
            return 0;
        }
    }
    #endregion
}
