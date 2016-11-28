using UnityEngine;
using UnityEngine.UI;

public class ItemActionPanel : Panel
{
    private static ItemActionPanel m_Prefab = null;
    private ButtonList m_ActionsButtonList;

    private PanelActionHandler m_UseAction;
    private PanelActionHandlerWithParameter m_RemovingAction;
    private Animator m_Animator = null;

    private int m_CountToRemove = 0;
    private const int m_MinValue = 0;
    private const int m_MaxValue = 99;

    [SerializeField]
    private GameObject m_RemovingArrows = null;
    [SerializeField]
    private Text m_CountToRemoveText = null;

    public static ItemActionPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<ItemActionPanel>("Prefabs/Panels/ItemActionPanel");
            }
            return m_Prefab;
        }
    }

    public ButtonList actionButtonList
    {
        get
        {
            if (m_ActionsButtonList == null)
            {
                m_ActionsButtonList = transform.FindChild("ActionList").GetComponentInChildren<ButtonList>();
            }
            return m_ActionsButtonList;
        }
    }

    public int countToRemove
    {
        get
        {
            return m_CountToRemove;
        }
        set
        {
            m_CountToRemove = Mathf.Clamp(value, m_MinValue, m_MaxValue);
            m_CountToRemoveText.text = m_CountToRemove.ToString();
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_ActionsButtonList = actionButtonList;
        m_Animator = GetComponent<Animator>();
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
            return;

        if (m_RemovingArrows.activeInHierarchy)
        {
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                countToRemove += 1;
                m_Animator.SetTrigger("RightArrow");
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                countToRemove -= 1;
                m_Animator.SetTrigger("LeftArrow");
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                countToRemove += 10;
                m_Animator.SetTrigger("UpArrow");
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                countToRemove -= 10;
                m_Animator.SetTrigger("DownArrow");
            }
            else if (Input.GetKeyUp(KeyCode.Z))
            {
                TryToRemove();
            }
        }
        else
        {
            m_ActionsButtonList.UpdateKey();
        }
    }

    public void InitActionButtonList(string p_ItemId)
    {
        if (ItemDataBase.GetInstance().GetItem(p_ItemId).itemType == ItemType.SingleUse)
        {
            string l_UseStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Use");
            AddActionButton(l_UseStr, UseAction);

            string l_ThrowStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Throw");
            AddActionButton(l_ThrowStr, ActivateArrows);

            string l_BackStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Back");
            AddActionButton(l_BackStr, CancelAction);
        }
        else if (ItemDataBase.GetInstance().GetItem(p_ItemId).itemType == ItemType.Equipment || ItemDataBase.GetInstance().GetItem(p_ItemId).itemType == ItemType.Weapon || ItemDataBase.GetInstance().GetItem(p_ItemId).itemType == ItemType.MultipleUse)
        {
            string l_ThrowStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Throw");
            AddActionButton(l_ThrowStr, ActivateArrows);

            string l_BackStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Back");
            AddActionButton(l_BackStr, CancelAction);
        }
        else if (ItemDataBase.GetInstance().GetItem(p_ItemId).itemType == ItemType.Key)
        {
            string l_UseStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Use");
            AddActionButton(l_UseStr, UseAction);

            string l_BackStr = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory:Back");
            AddActionButton(l_BackStr, CancelAction);
        }
    }


    private void AddActionButton(string p_ButtonTitle, PanelButtonActionHandler p_Action)
    {
        ItemActionButton l_ItemActionButton = Instantiate(ItemActionButton.prefab);
        l_ItemActionButton.title = p_ButtonTitle;
        l_ItemActionButton.AddAction(p_Action);

        actionButtonList.AddButton(l_ItemActionButton);
    }

    public void AddUseAction(PanelActionHandler p_Action)
    {
        m_UseAction += p_Action;
    }

    public void AddRemovingAction(PanelActionHandlerWithParameter p_Action)
    {
        m_RemovingAction += p_Action;
    }

    private void UseAction()
    {
        if (m_UseAction != null)
        {
            m_UseAction();
        }
        Close();
    }

    private void ActivateArrows()
    {
        m_RemovingArrows.SetActive(true);
    }

    private void TryToRemove()
    {
        YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
        l_YesNoPanel.SetText("Вы действительно хотите выбросить ?");
        l_YesNoPanel.AddYesAction(RemovingAction);
        JourneySystem.GetInstance().ShowPanel(l_YesNoPanel, true);
    }

    private void RemovingAction()
    {
        if (m_RemovingAction != null)
        {
            m_RemovingAction(countToRemove);
        }
        Close();
    }

    private void CancelAction()
    {
        Close();
    }
}
