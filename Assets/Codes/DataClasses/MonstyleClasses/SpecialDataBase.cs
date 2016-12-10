﻿using UnityEngine;

using System;
using System.Collections.Generic;

public enum EffectType
{
    NONE = -1,
    Attack
}

public class SpecialDataBase : Singleton<SpecialDataBase>
{
    private string m_PathFile = "Data/MonstyleList";
    private Dictionary<string, SpecialList> m_SpecialDictionary = new Dictionary<string, SpecialList>();

    public SpecialDataBase()
    {
        Parse();
    }

    public SpecialData GetSpecialData(string p_Id)
    {
        try
        {
            return m_SpecialDictionary[p_Id].monstyleData;
        }
        catch
        {
            Debug.LogError("Cannot find special for id: " + p_Id);
            return new SpecialData();
        }
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

        JSONObject l_JSONObject = new JSONObject(l_DecodedString);

        for (int i = 0; i < l_JSONObject.Count; i++)
        {
            string l_SpecialId = l_JSONObject.keys[i];
            float  l_Sp        = l_JSONObject[i]["Sp"].f;
            string l_Element   = l_JSONObject[i]["Element"].str;
            bool   l_IsAoe     = l_JSONObject[i]["Aoe"].b;

            SpecialData l_SpecialData = new SpecialData(l_SpecialId, l_Sp, l_Element, l_IsAoe);
            List<EffectData> l_EffectList = ParseEffect(l_JSONObject[i]["Effect"]);

            SpecialList l_SpecialList = new SpecialList(l_SpecialData, l_EffectList);
            m_SpecialDictionary.Add(l_SpecialId, l_SpecialList);
        }
    }

    private List<EffectData> ParseEffect(JSONObject p_JsonObject)
    {
        List<EffectData> p_EffectList = new List<EffectData>();

        for (int i = 0; i < p_JsonObject.Count; i++)
        {
            EffectType l_EffectType = (EffectType)Enum.Parse(typeof(EffectType), p_JsonObject.str);

            switch (l_EffectType)
            {
                case EffectType.Attack:
                    float  l_AttackValue = p_JsonObject[i]["AttackValue"].f;
                    string l_Element     = p_JsonObject[i]["Element"].str;

                    EffectData l_AttackEffect = new EffectData(l_EffectType, new string[] { l_AttackValue.ToString(), l_Element }  );
                    p_EffectList.Add(l_AttackEffect);
                    break;
            }
        }

        return p_EffectList;
    }
}