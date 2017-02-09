using UnityEngine;
using UnityEngine.UI;

public class PanelDropdown : Dropdown
{
    private int m_CurrentValue = 0;
    private bool m_IsActive = false;

    private event PanelButtonActionHandler m_CancelAction;

    public int currentValue
    {
        get { return m_CurrentValue; }
        set
        {
            m_CurrentValue = value;
            this.value = value;
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

    private void SelectMoveUp()
    {
        currentValue--;
        CheckSelectPosition();
    }

    private void SelectMoveDown()
    {
        currentValue++;
        CheckSelectPosition();
    }

    private void CheckSelectPosition()
    {
        if (options.Count == 0)
        {
            return;
        }

        if (currentValue < 0)
        {
            currentValue = 0;
        }
        else if (currentValue >= options.Count)
        {
            currentValue = options.Count - 1;
        }
    }

    public void UpdateKey()
    {
        if (!enabled || !m_IsActive)
            return; 
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectMoveUp();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectMoveDown();
        }

        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Return))
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