public class CountQuest : BaseQuest
{
    private int m_Count = 0;
    private int m_Counter = 0;

    public CountQuest(string p_Id, int p_Count) : base (p_Id)
    {
        m_Count = p_Count;
    }

    public override void Complete()
    {
        base.Complete();

        m_Counter++;

        if (m_Counter == m_Count)
        {
            complete = true;
        }
    }
}
