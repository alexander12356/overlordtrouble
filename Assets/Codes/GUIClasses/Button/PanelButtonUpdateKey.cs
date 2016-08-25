using UnityEngine;
using System.Collections;

public class PanelButtonUpdateKey : PanelButton
{
    protected bool m_Active = false;
    protected event PanelActionHandler m_CancelAction  = null;

    public bool isActive
    {
        get { return m_Active; }
    }

    public virtual void Activate(bool p_Value)
    {
        m_Active = p_Value;
    }

    public virtual void UpdateKey()
    {
    }

    public virtual void AddCancelAction(PanelActionHandler p_Action)
    {
        m_CancelAction += p_Action;
    }

    public virtual void RemoveCancelAction(PanelActionHandler p_Action)
    {
        m_CancelAction -= p_Action;
    }

    public void CancelAction()
    {
        if (m_CancelAction != null)
        {
            m_CancelAction();
        }
    }
}
