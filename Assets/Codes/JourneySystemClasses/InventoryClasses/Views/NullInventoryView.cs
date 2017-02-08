public class NullInventoryView : InventoryView
{
    public override bool Confirm()
    {
        return false;
    }

    public override void Init()
    {
    }

    public override bool isNull()
    {
        return true;
    }

    public override void Disable()
    {
        base.Disable();
    }
}