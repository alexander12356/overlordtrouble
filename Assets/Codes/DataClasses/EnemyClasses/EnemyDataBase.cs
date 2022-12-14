using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;

public enum EnemyAttackTarget
{
    NONE,
    Player,
    Enemies
}

public class EnemyDataBase : Singleton<EnemyDataBase>
{
    #region Variables
    private Dictionary<string, EnemyData> m_EnemyBase = new Dictionary<string, EnemyData>();
    private string m_PathFile = "Data/Enemies/";
    #endregion

    #region Interface
    public EnemyDataBase()
    {
        Parse();
    }

    public Dictionary<string, EnemyData> GetEnemyBase()
    {
        return m_EnemyBase;
    }

    public EnemyData GetEnemy(string p_EnemyId)
    {
        try
        {
            return m_EnemyBase[p_EnemyId];
        }
        catch
        {
            Debug.LogError("Cannot find EnemyData for id: " + p_EnemyId);
            return new EnemyData();
        }
    }
    #endregion

    #region Private
    private void Parse()
    {
        if (Directory.Exists(Application.streamingAssetsPath + "/Data/Enemies/"))
        {
            string[] l_FilesPath = Directory.GetFiles(Application.streamingAssetsPath + "/Data/Enemies/", "*.json");
            
            for (int i = 0; i < l_FilesPath.Length; i++)
            {
                ParseEnemy(File.ReadAllText(l_FilesPath[i]));
            }
        }
        else
        {
            TextAsset[] l_TextAssets = Resources.LoadAll<TextAsset>(m_PathFile);
            for (int i = 0; i < l_TextAssets.Length; i++)
            {
                ParseEnemy(l_TextAssets[i].ToString());
            }
        }
    }

    private void ParseEnemy(string p_DecodedString)
    {

        JSONObject l_JSONObject = new JSONObject(p_DecodedString);

        string  l_Id          = l_JSONObject["Id"].str;
        float   l_AttackStat  = l_JSONObject["Attack"].f;
        float   l_DefenseStat = l_JSONObject["Defense"].f;
        float   l_SpeedStat   = l_JSONObject["Speed"].f;
        int     l_Level       = (int)l_JSONObject["Level"].i;
        float   l_Health      = l_JSONObject["Health"].f;
        Element l_Element     = (Element)Enum.Parse(typeof(Element), l_JSONObject["Element"].str);
        int     l_Experience  = (int)l_JSONObject["Experience"].i;

        List<EnemyAttackData> l_AttackDataList = ParseAttack(l_JSONObject["Attacks"]);
        List<EnemyLootData>   l_LootList       = ParseLoots(l_JSONObject["Loot"]);

        string[] l_Property = ParseProperty(l_JSONObject["Property"]);
        
        EnemyData l_ImproveData = new EnemyData(l_Id, l_AttackStat, l_DefenseStat, l_SpeedStat, l_Level, l_Health, l_Element, l_AttackDataList, l_Experience, l_LootList, l_Property);

        m_EnemyBase.Add(l_Id, l_ImproveData);
    }

    private List<EnemyAttackData> ParseAttack(JSONObject p_JSONObject)
    {
        List<EnemyAttackData> l_AttackList = new List<EnemyAttackData>();
        for (int i = 0; i < p_JSONObject.Count; i++)
        {
            string l_AttackId = p_JSONObject[i]["Id"].str;
            Element l_Element = (Element)Enum.Parse(typeof(Element), p_JSONObject[i]["Element"].str);

            EnemyAttackTarget l_TargetId = EnemyAttackTarget.NONE;
            if (p_JSONObject[i].HasField("Target"))
            {
                l_TargetId = (EnemyAttackTarget)Enum.Parse(typeof(EnemyAttackTarget), p_JSONObject[i]["Target"].str);
            }

            List<EffectData> l_EffectList = EffectSystem.GetInstance().ParseEffect(p_JSONObject[i]["Effect"]);
            EnemyAttackData l_EnemyAttack = new EnemyAttackData(l_AttackId, l_Element, l_TargetId, l_EffectList);

            l_AttackList.Add(l_EnemyAttack);
        }

        return l_AttackList;
    }

    private List<EnemyLootData> ParseLoots(JSONObject p_JSONObject)
    {
        List<EnemyLootData> l_LootList = new List<EnemyLootData>();
        for (int i = 0; i < p_JSONObject.Count; i++)
        {
            l_LootList.Add(ParseLoot(p_JSONObject[i]));
        }
        return l_LootList;
    }

    private EnemyLootData ParseLoot(JSONObject p_LootJson)
    {
        string l_Id = p_LootJson["Id"].str;

        float l_Chance = 100;
        if (p_LootJson.HasField("Chance"))
        {
            l_Chance = p_LootJson["Chance"].f;
        }
        
        int l_Value1 = 1;
        int l_Value2 = 1;
        if (p_LootJson["Count"].Count > 0)
        {
            l_Value1 = (int)p_LootJson["Count"][0].i;
            l_Value2 = (int)p_LootJson["Count"][1].i;
        }
        else
        {
            l_Value1 = (int)p_LootJson["Count"].i;
            l_Value2 = (int)p_LootJson["Count"].i;
        }
        int[] l_Count = new int[2] { l_Value1, l_Value2 };

        return new EnemyLootData(l_Id, l_Count, l_Chance);
    }

    private string[] ParseProperty(JSONObject p_JSONObject)
    {
        int l_PropertyCount = p_JSONObject.Count;

        string[] l_Property = new string[l_PropertyCount];

        for (int i = 0; i < p_JSONObject.Count; i++)
        {
            l_Property[i] = p_JSONObject.list[i].f.ToString();
        }

        return l_Property;
    }
    #endregion
}
