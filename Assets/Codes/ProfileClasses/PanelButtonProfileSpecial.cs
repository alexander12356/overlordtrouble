using UnityEngine;

public class PanelButtonProfileSpecial : PanelButton
{
    private static PanelButtonProfileSpecial m_Prefab;
    private bool m_Chosen = false;
    private Color m_PrevColor;
    private string m_MonstyleId = string.Empty;

    public static PanelButtonProfileSpecial prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<PanelButtonProfileSpecial>("Prefabs/Button/PanelButtonProfileSpecial");
            }
            return m_Prefab;
        }
    }
    public bool chosen
    {
        get { return m_Chosen;  }
        set
        {
            m_Chosen = value;
            if (m_Chosen)
            {
                text.color = Color.yellow;
            }
            else
            {
                text.color = Color.black;
            }
        }
    }
    public string monstyleId
    {
        get { return m_MonstyleId; }
        set
        {
            m_MonstyleId = value;
            title = LocalizationDataBase.GetInstance().GetText("Skill:" + m_MonstyleId);
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_PrevColor = text.color;
    }

    public override void Select(bool p_Value)
    {
        base.Select(p_Value);

        if (p_Value)
        {
            if (chosen)
            {
                text.color = Color.yellow;
            }
        }
        else
        {
            if (chosen)
            {
                text.color = Color.green;
            }
            else
            {
                text.color = Color.black;
            }
        }
    }
}
