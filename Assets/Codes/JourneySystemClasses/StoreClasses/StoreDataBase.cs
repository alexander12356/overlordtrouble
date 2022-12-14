using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class StoreDataBase : Singleton<StoreDataBase>
{
    private string m_PathFile = "Data/StoreData";
    private Dictionary<string, StoreItemData> m_StoreItems = new Dictionary<string, StoreItemData>();

    public StoreDataBase()
    {
        Parse();
    }

    public Dictionary<string, StoreItemData> GetStoreItem()
    {
        return m_StoreItems.Where(obj => obj.Value.buyCost != 0).ToDictionary(obj => obj.Key, obj => obj.Value);
    }

    public StoreItemData GetItem(string p_Id)
    {
        try
        {
            return m_StoreItems[p_Id];
        }
        catch
        {
            Debug.LogError("Cannot find item, id: " + p_Id);
            return new StoreItemData();
        }
    }

    private void Parse()
    {
        string l_DecodedString = "";
        try
        {
            TextAsset l_TextAsset = (TextAsset)Resources.Load(m_PathFile);
            l_DecodedString = l_TextAsset.ToString();
        }
        catch
        {
            Debug.LogError("CANNOT READ FOR " + GetType());
        }

        JSONObject l_ItemTypeList = new JSONObject(l_DecodedString)["Items"];

        for (int i = 0; i < l_ItemTypeList.Count; i++)
        {
            string l_ItemId = l_ItemTypeList[i]["Id"].str;
            int l_ItemBuyCost = (int)l_ItemTypeList[i]["BuyCost"].n;
            int l_ItemCellCost = (int)l_ItemTypeList[i]["CellCost"].n;

            StoreItemData l_ItemData = new StoreItemData(l_ItemId, l_ItemBuyCost, l_ItemCellCost);
            m_StoreItems.Add(l_ItemId, l_ItemData);
        }
    }
}
