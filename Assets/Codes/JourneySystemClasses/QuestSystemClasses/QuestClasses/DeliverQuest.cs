public class DeliverQuest : BaseQuest
{
    public DeliverQuest(string p_Id) : base (p_Id)
    {

    }

    public override void Complete()
    {
        base.Complete();

        complete = true;
    }
}
