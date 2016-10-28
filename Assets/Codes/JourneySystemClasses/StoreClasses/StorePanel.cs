using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class StorePanel : Panel
{
    #region Variables
    private static StorePanel m_Prefab;
    private ButtonList m_ButtonList    = null;
    private TextBox    m_TextBox       = null;
    private StoreTab   m_CurrOpenedTab = null;
    private StoreItemButton m_CurrentSelectedItem = null;

    [SerializeField]
    private ButtonList m_TabButtonsList = null;

    [SerializeField]
    private GameObject m_Window = null;

    [SerializeField]
    private Text m_PlayerCoinsText = null;

    [SerializeField]
    private List<StoreTab> m_StoreTabs = null;
    #endregion

    #region Interface
    public static StorePanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<StorePanel>("Prefabs/Panels/StorePanel");
            }
            return m_Prefab;
        }
    }
    public ButtonList tabButtonList
    {
        get
        {
            return m_TabButtonsList;
        }
    }
    public StoreItemButton currentSelectedItem
    {
        get { return m_CurrentSelectedItem;  }
        set { m_CurrentSelectedItem = value; }
    }
    public int playerCoins
    {
        get { return PlayerInventory.GetInstance().coins; }
        set
        {
            PlayerInventory.GetInstance().coins = value;
            m_PlayerCoinsText.text = PlayerInventory.GetInstance().coins + " " + LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Monet");
        }
    }

    public override void Awake()
    {
        base.Awake();
        playerCoins = PlayerInventory.GetInstance().coins;

        InitTextBox();
        InitButtonActions();
        InitTabs();

        HideButtonList();
        m_TabButtonsList.isActive = false;
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ButtonList.UpdateKey();
        m_TabButtonsList.UpdateKey();
        m_CurrOpenedTab.itemsButtonList.UpdateKey();

        if (m_CurrentSelectedItem != null)
        {
            m_CurrentSelectedItem.UpdateKey();
        }

        m_TextBox.UpdateTextBox();
    }

    public override void PushAction()
    {
        base.PushAction();

        StartWelcomeDialog();
    }

    public void AddTalkingAnimator(Animator p_TalkingAnimator)
    {
        m_TextBox.SetTalkingAnimator(p_TalkingAnimator);
    }
    #endregion

    private void InitButtonActions()
    {
        m_ButtonList = GetComponentInChildren<ButtonList>();

        m_ButtonList[0].AddAction(OpenTabs);
        m_ButtonList[0].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Buy");
        m_ButtonList[1].AddAction(OpenTabs);
        m_ButtonList[1].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Sell");
        m_ButtonList[2].AddAction(StartDialog);
        m_ButtonList[2].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Talk");
        m_ButtonList[3].AddAction(CloseStore);
        m_ButtonList[3].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Leave");
    }

    private void InitTabs()
    {
        m_TabButtonsList.AddCancelAction(CloseTabs);
        m_TabButtonsList.AddKeyArrowAction(ShowTab);
        m_TabButtonsList[0].AddAction(ConfirmTab);
        m_TabButtonsList[0].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Tab:All");
        m_TabButtonsList[1].AddAction(ConfirmTab);
        m_TabButtonsList[1].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Tab:Equipments");
        m_TabButtonsList[2].AddAction(ConfirmTab);
        m_TabButtonsList[2].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Tab:SingleUse");
        m_TabButtonsList[3].AddAction(ConfirmTab);
        m_TabButtonsList[3].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Tab:MultipleUse");

        Dictionary<string, StoreItemData> l_StoreItems = StoreDataBase.GetInstance().GetStoreItem();
        foreach (string l_Key in l_StoreItems.Keys)
        {
            ItemType l_ItemDataType = ItemDataBase.GetInstance().GetItem(l_Key).itemType;
            switch (l_ItemDataType)
            {
                case ItemType.SingleUse:
                    m_StoreTabs[2].AddItem(l_StoreItems[l_Key]);
                    break;
                case ItemType.MultipleUse:
                    m_StoreTabs[3].AddItem(l_StoreItems[l_Key]);
                    break;
                case ItemType.Equipment:
                    m_StoreTabs[1].AddItem(l_StoreItems[l_Key]);
                    break;
                case ItemType.Weapon:
                    m_StoreTabs[1].AddItem(l_StoreItems[l_Key]);
                    break;
            }
            m_StoreTabs[0].AddItem(l_StoreItems[l_Key]);
        }

        m_CurrOpenedTab = m_StoreTabs[0];
        m_CurrOpenedTab.itemsButtonList.isActive = false;
    }

    private void InitTextBox()
    {
        m_TextBox = GetComponentInChildren<TextBox>();
        m_TextBox.Activate(false);
    }

    private void CloseStore()
    {
        m_TextBox.SetTalking(false);
        PlayerInventory.GetInstance().Save();
        Close();
    }

    private void OpenTabs()
    {
        m_Window.SetActive(true);
        m_ButtonList.isActive = false;
        m_TabButtonsList.isActive = true;
    }

    private void CloseTabs()
    {
        m_Window.SetActive(false);
        m_ButtonList.isActive = true;
        m_TabButtonsList.isActive = false;
    }

    private void StartDialog()
    {
        HideButtonList();

        m_TextBox.Activate(true);
        m_TextBox.SetText(DialogDataBase.GetInstance().GetDialog("StoreDialog").subDialogs[0].phrases);
        m_TextBox.ShowText();

        m_TextBox.AddEndAction(StartWelcomeDialog);
    }

    private void StartWelcomeDialog()
    {
        ShowButtonList();

        m_TextBox.Activate(true);
        m_TextBox.SetText(DialogDataBase.GetInstance().GetDialog("StoreWelcome").subDialogs[0].phrases);
        m_TextBox.ShowText();

        m_TextBox.AddEndAction(DisactiveTextBox);
    }

    private void DisactiveTextBox()
    {
        m_TextBox.Activate(false);
        m_TextBox.RemoveEndAction(DisactiveTextBox);
    }
    
    private void HideButtonList()
    {
        m_ButtonList.isActive = false;
        m_ButtonList.gameObject.SetActive(false);
    }

    private void ShowButtonList()
    {
        m_ButtonList.isActive = true;
        m_ButtonList.gameObject.SetActive(true);
    }

    private void ShowTab()
    {
        m_CurrOpenedTab.gameObject.SetActive(false);
        m_StoreTabs[m_TabButtonsList.currentButtonId].gameObject.SetActive(true);
        m_CurrOpenedTab = m_StoreTabs[m_TabButtonsList.currentButtonId];
        //m_CurrOpenedTab.CheckCountInInventory();
    }

    private void ConfirmTab()
    {
        m_CurrOpenedTab.Confirm();
        
        m_TabButtonsList.isActive = false;
        m_TabButtonsList.currentButton.selected = true;
    }
}
