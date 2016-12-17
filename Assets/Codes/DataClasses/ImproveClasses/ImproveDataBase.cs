using UnityEngine;

using System.Collections.Generic;
using System.IO;

public class ImproveDataBase : Singleton<ImproveDataBase>
{
    #region Variables
    private Dictionary<string, ImproveData> m_ImproveDictionary = new Dictionary<string, ImproveData>();
    private string m_PathFile = "Data/ImproveList";
    #endregion

    #region Interface
    public ImproveDataBase()
    {
        Parse();
    }

    public ImproveData GetImprove(string l_ImproveName)
    {
        try
        {
            return m_ImproveDictionary[l_ImproveName];
        }
        catch
        {
            Debug.LogError("Cannot find Improve for id: " + l_ImproveName);
            return new ImproveData();
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
            string l_ElementalId = l_JSONObject[i]["Elemental"].str;
            string l_ProfileImagePath = l_JSONObject[i]["Profile"].str;

            List<SpecialData> l_Skills = new List<SpecialData>();
            for (int j = 0; j < l_JSONObject[i]["Skills"].Count; j++)
            {
                string l_SkillId = l_JSONObject[i]["Skills"][j].str;
                SpecialData l_SkillData = SpecialDataBase.GetInstance().GetSpecialData(l_SkillId);
                l_Skills.Add(l_SkillData);
            }

            ImproveData l_ImproveData = new ImproveData(l_ImproveId, l_ProfileImagePath, l_ElementalId, l_Skills);
            m_ImproveDictionary.Add(l_ImproveId, l_ImproveData);
        }
    }
    #endregion
}
