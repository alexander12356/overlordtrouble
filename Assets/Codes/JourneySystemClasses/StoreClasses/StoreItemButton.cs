using UnityEngine;
using UnityEngine.UI;

public class StoreItemButton : PanelButton
{
    #region Variables
    private bool m_Active = false;
    private int m_CountToBuy = 0;
    private int m_ItemCost = 0;
    private static StoreItemButton   m_Prefab = null;
    private event PanelButtonActionHandler m_CancelAction = null;
    private event PanelActionHandler       m_BuyAction = null;
    private const int m_MaxValue = 999;
    private const int m_MinValue = 0;
    private string m_ItemId = string.Empty;
    private Animator m_Animator = null;

    [SerializeField]
    private GameObject m_BuyArrows = null;

    [SerializeField]
    private Text m_CountToBuyText = null;

    [SerializeField]
    private Text m_ItemCostText = null;
    #endregion

    #region Interface
    //TODO fix
    public static StoreItemButton prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<StoreItemButton>("Prefabs/Button/StoreItemButton");
            }
            return m_Prefab;
        }
    }
    public int countToBuy
    {
        get { return m_CountToBuy;  }
        set
        {
            m_CountToBuy = Mathf.Clamp(value, m_MinValue, m_MaxValue);
            m_CountToBuyText.text = m_CountToBuy.ToString();
        }
    }
    public int itemCost
    {
        get { return m_ItemCost; }
        set
        {
            m_ItemCost = value;
            m_ItemCostText.text = m_ItemCost.ToString();
        }
    }
    public bool isActive
    {
        get { return m_Active; }
    }
    public string itemId
    {
        get { return m_ItemId; }
        set
        {
            m_ItemId = value;
            itemCost = StoreDataBase.GetInstance().GetItem(m_ItemId).cost;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_Animator = GetComponent<Animator>();
    }

    public void UpdateKey()
    {
        if (!m_Active)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            countToBuy += 1;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            countToBuy -= 1;
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            countToBuy += 10;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            countToBuy -= 10;
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            if (PlayerInventory.GetInstance().coins > m_CountToBuy * m_ItemCost && m_CountToBuy > 0)
            {
                YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
                l_YesNoPanel.SetText("Вы действительно хотите купить " + title + "?");
                l_YesNoPanel.AddYesAction(m_BuyAction);
                l_YesNoPanel.AddYesAction(l_YesNoPanel.Cancel);

                PanelManager.GetInstance().ShowPanel(l_YesNoPanel, true);
            }
            else
            {
                m_Animator.SetTrigger("Lack");
            }
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            CancelBuy();
        }
    }

    public void Activate(bool p_Value)
    {
        m_Active = p_Value;
        m_BuyArrows.SetActive(m_Active);
        countToBuy = 0;
    }

    public void AddCancelAction(PanelButtonActionHandler p_Action)
    {
        m_CancelAction += p_Action;
    }

    public void RemoveCancelAction(PanelButtonActionHandler p_Action)
    {
        m_CancelAction -= p_Action;
    }

    public void AddBuyAction(PanelActionHandler p_Action)
    {
        m_BuyAction += p_Action;
    }

    public void RemoveBuyAction(PanelActionHandler p_Action)
    {
        m_BuyAction -= p_Action;
    }
    #endregion

    #region Private
    private void CancelBuy()
    {
        CancelAction();
    }

    private void CancelAction()
    {
        if (m_CancelAction != null)
        {
            m_CancelAction();
        }
    }
    #endregion
}
