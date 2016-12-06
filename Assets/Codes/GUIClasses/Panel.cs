using UnityEngine;

using System.Collections;

public delegate void PanelActionHandler();
public delegate void PanelActionHandlerWithParameter(int p_Value);
public class Panel : MonoBehaviour
{
    #region Variables
    private event PanelActionHandler m_PopAction = null;
    private event PanelActionHandler m_PushAction = null;
    private Transform m_Transform;
    private bool m_Close = false;
    private bool m_IsShowed = false;
    private BaseTransition m_BaseTransition = null;
    private PanelManager m_PanelManager = null;
    private string m_Id = "";
    #endregion

    #region Interface
    public bool isShowed
    {
        get { return m_IsShowed; }
    }
    public bool moving
    {
        get
        {
            if (m_BaseTransition == null)
            {
                m_BaseTransition = GetComponent<BaseTransition>();
            }
            return m_BaseTransition.isMoving;
        }
    }
    public Transform myTransform
    {
        get
        {
            if (m_Transform == null)
            {
                m_Transform = transform;
            }
            return m_Transform;
        }

        set
        {
            m_Transform = value;
        }
    }
    public string id
    {
        set { m_Id = value; }
    }

    public virtual void Awake()
    {
        m_Transform = transform;
        m_BaseTransition = GetComponent<BaseTransition>();

        m_BaseTransition.AddEndShowAction(EndShowing);
        m_BaseTransition.AddEndHideAction(EndHiding);
    }

    public void AddPopAction(PanelActionHandler p_Action)
    {
        m_PopAction += p_Action;
    }

    public void RemovePopAction(PanelActionHandler p_Action)
    {
        m_PopAction -= p_Action;
    }

    public virtual void PopAction()
    {
        if (m_PopAction != null)
        {
            m_PopAction();
        }
    }

    public void AddPushAction(PanelActionHandler p_Action)
    {
        m_PushAction += p_Action;
    }

    public void RemovePushAction(PanelActionHandler p_Action)
    {
        m_PushAction -= p_Action;
    }

    public virtual void PushAction()
    {
        if (m_PushAction != null)
        {
            m_PushAction();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        m_BaseTransition.Show();
    }

    public void Hide()
    {
        m_BaseTransition.Hide();
    }

    public virtual void Close()
    {
        m_Close = true;
        Hide();
    }

    public virtual void UpdatePanel()
    {
        
    }

    public void SetPanelManager(PanelManager p_PanelManager)
    {
        m_PanelManager = p_PanelManager;
    }
    #endregion

    #region Private
    private void EndShowing()
    {
        m_IsShowed = true;
        PushAction();
    }

    private void EndHiding()
    {
        m_IsShowed = false;
        gameObject.SetActive(false);
        if (m_Close)
        {
            m_PanelManager.ClosePanel();
            PopAction();
            Destroy(gameObject);
        }
    }
    #endregion
}