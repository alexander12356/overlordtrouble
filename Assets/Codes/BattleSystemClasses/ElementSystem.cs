using System;
using System.Collections.Generic;

using UnityEngine;

public enum Element
{
    NONE = -1,
    Live,
    Death,
    Dark,
    Nature,
    Fire,
    Water,
    Earth,
    Mana,
    Ancient,
    Physical
}

public class ElementSystem : Singleton<ElementSystem>
{
    private string m_PathFile = "Data/Element";
    private Dictionary<Element, Dictionary<Element, float>> m_ElementBalance = new Dictionary<Element, Dictionary<Element, float>>();

    public ElementSystem()
    {
        Parse();
    }

    public float GetModif(Element p_SenderElement, Element p_TargetElement)
    {
        if (!m_ElementBalance[p_SenderElement].ContainsKey(p_TargetElement))
        {
            return 1.0f;
        }
        else
        {
            return m_ElementBalance[p_SenderElement][p_TargetElement];
        }
    }

    private void Parse()
    {
        TextAsset l_TextAsset = Resources.Load<TextAsset>(m_PathFile);

        string l_DecodedString = "";
        l_DecodedString = l_TextAsset.ToString();

        JSONObject l_JSONObject = new JSONObject(l_DecodedString);

        for (int i = 0; i < l_JSONObject.Count; i++)
        {
            Element l_Element = (Element)Enum.Parse(typeof(Element), l_JSONObject.keys[i]);
            Dictionary<Element, float> l_ElementList = ParseBalance(l_JSONObject[i]);
            m_ElementBalance.Add(l_Element, l_ElementList);
        }
    }

    private Dictionary<Element, float> ParseBalance(JSONObject p_JSONObject)
    {
        Dictionary<Element, float> l_ElementList = new Dictionary<Element, float>();
        for (int i = 0; i < p_JSONObject.Count; i++)
        {
            Element p_TargetElement = (Element)Enum.Parse(typeof(Element), p_JSONObject.keys[i]);
            float p_Value = p_JSONObject[i].f;
            
            l_ElementList.Add(p_TargetElement, p_Value);
        }
        return l_ElementList;
    }
}
