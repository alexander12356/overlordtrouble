using UnityEngine;

using System.Collections.Generic;

public class EffectController : MonoBehaviour
{
    private Dictionary<string, List<BaseEffect>> m_EffectList = new Dictionary<string, List<BaseEffect>>();
    private List<string> m_DeleteSpecials = new List<string>();

    public virtual void AddEffect(string p_SpecialId, BaseEffect p_Effect)
    {
        if (!m_EffectList.ContainsKey(p_SpecialId))
        {
            m_EffectList.Add(p_SpecialId, new List<BaseEffect>());
            m_EffectList[p_SpecialId].Add(p_Effect);
        }
    }

    public virtual bool HasSpecial(string p_SpecialId)
    {
        if (m_EffectList.ContainsKey(p_SpecialId))
        {
            return true;
        }
        return false;
    }

    public virtual void StackEffect(string p_SpecialId, BaseEffect p_Effect)
    {
        for (int i = 0; i < m_EffectList[p_SpecialId].Count; i++)
        {
            if (m_EffectList[p_SpecialId][i].id == p_Effect.id)
            {
                m_EffectList[p_SpecialId][i].Stack(p_Effect);
            }
        }
    }

    public virtual void RunningEffect()
    {
        foreach (string l_Id in m_EffectList.Keys)
        {
            for (int i = 0; i < m_EffectList[l_Id].Count; i++)
            {
                m_EffectList[l_Id][i].Effective();
                if (m_EffectList[l_Id][i].CheckEnd())
                {
                    m_EffectList[l_Id].RemoveAt(i);
                    i--;
                }
            }
            if (m_EffectList[l_Id].Count == 0)
            {
                m_DeleteSpecials.Add(l_Id);
            }
        }

        for (int i = 0; i < m_DeleteSpecials.Count; i++)
        {
            m_EffectList.Remove(m_DeleteSpecials[i]);
        }
        m_DeleteSpecials.Clear();
    }

    public void EffectEndImmediately(string p_EffectId)
    {
        foreach (string l_Id in m_EffectList.Keys)
        {
            for (int i = 0; i < m_EffectList[l_Id].Count; i++)
            {
                string l_EffectId = m_EffectList[l_Id][i].id;
                if (l_EffectId == p_EffectId)
                {
                    m_EffectList[l_Id][i].EndImmediately();
                    m_EffectList[l_Id].RemoveAt(i);
                    i--;
                }
            }
            if (m_EffectList[l_Id].Count == 0)
            {
                m_DeleteSpecials.Add(l_Id);
            }
        }
        for (int i = 0; i < m_DeleteSpecials.Count; i++)
        {
            m_EffectList.Remove(m_DeleteSpecials[i]);
        }
        m_DeleteSpecials.Clear();
    }

    public bool HasEffect(string p_EffectId)
    {
        foreach (string l_Id in m_EffectList.Keys)
        {
            for (int i = 0; i < m_EffectList[l_Id].Count; i++)
            {
                string l_EffectId = m_EffectList[l_Id][i].id;
                if (l_EffectId == p_EffectId)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
