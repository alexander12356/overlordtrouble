using UnityEngine;
using System.Collections.Generic;

public class ItemDataBase : Singleton<ItemDataBase>
{
    private Dictionary<string, ItemData> m_SingleUseItems   = new Dictionary<string, ItemData>();
    private Dictionary<string, ItemData> m_MultipleUseItems = new Dictionary<string, ItemData>();
    private Dictionary<string, ItemData> m_EquipmentItems   = new Dictionary<string, ItemData>();
    private string m_PathFile = "Data/ItemList";

    public ItemDataBase()
    {
        Parse();
    }

    public Dictionary<string, ItemData> GetSingleUseItems(string p_Id)
    {
        return m_SingleUseItems;
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

    private Dictionary<string, ItemData> ParseItems(JSONObject p_ItemListObject)
    {
        Dictionary<string, ItemData> l_ItemList = new Dictionary<string, ItemData>();
        for (int i = 0; i < p_ItemListObject.Count; i++)
        {
            string l_ItemId    = p_ItemListObject[i]["Id"].str;
            string l_ImagePath = p_ItemListObject[i]["Image"].str;
            //TODO ParseActions
            string l_Action    = p_ItemListObject[i]["Actions"][0]["Id"].str;

            ItemData l_ItemData = new ItemData(l_ItemId, l_ImagePath, l_Action);
            l_ItemList.Add(l_ItemId, l_ItemData);
        }

        return l_ItemList;
    }
}
