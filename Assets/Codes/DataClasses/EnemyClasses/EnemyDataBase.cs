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
            Debug.LogError("Cannot find EnemyData for id: " + p_EnemyId);
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
            int l_Experience = (int)l_JSONObject[i]["Experience"].i;

            List<EnemyAttackData> l_AttackList = new List<EnemyAttackData>();
            for (int j = 0; j < l_JSONObject[i]["Attacks"].Count; j++)
            {
                string l_AttackId = l_JSONObject[i]["Attacks"][j]["Id"].str;
                int l_Temp1 = (int)l_JSONObject[i]["Attacks"][j]["DamageValue"][0].i;
                List<int> l_AttackValue = new List<int>() { (int)l_JSONObject[i]["Attacks"][j]["DamageValue"][0].i, (int)l_JSONObject[i]["Attacks"][j]["DamageValue"][1].i };

                EnemyAttackData l_EnemyAttack = new EnemyAttackData(l_AttackId, l_AttackValue);
                l_AttackList.Add(l_EnemyAttack);
            }

            EnemyData l_ImproveData = new EnemyData(l_ImproveId, l_Health, l_Experience, l_AttackList);
            m_EnemyBase.Add(l_ImproveId, l_ImproveData);
        }
    }
    #endregion
}
