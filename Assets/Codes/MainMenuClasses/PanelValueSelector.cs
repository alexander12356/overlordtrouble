using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelValueSelector : MonoBehaviour
{
    public static PanelValueSelector m_Prefab = null;
    private Dictionary<int, string> m_Values = new Dictionary<int, string>();
    private int m_CurrentIndex;
    private bool m_IsActive;
    private bool m_Selected;
    private Text m_TitleText;

    [SerializeField]
    private string m_Title = string.Empty;
    [SerializeField]
    private Color32 m_SelectedColor = new Color32(227, 220, 69, 255);

    private event PanelButtonActionHandler m_CancelAction;

    #region PROPERTIES

    public static PanelValueSelector prefab
    {
        get
        {
            if(m_Prefab == null)
            {
                m_Prefab = Resources.Load<PanelValueSelector>("Prefabs/Button/PanelValueSelector");
            }
            return m_Prefab;
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
            selected = value;
        }
    }

    public Dictionary<int, string> values
    {
        get
        {
            return m_Values;
        }
        set
        {
            m_Values = value;
            title = m_Values[currentIndex];
        }
    }

    public int currentIndex
    {
        get
        {
            return m_CurrentIndex;
        }
        set
        {
            m_CurrentIndex = value;
            CheckSelectPosition();
        }
    }

    public string currentValue
    {
        get
        {
            return m_Values[m_CurrentIndex];
        }
    }

    public string title
    {
        get
        {
            return m_Title;
        }
        set
        {
            if(m_TitleText == null)
            {
                m_TitleText = GetComponentInChildren<Text>();
            }
            m_TitleText.text = m_Title = value;
        }
    }

    public bool selected
    {
        get
        {
            return m_Selected;
        }
        set
        {
            Select(value);
        }
    }

    private void Select(bool value)
    {
        m_Selected = value;
        if (value)
            m_TitleText.color = m_SelectedColor;
        else
            m_TitleText.color = Color.white;
    }

    #endregion

    public void UpdateKey()
    {
        if (!enabled || !m_IsActive)
            return;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectMoveBack();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectMoveForward();
        }

        if (ControlSystem.ExitButton())
        {
            CancelAction();
        }
    }

    private void SelectMoveForward()
    {
        m_CurrentIndex++;
        CheckSelectPosition();
    }

    private void SelectMoveBack()
    {
        m_CurrentIndex--;
        CheckSelectPosition();
    }

    private void CheckSelectPosition()
    {
        if (m_Values.Count == 0)
            return;
        if (m_CurrentIndex < 0)
            m_CurrentIndex = 0;
        else if (m_CurrentIndex >= (m_Values.Count - 1))
            m_CurrentIndex = m_Values.Count - 1;
        title = m_Values[m_CurrentIndex];
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
