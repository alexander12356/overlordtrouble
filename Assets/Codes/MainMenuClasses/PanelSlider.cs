using UnityEngine;
using UnityEngine.UI;

public class PanelSlider : Slider
{
    private float m_CurrentValue = 0.0f;
    private bool m_IsActive = false;

    private event PanelButtonActionHandler m_CancelAction;

    public float currentValue
    {
        get
        {
            return value;
        }
        set
        {
            m_CurrentValue = this.value = value;
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
            enabled = m_IsActive;
            if (enabled)
            {
                Select();
                DoStateTransition(SelectionState.Highlighted, true);
            }
            else
            {
                InstantClearState();
                DoStateTransition(SelectionState.Normal, true);
            }
        }
    }

    public void UpdateKey()
    {
        if (!enabled || !m_IsActive)
            return;

        if (ControlSystem.ExitButton())
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
