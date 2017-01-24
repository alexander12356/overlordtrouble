using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : Panel
{
    #region Variables

    private static InventoryPanel m_Prefab;
    [SerializeField]
    private ButtonList m_ViewButtonsList = null;

    private InventoryView m_CurrOpenedView = null;

    #endregion

    #region Properties
    public static InventoryPanel prefab
    {
        get
        {
            if(m_Prefab == null)
            {
                m_Prefab = Resources.Load<InventoryPanel>("Prefabs/Panels/InventoryPanel");
            }
            return m_Prefab;
        }
    }

    public ButtonList tabButtonList
    {
        get
        {
            return m_ViewButtonsList;
        }
    }

    #endregion

    #region Methods

    public override void Awake()
    {
        base.Awake();

        InitViewButtonList();
        ShowView();
    }

    private void InitViewButtonList()
    {
        m_ViewButtonsList.AddKeyArrowAction(ShowView);
        m_ViewButtonsList[0].AddAction(ConfirmView);
        m_ViewButtonsList[1].AddAction(ConfirmView);
        m_ViewButtonsList[2].AddAction(ConfirmView);
        m_ViewButtonsList[3].AddAction(ConfirmView);
        m_ViewButtonsList[4].AddAction(ConfirmView);
        m_ViewButtonsList[5].AddAction(ConfirmView);
        m_ViewButtonsList[6].AddAction(ConfirmView);
        m_ViewButtonsList[7].AddAction(CloseInventory);

        InventoryViewButton l_ViewButton;
        l_ViewButton = (InventoryViewButton) m_ViewButtonsList[0];
        l_ViewButton.inventoryView = new InventoryEquipmentView(this);

        l_ViewButton = (InventoryViewButton)m_ViewButtonsList[1];
        l_ViewButton.inventoryView = new InventoryItemsView(this, new AllGetter());

        l_ViewButton = (InventoryViewButton)m_ViewButtonsList[2];
        l_ViewButton.inventoryView = new InventoryItemsView(this, new WepsGetter());

        l_ViewButton = (InventoryViewButton)m_ViewButtonsList[3];
        l_ViewButton.inventoryView = new InventoryItemsView(this, new BlingGetter());

        l_ViewButton = (InventoryViewButton)m_ViewButtonsList[4];
        l_ViewButton.inventoryView = new InventoryItemsView(this, new SingleUseGetter());

        l_ViewButton = (InventoryViewButton)m_ViewButtonsList[5];
        l_ViewButton.inventoryView = new InventoryItemsView(this, new MultiUseGetter());

        l_ViewButton = (InventoryViewButton)m_ViewButtonsList[6];
        l_ViewButton.inventoryView = new InventoryItemsView(this, new KeyItemGetter());

        l_ViewButton = (InventoryViewButton)m_ViewButtonsList[7];
        l_ViewButton.inventoryView = new InventoryItemsView(this, null);

        InventoryStandingView l_StandingView = new InventoryStandingView(this);
        l_StandingView.Init();

        m_ViewButtonsList.SelectMoveDown();
        m_ViewButtonsList.isActive = true;

        m_CurrOpenedView = new NullInventoryView();
    }

    private void ConfirmView()
    {
        if (m_CurrOpenedView.Confrim())
        {
            m_ViewButtonsList.isActive = false;
            m_ViewButtonsList.currentButton.selected = true;
        }
    }

    public void ShowView()
    {
        m_CurrOpenedView.Disable();
        m_CurrOpenedView = GetCurrentView();
        m_CurrOpenedView.Init();
    }

    private InventoryView GetCurrentView()
    {
        return (m_ViewButtonsList[m_ViewButtonsList.currentButtonId] as InventoryViewButton).inventoryView;
    }

    private void CloseInventory()
    {
        Close();
        JourneySystem.GetInstance().SetControl(ControlType.Player);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ViewButtonsList.UpdateKey();
        m_CurrOpenedView.UpdateKey();
    }

    #endregion
}
