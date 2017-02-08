
using System;

public class NullStoreTab : StoreTab
{
    public override void DeselectItem()
    {
        throw new NotImplementedException();
    }

    public override void InitItemList()
    {
        throw new NotImplementedException();
    }

    public override void SelectItem()
    {
        throw new NotImplementedException();
    }

    public override bool Confirm()
    {
        return false;
    }

    public override void UpdateKey()
    {
        
    }
}
