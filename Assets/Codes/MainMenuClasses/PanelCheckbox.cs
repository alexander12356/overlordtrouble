using UnityEngine;
using UnityEngine.UI;

public class PanelCheckbox : Toggle
{
    private bool m_CurrentValue = false;
    private bool m_IsActive = false;

    private event PanelButtonActionHandler m_CancelAction;

    public bool currentValue
    {
        get { return isOn; }
        set
        {
            m_CurrentValue = isOn = value;
        }
    }

    public bool isActive
    {
        get
        {
            return m_IsActive;
        }
        set
        {
            m_IsActive = value;
        }
    }

    private void Toggle()
    {
        m_CurrentValue = !currentValue;
        isOn = m_CurrentValue;
    }

    public void UpdateKey()
    {
        if (!enabled || !m_IsActive)
            return;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Toggle();
        }

        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Backspace))
        {
            CancelAction();
        }
    }

    public void AddCancelAction(PanelButtonActionHandler p_Action)
    {
        m_CancelAction += p_Action;
    }

    public void RemoveCancelAction(PanelButtonActionHandler p_Action)
    {
        m_CancelAction -= p_Action;
    }

    private void CancelAction()
    {
        if (m_CancelAction != null)
        {
            m_CancelAction();
        }
    }
}
