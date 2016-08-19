using UnityEngine;
using System.Collections;

public class StorePanel : Panel
{
    #region Variables
    private static StorePanel m_Prefab;
    private ButtonList m_ButtonList = null;

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
        
        InitButtonActions();
        InitTabsButtonActions();

        m_TabButtonsList.isActive = false;
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ButtonList.UpdateKey();
        m_TabButtonsList.UpdateKey();
    }

    #endregion

    private void InitButtonActions()
    {
        m_ButtonList = GetComponentInChildren<ButtonList>();

        m_ButtonList[0].AddAction(OpenTabs);
        m_ButtonList[1].AddAction(OpenTabs);
        //m_ButtonList[2].AddAction(StartDialog);
        m_ButtonList[3].AddAction(CloseStore);
    }

    private void InitTabsButtonActions()
    {
        m_TabButtonsList.AddCancelAction(CloseTabs);
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
        DialogPanel l_DialogPanel = Instantiate(DialogPanel.prefab);
        l_DialogPanel.SetDialog(DialogDataBase.GetInstance().GetDialog("Store"));

        PanelManager.GetInstance().ShowPanel(l_DialogPanel);
        l_DialogPanel.myTransform.localPosition = new Vector3();
    }
}
