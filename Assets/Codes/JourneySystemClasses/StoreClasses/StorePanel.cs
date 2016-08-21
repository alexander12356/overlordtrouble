using UnityEngine;
using System.Collections;

public class StorePanel : Panel
{
    #region Variables
    private static StorePanel m_Prefab;
    private ButtonList m_ButtonList = null;
    private TextBox    m_TextBox = null;

    [SerializeField]
    private ButtonList m_TabButtonsList = null;

    [SerializeField]
    private GameObject m_Window = null;
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

    public override void Awake()
    {
        base.Awake();

        InitTextBox();
        InitButtonActions();
        InitTabsButtonActions();

        HideButtonList();
        m_TabButtonsList.isActive = false;
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ButtonList.UpdateKey();
        m_TabButtonsList.UpdateKey();
        m_TextBox.UpdateTextBox();
    }

    public override void PushAction()
    {
        base.PushAction();

        StartWelcomeDialog();
    }
    #endregion

    private void InitButtonActions()
    {
        m_ButtonList = GetComponentInChildren<ButtonList>();

        m_ButtonList[0].AddAction(OpenTabs);
        m_ButtonList[1].AddAction(OpenTabs);
        m_ButtonList[2].AddAction(StartDialog);
        m_ButtonList[3].AddAction(CloseStore);
    }

    private void InitTabsButtonActions()
    {
        m_TabButtonsList.AddCancelAction(CloseTabs);
    }

    private void InitTextBox()
    {
        m_TextBox = GetComponentInChildren<TextBox>();
        m_TextBox.Activate(false);
    }

    private void CloseStore()
    {
        PanelManager.GetInstance().ClosePanel(this);
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
        m_TextBox.SetText(DialogDataBase.GetInstance().GetDialog("StoreDialog").phrases);
        m_TextBox.ShowText();

        m_TextBox.AddEndAction(StartWelcomeDialog);
    }

    private void StartWelcomeDialog()
    {
        ShowButtonList();

        m_TextBox.Activate(true);
        m_TextBox.SetText(DialogDataBase.GetInstance().GetDialog("StoreWelcome").phrases);
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
}
