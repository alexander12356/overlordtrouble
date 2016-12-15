using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectIcon : MonoBehaviour
{
    private static EffectIcon m_Prefab = null;
    private Image m_Image = null;

    public static EffectIcon prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<EffectIcon>("Prefabs/Effects/EffectIcon");
            }
            return m_Prefab;
        }
    }
	
    public void Awake()
    {
        m_Image = transform.FindChild("IconImage").GetComponent<Image>();
    }

    public void SetIconId(string m_Id)
    {
        m_Image.sprite = Resources.Load<Sprite>("Sprites/GUI/BattleSystem/EffectIcons/" + m_Id);
    }
}
