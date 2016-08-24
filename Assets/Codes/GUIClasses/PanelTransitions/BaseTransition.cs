using UnityEngine;
using System.Collections;

public class BaseTransition : MonoBehaviour
{
    private event PanelActionHandler m_EndShowAction;
    private event PanelActionHandler m_EndHideAction;

    protected Transform m_PanelTransform;
    protected bool m_IsMoving = false;
    
    public bool isMoving
    {
        get { return m_IsMoving; }
    }

    public virtual void Awake()
    {
        m_PanelTransform = transform;
    }

    public virtual void Show()
    {
        EndShowing();
    }

    public virtual void Hide()
    {
        EndHiding();
    }

    public void AddEndShowAction(PanelActionHandler p_Action)
    {
        m_EndShowAction += p_Action;
    }

    public void AddEndHideAction(PanelActionHandler p_Action)
    {
        m_EndHideAction += p_Action;
    }

    public virtual void EndShowing()
    {
        if (m_EndShowAction != null)
        {
            m_EndShowAction();
        }
    }

    public virtual void EndHiding()
    {
        if (m_EndHideAction != null)
        {
            m_EndHideAction();
        }
    }
}
