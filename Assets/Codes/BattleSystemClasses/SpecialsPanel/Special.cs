public class Special
{
    public enum Element
    {
        Water,
        Fire,
        Earth,
        Air
    }

    #region Variables
    private string  m_Title;
    private int     m_Level = 0;
    private Element m_Element;
    private string  m_Id;
    private float   m_DamageValue = 5;
    private float   m_Mana = 4.0f;
    #endregion

    #region Interface
    public string title
    {
        get { return m_Title;  }
        set { m_Title = value; }
    }
    public int level
    {
        get { return m_Level;  }
        set { m_Level = value; }
    }
    public Element element
    {
        get { return m_Element;  }
        set { m_Element = value; }
    }
    public string id
    {
        get { return m_Id;  }
        set { m_Id = value; }
    }
    public float damageValue
    {
        get { return m_DamageValue;  }
        set { m_DamageValue = value; }
    }
    public float mana
    {
        get { return m_Mana;  }
        set { m_Mana = value; }
    }

    public Special(string p_Id)
    {
        m_Title = m_Id = p_Id;
    }
    #endregion
}
