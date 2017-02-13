using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private List<BaseEffect> m_EffectsList = new List<BaseEffect>();
    private string m_Id;
    private string m_ItemName = string.Empty;

    public string id
    {
        get { return m_Id; }
    }
    public string itemName
    {
        get { return m_ItemName; }
        set { m_ItemName = value; }
    }

    public Item(string p_Id)
    {
        m_Id = p_Id;
    }

    public void SetEffects(List<BaseEffect> p_Effects)
    {
        m_EffectsList = p_Effects;
    }

    public void Run(IEffectInfluenced p_Sender)
    {
        for (int i = 0; i < m_EffectsList.Count; i++)
        {
            m_EffectsList[i].Run(p_Sender, p_Sender);
        }
        return;
    }
}
