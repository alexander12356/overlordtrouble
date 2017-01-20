using System.Collections.Generic;

using UnityEngine;

public delegate void KeyArrowActionHandler();
public class ButtonList : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private List<PanelButton> m_ButtonsList = null;

    private int m_CurrentButtonId = 0;
    private int m_PrevButtonId = 0;
    private RectTransform m_RectTransform = null;

    private event KeyArrowActionHandler KeyArrowActioneEvent;
    private event PanelButtonActionHandler m_CancelAction;

    [SerializeField]
    private bool m_IsActive = true;
    #endregion

    #region Interface
    public int currentButtonId
    {
        get { return m_CurrentButtonId; }
    }
    public int prevButtonId
    {
        get { return m_PrevButtonId; }
    }
    public PanelButton this[int i]
    {
        get { return m_ButtonsList[i]; }
        set { m_ButtonsList[i] = value; }
    }
    public PanelButton currentButton
    {
        get { return m_ButtonsList[m_CurrentButtonId]; }
    }
    public bool isActive
    {
        get { return m_IsActive; }
        set
        {
            m_IsActive = value;
            if (m_ButtonsList == null || m_ButtonsList.Count == 0)
            {
                return;
            }
            if (m_IsActive)
            {
                currentButton.selected = true;
            }
            else
            {
                currentButton.selected = false;
            }
        }
    }
    public int count
    {
        get { return m_ButtonsList.Count; }
    }
    public RectTransform rectTransform
    {
        get
        {
            if (m_RectTransform == null)
            {
                m_RectTransform = GetComponent<RectTransform>();
            }
            return m_RectTransform;
        }
    }

    public void AddKeyArrowAction(KeyArrowActionHandler p_Action)
    {
        KeyArrowActioneEvent += p_Action;
    }

    public void RemoveKeyArrowAction(KeyArrowActionHandler p_Action)
    {
        KeyArrowActioneEvent -= p_Action;
    }

    public void AddCancelAction(PanelButtonActionHandler p_Action)
    {
        m_CancelAction += p_Action;
    }

    public void RemoveCancelAction(PanelButtonActionHandler p_Action)
    {
        m_CancelAction -= p_Action;
    }

    public void SelectMoveUp()
    {
        m_PrevButtonId = m_CurrentButtonId;
        m_CurrentButtonId--;

        CheckSelectPosition();
    }

    public void SelectMoveDown()
    {
        m_PrevButtonId = m_CurrentButtonId;
        m_CurrentButtonId++;

        CheckSelectPosition();
    }

    public void AddButton(PanelButton p_Button)
    {
        p_Button.transform.SetParent(transform);
        p_Button.transform.localScale = Vector3.one;
        m_ButtonsList.Add(p_Button);
    }

    public void InsertButton(PanelButton p_Button)
    {
        m_ButtonsList.Add(p_Button);
    }

    public void RemoveButton(int p_Index)
    {
        PanelButton l_DeletedButton = m_ButtonsList[p_Index];
        Destroy(l_DeletedButton.gameObject);
        m_ButtonsList.RemoveAt(p_Index);
        ReconstructButtons();
    }

    public void RemoveButton(PanelButton p_Button)
    {
        m_ButtonsList.Remove(p_Button);
        Destroy(p_Button.gameObject);
        ReconstructButtons();
    }

    public void RemoveButton(string p_Id)
    {
        for (int i = 0; i < m_ButtonsList.Count; i++)
        {
            if ((m_ButtonsList[i]).title == p_Id)
            {
                PanelButton l_PanelButton = m_ButtonsList[i];
                m_ButtonsList.RemoveAt(i);
                Destroy(l_PanelButton.gameObject);
                ReconstructButtons();
                return;
            }
        }        
    }

    public void UpdateKey()
    {
        if (!isActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectMoveUp();
            KeyArrowAction();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectMoveDown();
            KeyArrowAction();
        }

        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.Z))
        {
            ConfirmAction();
            Input.ResetInputAxes();
        }
        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Backspace))
        {
            CancelAction();
            Input.ResetInputAxes();
        }
    }

    public void Clear()
    {
        for (int i = 0; i < m_ButtonsList.Count; i++)
        {
            Destroy(m_ButtonsList[i].gameObject);
        }
        m_ButtonsList.Clear();
        m_CurrentButtonId = 0;
        m_PrevButtonId = 0;
    }
    #endregion

    #region Private
    private void KeyArrowAction()
    {
        if (KeyArrowActioneEvent != null)
        {
            KeyArrowActioneEvent();
        }
    }

    private void Awake()
    {
    }

    private void Start()
    {
        if (isActive)
        {
            CheckSelectPosition();
        }
    }

    private void CheckSelectPosition()
    {
        if (m_ButtonsList.Count == 0)
        {
            return;
        }

        if (m_CurrentButtonId < 0)
        {
            m_CurrentButtonId = m_ButtonsList.Count - 1;
        }
        else if (m_CurrentButtonId >= m_ButtonsList.Count)
        {
            m_CurrentButtonId = 0;
        }

        m_ButtonsList[m_PrevButtonId].selected = false;
        m_ButtonsList[m_CurrentButtonId].selected = true;
    }

    private void ReconstructButtons()
    {
        for (int i = 0; i < m_ButtonsList.Count; i++)
        {
            m_ButtonsList[i].transform.localPosition = new Vector3(220, -70.0f - i * 50, 0.0f);
            m_ButtonsList[i].transform.localScale = Vector3.one;
        }
    }

    private void ConfirmAction()
    {
        if (m_ButtonsList.Count > 0)
        {
            m_ButtonsList[m_CurrentButtonId].RunAction();
        }
    }

    private void CancelAction()
    {
        if (m_CancelAction != null)
        {
            m_CancelAction();
        }
    }
    #endregion
}
