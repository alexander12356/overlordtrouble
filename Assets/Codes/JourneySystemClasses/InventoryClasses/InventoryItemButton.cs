using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemButton : PanelButtonUpdateKey
{
    private static InventoryItemButton m_Prefab = null;
    private int m_ItemCount;
    private string m_ItemId = string.Empty;
    private ButtonList m_ActionsButtonList;
    private event PanelActionHandler m_DestroyButtonAction = null;
    private int m_ThrownItemCount;
    [SerializeField]
    private Text m_ItemCountText = null;
    [SerializeField]
    private GameObject m_ActionList = null;
    
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
            m_ItemCountText.text = "x" + m_ItemCount.ToString();
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

    public ButtonList actionButtonList
    {
        get
        {
            if(m_ActionsButtonList == null)
            {
                m_ActionsButtonList = transform.FindChild("ActionList").GetComponentInChildren<ButtonList>();
            }
            return m_ActionsButtonList;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_ActionsButtonList = actionButtonList;
    }

    public override void UpdateKey()
    {
        base.UpdateKey();
        m_ActionsButtonList.UpdateKey();
    }

    public override void Activate(bool p_Value)
    {
        base.Activate(p_Value);
        m_ActionList.SetActive(p_Value);
        m_ActionsButtonList.isActive = p_Value;
    }

    public void InitActionButtonList()
    {
        if(ItemDataBase.GetInstance().GetItem(itemId).itemType == ItemType.SingleUse)
        {
            string l_UseStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Use");
            AddActionButton(l_UseStr, UseItem);

            string l_ThrowStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Throw");
            AddActionButton(l_ThrowStr, TryThrowItem);

            string l_BackStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Back");
            AddActionButton(l_BackStr, CancelActionList);
        }
        else if(ItemDataBase.GetInstance().GetItem(itemId).itemType == ItemType.Equipment || ItemDataBase.GetInstance().GetItem(itemId).itemType == ItemType.Weapon || ItemDataBase.GetInstance().GetItem(itemId).itemType == ItemType.MultipleUse)
        {
            string l_ThrowStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Throw");
            AddActionButton(l_ThrowStr, TryThrowItem);

            string l_BackStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Back");
            AddActionButton(l_BackStr, CancelActionList);
        }
        else if(ItemDataBase.GetInstance().GetItem(itemId).itemType == ItemType.Crucial)
        {
            string l_UseStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Use");
            AddActionButton(l_UseStr, UseItem);

            string l_BackStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Back");
            AddActionButton(l_BackStr, CancelActionList);
        }
    }

    private void AddActionButton(string p_ButtonTitle, PanelButtonActionHandler p_Action)
    {
        ItemActionButton l_ItemActionButton = Instantiate(ItemActionButton.prefab);
        l_ItemActionButton.title = p_ButtonTitle;
        l_ItemActionButton.AddAction(p_Action);

        actionButtonList.AddButton(l_ItemActionButton);
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
        }
        CancelAction();
    }

    private void TryThrowItem()
    {
        m_ThrownItemCount = 1;
        YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
        l_YesNoPanel.SetText("Вы действительно хотите выбросить " + itemId + "?");
        l_YesNoPanel.AddYesAction(ThrowItem);

        JourneySystem.GetInstance().ShowPanel(l_YesNoPanel, true);
    }

    private void ThrowItem()
    {
        int l_ItemCount = itemCount - m_ThrownItemCount;
        if (l_ItemCount <= 0)
        {
            itemCount = 0;
            StartCoroutine(DestroyButton());
        }
        itemCount = l_ItemCount;
        PlayerInventory.GetInstance().SetItemCount(itemId, itemCount);
        CancelAction();
    }

    private void CancelActionList()
    {
        CancelAction();
    }

    public void AddDestroyButtonAction(PanelActionHandler p_Action)
    {
        m_DestroyButtonAction += p_Action;
    }

    public void RemoveDestroyButtonAction(PanelActionHandler p_Action)
    {
        m_DestroyButtonAction -= p_Action;
    }

    private IEnumerator DestroyButton()
    {
        yield return new WaitForSeconds(0.1f);
        m_DestroyButtonAction();
    }
}
