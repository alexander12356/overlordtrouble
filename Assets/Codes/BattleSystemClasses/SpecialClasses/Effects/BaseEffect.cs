public class BaseEffect
{
    public virtual void Run(BattleActor p_Sender, BattleActor m_Target)
    {
    }

    public virtual void Upgrade()
    {
    }

    public virtual string GetDescription()
    {
        return string.Empty;
    }
}
