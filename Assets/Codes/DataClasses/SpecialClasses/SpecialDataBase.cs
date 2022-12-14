using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;

public class SpecialDataBase : Singleton<SpecialDataBase>
{
    private string m_PathFile = "Data/SpecialList";
    private Dictionary<string, SpecialData> m_SpecialDictionary = new Dictionary<string, SpecialData>();

    public SpecialDataBase()
    {
        Parse();
    }

    public SpecialData GetSpecialData(string p_Id)
    {
        try
        {
            return m_SpecialDictionary[p_Id];
        }
        catch
        {
            Debug.LogError("Cannot find special " + p_Id);
            return new SpecialData();
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

        JSONObject l_JSONObject = new JSONObject(l_DecodedString);

        for (int i = 0; i < l_JSONObject.Count; i++)
        {
            try
            {
                List<EffectData> l_EffectList = EffectSystem.GetInstance().ParseEffect(l_JSONObject[i]["Effect"]);

                string l_SpecialId = l_JSONObject.keys[i];
                float l_Sp = l_JSONObject[i]["Sp"].f;
                Element l_Element = (Element)Enum.Parse(typeof(Element), l_JSONObject[i]["Element"].str);
                bool l_IsAoe = l_JSONObject[i]["Aoe"].b;
                bool l_MySelf = false;
                if (l_JSONObject[i].HasField("Myself"))
                {
                    l_MySelf = l_JSONObject[i]["Myself"].b;
                }

                SpecialData l_SpecialData = new SpecialData(l_SpecialId, l_Sp, l_Element, l_IsAoe, l_MySelf, l_EffectList);

                m_SpecialDictionary.Add(l_SpecialId, l_SpecialData);
            }
            catch
            {
                Debug.LogError("Cannot parse effect for special: " + l_JSONObject.keys[i]);
            }
        }
    }

    
}