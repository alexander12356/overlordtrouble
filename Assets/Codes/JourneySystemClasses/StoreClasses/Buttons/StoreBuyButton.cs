
using System;
using UnityEngine;

public class StoreBuyButton : StoreItemButton
{
    private static StoreBuyButton m_Prefab = null;
    private string m_ItemId = string.Empty;

    public static StoreBuyButton prefab
    {
        get
        {
            if(m_Prefab == null)
            {
                m_Prefab = Resources.Load<StoreBuyButton>("Prefabs/Button/StoreBuyButton");
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
            itemCost = StoreDataBase.GetInstance().GetItem(m_ItemId).buyCost;
        }
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void StoreItemButtonAction()
    {
        if (PlayerInventory.GetInstance().coins >= countToAction * itemCost && countToAction > 0)
        {
            YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
            l_YesNoPanel.SetText("Вы действительно хотите купить " + title + "?");
            l_YesNoPanel.AddYesAction(action);

            JourneySystem.GetInstance().ShowPanel(l_YesNoPanel, true);
        }
        else
        {
            animator.SetTrigger("Lack");
        }
    }
}
