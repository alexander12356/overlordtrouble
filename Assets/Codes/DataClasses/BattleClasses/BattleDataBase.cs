using UnityEngine;
using System.Collections.Generic;

public class BattleDataBase : Singleton<BattleDataBase>
{
    #region Variables
    private Dictionary<string, BattleData> m_BattleBase = new Dictionary<string, BattleData>();
    private string m_PathFile = "Data/BattleList";
    #endregion

    #region Interface
    public BattleDataBase()
    {
        Parse();
    }

    public BattleData GetBattle(string p_BattleId)
    {
        try
        {
            return m_BattleBase[p_BattleId];
        }
        catch
        {
            Debug.LogError("Cannot find BattleData for id: " + p_BattleId);
            return new BattleData();
        }
    }
    #endregion

    #region Private
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

        JSONObject l_JSONObject = new JSONObject(l_DecodedString);

        for (int i = 0; i < l_JSONObject.Count; i++)
        {
            string l_BattleId = l_JSONObject.keys[i];
            List<string> p_EnemyList = new List<string>();
            Dictionary<string, int> p_PlayerSettings = new Dictionary<string, int>();

            for (int j = 0; j < l_JSONObject[i]["Enemy"].Count; j++)
            {
                p_EnemyList.Add(l_JSONObject[i]["Enemy"][j].str);
            }
            if (l_JSONObject[i].HasField("Player"))
            {
                for (int j = 0; j < l_JSONObject[i]["Player"].Count; j++)
                {
                    p_PlayerSettings.Add(l_JSONObject[i]["Player"].keys[j], (int)l_JSONObject[i]["Player"][j].f);
                }
            }

            BattleData l_ImproveData = new BattleData(l_BattleId, p_EnemyList, p_PlayerSettings);
            m_BattleBase.Add(l_BattleId, l_ImproveData);
        }
    }
    #endregion
}
