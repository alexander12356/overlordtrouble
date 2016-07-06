using UnityEngine;

public delegate void PanelPopActionHandler();
public class Panel : MonoBehaviour
{
    #region Variables
    private event PanelButtonActionHandler m_Action = null;
    #endregion

    #region Interface
    public void AddPopAction(PanelButtonActionHandler p_Action)
    {
        m_Action += p_Action;
    }

    public void RemovePopAction(PanelButtonActionHandler p_Action)
    {
        m_Action -= p_Action;
    }

    public void PopAction()
    {
        if (m_Action != null)
        {
            m_Action();
        }
    }
    #endregion

    #region Private
    #endregion
}