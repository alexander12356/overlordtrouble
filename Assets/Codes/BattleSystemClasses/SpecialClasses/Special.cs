﻿using System;
using System.Collections.Generic;

using UnityEngine;

public class Special
{
    private List<BaseEffect> m_EffectsList = new List<BaseEffect>();
    private string m_SpecialId = string.Empty;
    private string m_Id;
    private float m_Sp = 0.0f;
    private Element m_Element = Element.NONE;
    private bool m_IsAoe = false;
    
    public string id
    {
        get { return m_Id; }
    }

    public Special(string p_Id, float p_Sp, string p_Element, bool p_IsAoe)
    {
        m_Id = p_Id;
        m_Sp = p_Sp;
        m_Element = (Element)Enum.Parse(typeof(Element), p_Element);
        m_IsAoe = p_IsAoe;
    }

    public void SetEffects(List<BaseEffect> p_Effects)
    {
        m_EffectsList = p_Effects;
    }

    public void Upgrade()
    {
        for (int i = 0; i < m_EffectsList.Count; i++)
        {
            m_EffectsList[i].Upgrade();
        }
    }

    public void Run(BattleActor p_Sender, BattleActor p_Actor)
    {
        for (int i = 0; i < m_EffectsList.Count; i++)
        {
            m_EffectsList[i].Run(p_Sender, p_Actor);
        }
    }
}
