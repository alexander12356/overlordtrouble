using System;
using System.Collections.Generic;

public class Special
{
    private List<BaseEffect> m_EffectsList = new List<BaseEffect>();
    private string m_SpecialId = string.Empty;
    private string m_Id;
    private float m_Sp = 0.0f;
    private Element m_Element = Element.NONE;
    private bool m_IsAoe = false;
    private bool m_Myself = false;
    
    public string id
    {
        get { return m_Id; }
    }
    public bool isAoe
    {
        get { return m_IsAoe; }
    }
    public bool myself
    {
        get { return m_Myself; }
    }
    public Element element
    {
        get { return m_Element; }
    }

    public Special(string p_Id, float p_Sp, string p_Element, bool p_IsAoe, bool p_Myself)
    {
        m_Id = p_Id;
        m_Sp = p_Sp;
        m_Element = (Element)Enum.Parse(typeof(Element), p_Element);
        m_IsAoe = p_IsAoe;
        m_Myself = p_Myself;
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

    public void Run(BattleActor p_Sender, BattleActor p_Target)
    {
        for (int i = 0; i < m_EffectsList.Count; i++)
        {
            m_EffectsList[i].Run(p_Sender, p_Target);
        }
        return;
    }
}
