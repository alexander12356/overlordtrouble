
using System;
using UnityEngine;

public class StoreCellButton : StoreItemButton
{
    private static StoreCellButton m_Prefab = null;
    private string m_ItemId = string.Empty;

    public static StoreCellButton prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<StoreCellButton>("Prefabs/Button/StoreCellButton");
            }
            return m_Prefab;
        }
    }

    public string itemId
    {
        get { return m_ItemId; }
        set
        {
            m_ItemId = value;
            itemCost = StoreDataBase.GetInstance().GetItem(m_ItemId).cellCost;
        }
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void StoreItemButtonAction()
    {
        if (PlayerInventory.GetInstance().GetItemCount(itemId) > 0 && countToAction > 0 && countToAction <= PlayerInventory.GetInstance().GetItemCount(itemId))
        {
            YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
            l_YesNoPanel.SetText("Вы действительно хотите продать " + title + "?");
            l_YesNoPanel.AddYesAction(action);
            l_YesNoPanel.AddPopAction(CancelAction);

            JourneySystem.GetInstance().ShowPanel(l_YesNoPanel, true);
        }
        else
        {
            animator.SetTrigger("Lack");
        }
    }
}
