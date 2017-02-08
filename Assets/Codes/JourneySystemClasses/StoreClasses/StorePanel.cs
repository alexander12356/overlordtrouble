using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using System;

public class StorePanel : Panel
{
    #region Variables
    private static StorePanel m_Prefab;
    private ButtonList m_ButtonList    = null;
    private TextBox    m_TextBox       = null;
    private StoreTab   m_CurrOpenedTab = null;
    private StoreItemButton m_CurrentSelectedItem = null;
    private List<StoreTab> m_StoreTabs = null;

    [SerializeField]
    private ButtonList m_TabButtonsList = null;

    [SerializeField]
    private GameObject m_Window = null;

    [SerializeField]
    private Text m_PlayerCoinsText = null;
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
        InitTabButtonList();
        m_CurrOpenedTab = new NullStoreTab();

        HideButtonList();
        m_TabButtonsList.isActive = false;
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ButtonList.UpdateKey();
        m_TabButtonsList.UpdateKey();
        m_CurrOpenedTab.UpdateKey();

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
        m_TextBox.SetTalkingAnimator(p_TalkingAnimator, "Talking");
    }
    #endregion

    private void InitButtonActions()
    {
        m_ButtonList = GetComponentInChildren<ButtonList>();

        m_ButtonList[0].AddAction(InitBuyTabs);
        m_ButtonList[0].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Buy");
        m_ButtonList[1].AddAction(InitCellTabs);
        m_ButtonList[1].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Sell");
        m_ButtonList[2].AddAction(StartDialog);
        m_ButtonList[2].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Talk");
        m_ButtonList[3].AddAction(CloseStore);
        m_ButtonList[3].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Store:Leave");
    }

    private void InitTabButtonList()
    {
        m_TabButtonsList.AddCancelAction(CloseTabs);
        m_TabButtonsList.AddKeyArrowAction(ShowTab);
        m_TabButtonsList[0].AddAction(ConfirmTab);
        m_TabButtonsList[1].AddAction(ConfirmTab);
        m_TabButtonsList[2].AddAction(ConfirmTab);
        m_TabButtonsList[3].AddAction(ConfirmTab);
        m_TabButtonsList[4].AddAction(ConfirmTab);
    }

    private void InitBuyTabs()
    {
        StoreTabBuy l_TabBuy = null;

        if (m_StoreTabs != null)
            m_StoreTabs.Clear();
        m_StoreTabs = new List<StoreTab>();

        l_TabBuy = new StoreTabBuy(this, new AllGetter());
        m_StoreTabs.Add(l_TabBuy);

        l_TabBuy = new StoreTabBuy(this, new WepsGetter());
        m_StoreTabs.Add(l_TabBuy);

        l_TabBuy = new StoreTabBuy(this, new BlingGetter());
        m_StoreTabs.Add(l_TabBuy);

        l_TabBuy = new StoreTabBuy(this, new SingleUseGetter());
        m_StoreTabs.Add(l_TabBuy);

        l_TabBuy = new StoreTabBuy(this, new MultiUseGetter());
        m_StoreTabs.Add(l_TabBuy);

        m_CurrOpenedTab = m_StoreTabs[0];
        m_CurrOpenedTab.Init();
        OpenTabs();
    }

    private void InitCellTabs()
    {
        StoreTabCell l_TabBuy = null;
        if (m_StoreTabs != null)
            m_StoreTabs.Clear();
        m_StoreTabs = new List<StoreTab>();

        l_TabBuy = new StoreTabCell(this, new AllGetter());
        m_StoreTabs.Add(l_TabBuy);

        l_TabBuy = new StoreTabCell(this, new WepsGetter());
        m_StoreTabs.Add(l_TabBuy);

        l_TabBuy = new StoreTabCell(this, new BlingGetter());
        m_StoreTabs.Add(l_TabBuy);

        l_TabBuy = new StoreTabCell(this, new SingleUseGetter());
        m_StoreTabs.Add(l_TabBuy);

        l_TabBuy = new StoreTabCell(this, new MultiUseGetter());
        m_StoreTabs.Add(l_TabBuy);

        m_CurrOpenedTab = m_StoreTabs[0];
        m_CurrOpenedTab.Init();
        OpenTabs();
    }

    private void InitTextBox()
    {
        m_TextBox = GetComponentInChildren<TextBox>();
        m_TextBox.Activate(false);
    }

    private void CloseStore()
    {
        m_TextBox.SetTalking(false);
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
        m_CurrOpenedTab.Disable();
    }

    private void StartDialog()
    {
        HideButtonList();

        m_TextBox.Activate(true);

        List<string> l_Text = new List<string>();
        l_Text.Add(LocalizationDataBase.GetInstance().GetText("Dialog:Store:1"));
        l_Text.Add(LocalizationDataBase.GetInstance().GetText("Dialog:Store:2"));

        m_TextBox.SetText(l_Text);
        m_TextBox.ShowText();

        m_TextBox.AddEndAction(StartWelcomeDialog);
    }

    private void StartWelcomeDialog()
    {
        ShowButtonList();

        m_TextBox.Activate(true);
        List<string> l_Text = new List<string>() { LocalizationDataBase.GetInstance().GetText("Dialog:Store:Welcome") };
        m_TextBox.SetText(l_Text);
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
        m_CurrOpenedTab.Disable();
        m_CurrOpenedTab = GetCurrentTab();
        m_CurrOpenedTab.Init();
    }

    private StoreTab GetCurrentTab()
    {
        return m_StoreTabs[m_TabButtonsList.currentButtonId];
    }

    private void ConfirmTab()
    {
        if (m_CurrOpenedTab.Confirm())
        {
            m_TabButtonsList.isActive = false;
            m_TabButtonsList.currentButton.selected = true;
        }
    }
}
