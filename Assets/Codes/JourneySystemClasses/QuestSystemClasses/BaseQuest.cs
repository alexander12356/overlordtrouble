public class BaseQuest
{
    protected string m_Id;
    private bool m_Complete = false;

    public string id
    {
        get { return m_Id; }
        set { m_Id = value; }
    }
    public bool complete
    {
        get { return m_Complete;  }
        set { m_Complete = value; }
    }

    public BaseQuest(string p_Id)
    {
        m_Id = p_Id;
    }

    public virtual void Complete()
    {
    }

    public virtual bool HasComplete()
    {
        return false;
    }
}
