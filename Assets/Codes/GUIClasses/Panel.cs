using UnityEngine;

using System.Collections;

public delegate void PanelActionHandler();
public class Panel : MonoBehaviour
{
    #region Variables
    private event PanelActionHandler m_PopAction = null;
    private event PanelActionHandler m_PushAction = null;
    private Transform m_Transform;
    private bool m_Close = false;
    private bool m_IsShowed = false;
    private BaseTransition m_BaseTransition = null;
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
        //StartCoroutine(Showing());
        m_BaseTransition.Show();
    }

    public void Hide()
    {
        m_BaseTransition.Hide();
        //StartCoroutine(Hiding());
    }

    public void Close()
    {
        Hide();
        m_Close = true;
    }

    public virtual void UpdatePanel()
    {

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
            PopAction();
            Destroy(gameObject);
        }
    }
    #endregion
}