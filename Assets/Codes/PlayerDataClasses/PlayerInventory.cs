using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class PlayerInventory : Singleton<PlayerInventory>
{
    #region Variables;
    private string m_ItemsPersistentPathFile = Application.persistentDataPath + "/InventoryItems.json";
    private string m_SlotDataPersistentPathFile = Application.persistentDataPath + "/InventorySlotData.json";
    private string m_ItemsDefaultPathFile = "Data/InventoryItems";
    private string m_SlotDataDefaultPathFile = "Data/InventorySlotData";

    private Dictionary<string, InventoryItemData> m_Items = new Dictionary<string, InventoryItemData>();
    private Dictionary<string, InventorySlotData> m_SlotData = new Dictionary<string, InventorySlotData>();
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
        return m_Items;
    }

    public Dictionary<string, InventorySlotData> GetInventorySlotData()
    {
        return m_SlotData;
    }

    public void UpdateSlotData(string pSlotId, InventorySlotData pSlotData)
    {
        if(m_SlotData.ContainsKey(pSlotId))
        {
            m_SlotData[pSlotId] = pSlotData;
        }
        else
        {
            m_SlotData.Add(pSlotId, pSlotData);
        }
    }

    public bool ItemAlreadyUsed(string pCurrentSlotId, string pTtemId)
    {
        return m_SlotData.Where(obj => obj.Key != pCurrentSlotId).Any(x => x.Value.itemId == pTtemId);
    }

    public bool SlotsContainItem(string pItemId)
    {
        return m_SlotData.Values.Any(x => x.itemId == pItemId);
    }

    private void ParseSlotData()
    {
        string lDecodedString = "";
        try
        {
            if (File.Exists(m_ItemsPersistentPathFile))
            {
                StreamReader streamReader = File.OpenText(m_SlotDataPersistentPathFile);
                lDecodedString = streamReader.ReadToEnd();
                streamReader.Close();
            }
            else
            {
                TextAsset l_TextAsset = (TextAsset)Resources.Load(m_SlotDataDefaultPathFile);
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
            m_SlotData.Add(l_SlotId, l_SlotData);
        }
    }

    public void SaveSlotData()
    {
        JSONObject jSlotData = new JSONObject(JSONObject.Type.OBJECT);
        JSONObject jList = new JSONObject(JSONObject.Type.ARRAY);
        jSlotData.AddField("SlotData", jList);
        foreach (var lKey in m_SlotData.Keys)
        {
            JSONObject jListElement = new JSONObject(JSONObject.Type.OBJECT);
            jListElement.AddField("SlotId", m_SlotData[lKey].slotId);
            switch(m_SlotData[lKey].slotType)
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
            jListElement.AddField("ItemId", m_SlotData[lKey].itemId);
            jList.Add(jListElement);
        }
        string lEncodedString = jSlotData.Print(true);
        File.WriteAllText(m_SlotDataPersistentPathFile, lEncodedString);
    }

    private void ParseItemList()
    {
        string lDecodedString = "";
        try
        {
            if (File.Exists(m_ItemsPersistentPathFile))
            {
                StreamReader streamReader = File.OpenText(m_ItemsPersistentPathFile);
                lDecodedString = streamReader.ReadToEnd();
                streamReader.Close();
            }
            else
            {
                TextAsset l_TextAsset = (TextAsset)Resources.Load(m_ItemsDefaultPathFile);
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
            m_Items.Add(l_ItemId, l_ItemData);
        }
    }

    public void SaveItemList()
    {
        JSONObject jItemList = new JSONObject(JSONObject.Type.OBJECT);
        jItemList.AddField("Coins", m_Coins);
        JSONObject jList = new JSONObject(JSONObject.Type.ARRAY);
        jItemList.AddField("Items", jList);
        foreach(var lKey in m_Items.Keys)
        {
            JSONObject jListElement = new JSONObject(JSONObject.Type.OBJECT);
            jListElement.AddField("Id", m_Items[lKey].id);
            jListElement.AddField("Count", m_Items[lKey].count);
            jList.Add(jListElement);
        }
        string lEncodedString = jItemList.Print(true);
        File.WriteAllText(m_ItemsPersistentPathFile, lEncodedString);
    }

    public void SaveAll()
    {
        SaveItemList();
        SaveSlotData();
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

    public void SetItemCount(string p_ItemId, int p_Count)
    {
        if (m_Items.ContainsKey(p_ItemId))
        {
            InventoryItemData lInventoryItemData;
            lInventoryItemData.id = p_ItemId;
            lInventoryItemData.count = p_Count;

            if (p_Count <= 0)
                m_Items.Remove(p_ItemId);
            else
                m_Items[p_ItemId] = lInventoryItemData;
        }
        else
        {
            InventoryItemData lInventoryItemData;
            lInventoryItemData.id = p_ItemId;
            lInventoryItemData.count = p_Count;
            if(p_Count > 0)
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
