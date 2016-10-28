using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class PlayerInventory : Singleton<PlayerInventory>
{
    #region Variables;
    private string mItemsPersistentPathFile = Application.persistentDataPath + "/InventoryItems.json";
    private string mSlotDataPersistentPathFile = Application.persistentDataPath + "/InventorySlotData.json";
    private string mItemsDefaultPathFile = "Data/InventoryItems";
    private string mSlotDataDefaultPathFile = "Data/InventorySlotData";

    private Dictionary<string, InventoryItemData> mItems = new Dictionary<string, InventoryItemData>();
    private Dictionary<string, InventorySlotData> mSlotData = new Dictionary<string, InventorySlotData>();
    private int m_Coins;
    #endregion

    #region Interface
    public int coins
    {
        get { return m_Coins; }
        set { m_Coins = value; }
    }

    public PlayerInventory()
    {
        ParseItemList();
        ParseSlotData();
    }

    public Dictionary<string, InventoryItemData> GetInventoryItems()
    {
        return mItems;
    }

    public Dictionary<string, InventorySlotData> GetInventorySlotData()
    {
        return mSlotData;
    }

    public void UpdateSlotData(string pSlotId, InventorySlotData pSlotData)
    {
        if(mSlotData.ContainsKey(pSlotId))
        {
            mSlotData[pSlotId] = pSlotData;
        }
        else
        {
            mSlotData.Add(pSlotId, pSlotData);
        }
    }

    // TODO : Ужасное название метода, сменить
    public bool IsItemOccupied(string pCurrentSlotId, string pTtemId)
    {
        return mSlotData.Where(obj => obj.Key != pCurrentSlotId).Any(x => x.Value.itemId == pTtemId);
    }

    // TODO : Ужасное название метода, сменить
    public bool SlotsContainsItem(string pItemId)
    {
        return mSlotData.Values.Any(x => x.itemId == pItemId);
    }

    private void ParseSlotData()
    {
        string lDecodedString = "";
        try
        {
            if (File.Exists(mItemsPersistentPathFile))
            {
                StreamReader streamReader = File.OpenText(mSlotDataPersistentPathFile);
                lDecodedString = streamReader.ReadToEnd();
                streamReader.Close();
            }
            else
            {
                TextAsset l_TextAsset = (TextAsset)Resources.Load(mSlotDataDefaultPathFile);
                lDecodedString = l_TextAsset.ToString();
            }
        }
        catch
        {
            Debug.LogError("CANNOT READ FOR " + GetType());
        }

        JSONObject l_SlotDataList = new JSONObject(lDecodedString)["SlotData"];

        for(int i = 0; i < l_SlotDataList.Count; i++)
        {
            string l_SlotId = l_SlotDataList[i]["SlotId"].str;
            eSlotType l_SlotType = (eSlotType)Enum.Parse(typeof(eSlotType), l_SlotDataList[i]["SlotType"].str);
            string l_ItemId = l_SlotDataList[i]["ItemId"].str;
            InventorySlotData l_SlotData = new InventorySlotData(l_SlotId, l_SlotType, l_ItemId);
            mSlotData.Add(l_SlotId, l_SlotData);
        }
    }

    private void SaveSlotData()
    {
        JSONObject jSlotData = new JSONObject(JSONObject.Type.OBJECT);
        JSONObject jList = new JSONObject(JSONObject.Type.ARRAY);
        jSlotData.AddField("SlotData", jList);
        foreach (var lKey in mSlotData.Keys)
        {
            JSONObject jListElement = new JSONObject(JSONObject.Type.OBJECT);
            jListElement.AddField("SlotId", mSlotData[lKey].slotId);
            switch(mSlotData[lKey].slotType)
            {
                case eSlotType.normal:
                    jListElement.AddField("SlotType", "normal");
                    break;
                case eSlotType.weapon:
                    jListElement.AddField("SlotType", "weapon");
                    break;
                case eSlotType.universal:
                    jListElement.AddField("SlotType", "universal");
                    break;
            }
            jListElement.AddField("ItemId", mSlotData[lKey].itemId);
            jList.Add(jListElement);
        }
        string lEncodedString = jSlotData.Print();
        File.WriteAllText(mSlotDataPersistentPathFile, lEncodedString);
    }

    private void ParseItemList()
    {
        string lDecodedString = "";
        try
        {
            if (File.Exists(mItemsPersistentPathFile))
            {
                StreamReader streamReader = File.OpenText(mItemsPersistentPathFile);
                lDecodedString = streamReader.ReadToEnd();
                streamReader.Close();
            }
            else
            {
                TextAsset l_TextAsset = (TextAsset)Resources.Load(mItemsDefaultPathFile);
                lDecodedString = l_TextAsset.ToString();
            }
        }
        catch
        {
            Debug.LogError("CANNOT READ FOR " + GetType());
        }

        JSONObject l_JSONObject = new JSONObject(lDecodedString);

        m_Coins = (int) l_JSONObject["Coins"].i;

        JSONObject l_ItemList = new JSONObject(lDecodedString)["Items"];

        for(int i = 0; i < l_ItemList.Count; i++)
        {
            string l_ItemId = l_ItemList[i]["Id"].str;
            int l_ItemCount = (int)l_ItemList[i]["Count"].i;
            InventoryItemData l_ItemData = new InventoryItemData(l_ItemId, l_ItemCount);
            mItems.Add(l_ItemId, l_ItemData);
        }
    }

    private void SaveItemList()
    {
        JSONObject jItemList = new JSONObject(JSONObject.Type.OBJECT);
        jItemList.AddField("Coins", m_Coins);
        JSONObject jList = new JSONObject(JSONObject.Type.ARRAY);
        jItemList.AddField("Items", jList);
        foreach(var lKey in mItems.Keys)
        {
            JSONObject jListElement = new JSONObject(JSONObject.Type.OBJECT);
            jListElement.AddField("Id", mItems[lKey].id);
            jListElement.AddField("Count", mItems[lKey].count);
            jList.Add(jListElement);
        }
        string lEncodedString = jItemList.Print();
        File.WriteAllText(mItemsPersistentPathFile, lEncodedString);
    }

    public void Save()
    {
        SaveItemList();
        SaveSlotData();
    }

    public void AddItem(string p_ItemId, int p_Count)
    {
        if (mItems.ContainsKey(p_ItemId))
        {
            InventoryItemData lInventoryItemData;
            lInventoryItemData.id = p_ItemId;
            lInventoryItemData.count = mItems[p_ItemId].count + p_Count;
            mItems[p_ItemId] = lInventoryItemData;
        }
        else
        {
            InventoryItemData lInventoryItemData;
            lInventoryItemData.id = p_ItemId;
            lInventoryItemData.count = p_Count;
            mItems.Add(p_ItemId, lInventoryItemData);
        }
    }

    public int GetItemCount(string p_Id)
    {
        if (mItems.ContainsKey(p_Id))
        {
            return mItems[p_Id].count;
        }
        else
        {
            return 0;
        }
    }
    #endregion
}
