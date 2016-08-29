using UnityEngine;
using System.Collections.Generic;

public class EnemyDataBase : Singleton<EnemyDataBase>
{
    #region Variables
    private Dictionary<string, EnemyData> m_EnemyBase = new Dictionary<string, EnemyData>();
    private string m_PathFile = "Data/EnemyList";
    #endregion

    #region Interface
    public EnemyDataBase()
    {
        Parse();
    }

    public EnemyData GetEnemy(string p_EnemyId)
    {
        try
        {
            return m_EnemyBase[p_EnemyId];
        }
        catch
        {
            Debug.LogError("Cannot find Improve for id: " + p_EnemyId);
            return new EnemyData();
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
            string l_ImproveId = l_JSONObject.keys[i];
            float  l_Health = l_JSONObject[i]["Health"].f;
            List<int> l_DamageValue = new List<int>() { (int)l_JSONObject[i]["DamageValue"][0].i, (int)l_JSONObject[i]["DamageValue"][1].i };

            EnemyData l_ImproveData = new EnemyData(l_ImproveId, l_Health, l_DamageValue);
            m_EnemyBase.Add(l_ImproveId, l_ImproveData);
        }
    }
    #endregion
}
