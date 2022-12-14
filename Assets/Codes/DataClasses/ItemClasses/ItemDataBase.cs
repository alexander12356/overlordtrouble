using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;

public class ItemDataBase : Singleton<ItemDataBase>
{
    private Dictionary<string, ItemData> m_Items   = new Dictionary<string, ItemData>();
    private string m_PathFile = "Data/ItemList";

    public ItemDataBase()
    {
        Parse();
    }

    public Dictionary<string, ItemData> GetItemsDictionary(string p_Id)
    {
        return m_Items;
    }

    public ItemData GetItem(string p_Id)
    {
        try
        {
            return m_Items[p_Id];
        }
        catch
        {
            Debug.LogError("Cannot find item, id: " + p_Id);
            return new ItemData();
        }
    }

    private void Parse()
    {
        string l_DecodedString = "";

        if (File.Exists(Application.streamingAssetsPath + "/" + m_PathFile + ".json"))
        {
            l_DecodedString = File.ReadAllText(Application.streamingAssetsPath + "/" + m_PathFile + ".json");
        }
        else
        {
            try
            {
                TextAsset l_TextAsset = (TextAsset)Resources.Load(m_PathFile);
                l_DecodedString = l_TextAsset.ToString();
            }
            catch
            {
                Debug.LogError("CANNOT READ FOR " + GetType());
            }
        }

        JSONObject l_ItemTypeList = new JSONObject(l_DecodedString);

        m_Items = ParseItems(l_ItemTypeList["Items"]);
    }

    private Dictionary<string, ItemData> ParseItems(JSONObject p_ItemListObject)
    {
        Dictionary<string, ItemData> l_ItemList = new Dictionary<string, ItemData>();
        for (int i = 0; i < p_ItemListObject.Count; i++)
        {
            string l_ItemId    = p_ItemListObject[i]["Id"].str;
            string l_ImagePath = p_ItemListObject[i]["Image"].str;
            ItemType l_ItemType = (ItemType)Enum.Parse(typeof(ItemType), p_ItemListObject[i]["Type"].str);
            List<EffectData> l_EffectList = EffectSystem.GetInstance().ParseEffect(p_ItemListObject[i]["Effect"]);

            ItemData l_ItemData = new ItemData(l_ItemId, l_ImagePath, l_ItemType, l_EffectList);
            l_ItemList.Add(l_ItemId, l_ItemData);
        }

        return l_ItemList;
    }
}
