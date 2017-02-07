using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class PlayerInventory : Singleton<PlayerInventory>
{
    #region Variables;
    private string m_ItemsDefaultPathFile = "Data/InventoryItems";
    private string m_SlotDataDefaultPathFile = "Data/InventorySlotData";

    private Dictionary<string, InventoryItemData> m_Items = new Dictionary<string, InventoryItemData>();
    private Dictionary<string, InventorySlotData> m_SlotData = new Dictionary<string, InventorySlotData>();
    private int m_Coins;

    private enum eSlotType
    {
        normal,
        weapon,
        universal
    }
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

    public bool SlotsContainItem(string pItemId)
    {
        return m_SlotData.Values.Any(x => x.itemId == pItemId);
    }

    private void ParseSlotData()
    {
        string lDecodedString = "";

        TextAsset l_TextAsset = (TextAsset)Resources.Load(m_SlotDataDefaultPathFile);
        lDecodedString = l_TextAsset.ToString();

        JSONObject l_SlotDataJson = new JSONObject(lDecodedString);

        InitSlot(l_SlotDataJson);
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
            jListElement.AddField("SlotType", m_SlotData[lKey].slotType.GetType());
            jListElement.AddField("ItemId", m_SlotData[lKey].itemId);
            jList.Add(jListElement);
        }
        string lEncodedString = jSlotData.Print(true);
        File.WriteAllText(PlayerData.GetInstance().GetSavePath() + "InventorySlotData.json", lEncodedString);
    }

    private void ParseItemList()
    {
        string lDecodedString = "";

        TextAsset l_TextAsset = (TextAsset)Resources.Load(m_ItemsDefaultPathFile);
        lDecodedString = l_TextAsset.ToString();

        JSONObject l_JSONObject = new JSONObject(lDecodedString);

        InitItems(l_JSONObject);
    }

    public void SaveItemList()
    {
        JSONObject l_ItemList = new JSONObject(JSONObject.Type.OBJECT);
        l_ItemList.AddField("Coins", m_Coins);
        JSONObject l_List = new JSONObject(JSONObject.Type.ARRAY);
        l_ItemList.AddField("Items", l_List);
        foreach(var lKey in m_Items.Keys)
        {
            JSONObject l_ListElement = new JSONObject(JSONObject.Type.OBJECT);
            l_ListElement.AddField("Id", m_Items[lKey].id);
            l_ListElement.AddField("Count", m_Items[lKey].count);
            l_List.Add(l_ListElement);
        }
        string l_EncodedString = l_ItemList.Print(true);
        File.WriteAllText(PlayerData.GetInstance().GetSavePath() + "InventoryItems.json", l_EncodedString);
    }

    public void SaveToDisk()
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

    public void InitItems(JSONObject p_ItemsJson)
    {
        m_Coins = (int)p_ItemsJson["Coins"].i;

        JSONObject l_ItemList = p_ItemsJson["Items"];

        for (int i = 0; i < l_ItemList.Count; i++)
        {
            string l_ItemId = l_ItemList[i]["Id"].str;
            int l_ItemCount = (int)l_ItemList[i]["Count"].i;
            InventoryItemData l_ItemData = new InventoryItemData(l_ItemId, l_ItemCount);
            m_Items.Add(l_ItemId, l_ItemData);
        }
    }

    public void InitSlot(JSONObject p_SlotJson)
    {
        JSONObject l_SlotDataList = p_SlotJson["SlotData"];

        for (int i = 0; i < l_SlotDataList.Count; i++)
        {
            string l_SlotId = l_SlotDataList[i]["SlotId"].str;
            eSlotType l_SlotType = (eSlotType)Enum.Parse(typeof(eSlotType), l_SlotDataList[i]["SlotType"].str);
            Slot lSlot = null;
            switch (l_SlotType)
            {
                case eSlotType.normal:
                    lSlot = new NormalSlot();
                    break;
                case eSlotType.weapon:
                    lSlot = new WeaponSlot();
                    break;
                case eSlotType.universal:
                    lSlot = new UniversalSlot();
                    break;
            }
            string l_ItemId = l_SlotDataList[i]["ItemId"].str;
            InventorySlotData l_SlotData = new InventorySlotData(l_SlotId, lSlot, l_ItemId);
            m_SlotData.Add(l_SlotId, l_SlotData);
        }
    }

    public void LoadData(JSONObject p_ItemsJson, JSONObject p_SlotJson)
    {
        Clear();
        InitItems(p_ItemsJson);
        InitSlot(p_SlotJson);
    }

    public void Clear()
    {
        m_Items.Clear();
        m_SlotData.Clear();
    }

    public void NewGameDataInit()
    {
        Clear();
        ParseItemList();
        ParseSlotData();
    }
    #endregion
}
