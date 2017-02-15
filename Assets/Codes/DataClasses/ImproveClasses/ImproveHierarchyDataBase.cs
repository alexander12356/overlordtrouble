using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ImproveHierarchyDataBase : Singleton<ImproveHierarchyDataBase>
{
    private string m_PathFile = "Data/ImproveHierarchy";
    private Dictionary<string, List<string>> m_ImproveHierarchuList = new Dictionary<string, List<string>>();

    public ImproveHierarchyDataBase()
    {
        Parse();
    }

    public List<string> GetImproveList(string p_ImproveId)
    {
        if (!m_ImproveHierarchuList.ContainsKey(p_ImproveId))
        {
            return new List<string>();
        }
        return m_ImproveHierarchuList[p_ImproveId];
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

        JSONObject l_Json = new JSONObject(l_DecodedString);

        for (int i = 0; i < l_Json.Count; i++)
        {
            string l_MainImproveId = l_Json.keys[i];
            m_ImproveHierarchuList.Add(l_MainImproveId, new List<string>());

            for (int j = 0; j < l_Json[i].Count; j++)
            {
                m_ImproveHierarchuList[l_MainImproveId].Add(l_Json[i].keys[j]);
            }
        }
    }
}
