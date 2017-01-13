using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemButton : PanelButtonUpdateKey
{
    private static InventoryItemButton m_Prefab = null;
    private int m_ItemCount;
    private string m_ItemId = string.Empty; 
    [SerializeField]
    private Text m_ItemCountText = null;
    
    public static InventoryItemButton prefab
    {
        get
        {
            if(m_Prefab == null)
            {
                m_Prefab = Resources.Load<InventoryItemButton>("Prefabs/Button/InventoryItemButton");
            }
            return m_Prefab;
        }
    }

    public int itemCount
    {
        get
        {
            return m_ItemCount;
        }
        set
        {
            m_ItemCount = value;
            m_ItemCountText.text = "x" + m_ItemCount;
        }
    }

    public string itemId
    {
        get
        {
            return m_ItemId;
        }
        set
        {
            m_ItemId = value;
            itemCount = PlayerInventory.GetInstance().GetItemCount(m_ItemId);
        }
    }

    public void CreateItemActionPanel()
    {
        ItemActionPanel l_ItemActionPanel = Instantiate(ItemActionPanel.prefab);
        l_ItemActionPanel.InitActionButtonList(itemId);
        l_ItemActionPanel.AddUseAction(UseItem);
        l_ItemActionPanel.AddRemovingAction(RemoveItem);
        JourneySystem.GetInstance().ShowPanel(l_ItemActionPanel, true);
    }

    private void UseItem()
    {
        // TODO: Вызывать окно с выбором персонажа, а затем окно с описанием эффекта
        if (ItemDataBase.GetInstance().GetItem(itemId).itemType == ItemType.SingleUse)
        {
            int l_ItemCount = itemCount - 1;
            if (l_ItemCount <= 0)
            {
                itemCount = 0;
                StartCoroutine(DestroyButton());
            }
            itemCount = l_ItemCount;
            PlayerInventory.GetInstance().SetItemCount(itemId, itemCount);

            Item l_Item = ItemDataBase.GetInstance().GetItem(itemId).CreateItem();
            l_Item.Run(JourneySystem.GetInstance().player.statistics);
        }
    }

    private void RemoveItem(int p_CountToRemove)
    {
        int l_ItemCount = itemCount - p_CountToRemove;
        if (l_ItemCount <= 0)
        {
            itemCount = 0;
            StartCoroutine(DestroyButton());
        }
        itemCount = l_ItemCount;
        PlayerInventory.GetInstance().SetItemCount(itemId, itemCount);
    }

    private IEnumerator DestroyButton()
    {
        yield return new WaitForSeconds(0.1f);
        InventoryPanel l_InventoryPanel = GetComponentInParent<InventoryPanel>();
        l_InventoryPanel.ShowView();
    }
}
