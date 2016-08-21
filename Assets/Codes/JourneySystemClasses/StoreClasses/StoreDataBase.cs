using UnityEngine;
using System.Collections.Generic;

public class StoreDataBase : Singleton<StoreDataBase>
{
    private string m_PathFile = "Data/StoreData";
    private Dictionary<string, StoreItemData> m_SingleUseItems = null;
    private Dictionary<string, StoreItemData> m_MultipleUseItems = null;
    private Dictionary<string, StoreItemData> m_EquipmentItems = null;

    public StoreDataBase()
    {
        Parse();
    }

    public Dictionary<string, StoreItemData> GetSingleUseItems()
    {
        return m_SingleUseItems;
    }

    public Dictionary<string, StoreItemData> GetMultipleUseItems()
    {
        return m_MultipleUseItems;
    }

    public Dictionary<string, StoreItemData> GetEquipmentItems()
    {
        return m_EquipmentItems;
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

        JSONObject l_ItemTypeList = new JSONObject(l_DecodedString);

        for (int i = 0; i < l_ItemTypeList.Count; i++)
        {
            switch (l_ItemTypeList.keys[i])
            {
                case "SingleUse":
                    m_SingleUseItems = ParseItems(l_ItemTypeList[i]);
                    break;
                case "MultipleUse":
                    m_MultipleUseItems = ParseItems(l_ItemTypeList[i]);
                    break;
                case "Equipment":
                    m_EquipmentItems = ParseItems(l_ItemTypeList[i]);
                    break;
            }
        }
    }

    private Dictionary<string, StoreItemData> ParseItems(JSONObject p_ItemListObject)
    {
        Dictionary<string, StoreItemData> l_ItemList = new Dictionary<string, StoreItemData>();
        for (int i = 0; i < p_ItemListObject.Count; i++)
        {
            string l_ItemId = p_ItemListObject[i]["Id"].str;
            int l_ItemCount = (int)p_ItemListObject[i]["Count"].n;

            StoreItemData l_ItemData = new StoreItemData(l_ItemId, l_ItemCount);
            l_ItemList.Add(l_ItemId, l_ItemData);
        }

        return l_ItemList;
    }
}
