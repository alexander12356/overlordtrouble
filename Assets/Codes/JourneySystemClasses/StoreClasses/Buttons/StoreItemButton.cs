using UnityEngine;
using UnityEngine.UI;

public abstract class StoreItemButton : PanelButtonUpdateKey
{
    #region Variables
    private int m_CountToAction = 0;
    private int m_ItemCost = 0;
    private event PanelActionHandler m_Action = null;
    private const int m_MaxValue = 999;
    private const int m_MinValue = 0;
    private Animator m_Animator = null;

    [SerializeField]
    private GameObject m_Arrows = null;

    [SerializeField]
    private Text m_CountToBuyText = null;

    [SerializeField]
    private Text m_ItemCostText = null;
    #endregion

    #region Interface
    //TODO fix
    public int countToAction
    {
        get { return m_CountToAction;  }
        set
        {
            m_CountToAction = Mathf.Clamp(value, m_MinValue, m_MaxValue);
            m_CountToBuyText.text = m_CountToAction.ToString();
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

    public PanelActionHandler action
    {
        get
        {
            return m_Action;
        }
    }

    public Animator animator
    {
        get
        {
            return m_Animator;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_Animator = GetComponent<Animator>();
    }

    public override void UpdateKey()
    {
        if (!m_Active)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            countToAction += 1;
            m_Animator.SetTrigger("RightArrow");
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            countToAction -= 1;
            m_Animator.SetTrigger("LeftArrow");
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            countToAction += 10;
            m_Animator.SetTrigger("UpArrow");
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            countToAction -= 10;
            m_Animator.SetTrigger("DownArrow");
        }
        else if (ControlSystem.EnterButton())
        {
            StoreItemButtonAction();
        }
        else if (ControlSystem.ExitButton())
        {
            Cancel();
        }
    }

    public abstract void StoreItemButtonAction();

    public override void Activate(bool p_Value)
    {
        base.Activate(p_Value);
        m_Arrows.SetActive(m_Active);
        countToAction = 0;
    }

    public void AddAction(PanelActionHandler p_Action)
    {
        m_Action += p_Action;
    }

    public void RemoveAction(PanelActionHandler p_Action)
    {
        m_Action -= p_Action;
    }
    #endregion

    #region Private
    private void Cancel()
    {
        CancelAction();
    }
    #endregion
}
