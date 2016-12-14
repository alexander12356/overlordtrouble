﻿public class BaseEffect
{
    private string m_Id;

    protected Special m_Special = null;

    public string id
    {
        get { return m_Id;  }
        set { m_Id = value; }
    }

    public BaseEffect(Special p_Special)
    {
        m_Special = p_Special;
    }

    public virtual void Run(BattleActor p_Sender, BattleActor p_Target)
    {
    }

    public virtual void Upgrade()
    {
    }

    public virtual string GetDescription()
    {
        return string.Empty;
    }

    public virtual void Effective()
    {
    }

    public virtual bool CheckEnd()
    {
        return true;
    }

    public virtual void Stack(BaseEffect p_Effect)
    {
    }
}
