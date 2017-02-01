using System.Collections.Generic;

using UnityEngine;

public delegate void KeyArrowActionHandler();
public class ButtonList : MonoBehaviour
{
    private enum SlideDirection
    {
        Horizontal,
        Vertical
    }    

    #region Variables
    [SerializeField]
    private List<PanelButton> m_ButtonsList = null;

    private int m_CurrentButtonId = 0;
    private int m_PrevButtonId = 0;
    private RectTransform m_RectTransform = null;

    private event KeyArrowActionHandler KeyArrowActioneEvent;
    private event PanelButtonActionHandler m_CancelAction;
    private event PanelButtonActionHandler m_UpOutwardAction;
    private event PanelButtonActionHandler m_DownOutwardAction;

    [SerializeField]
    private bool m_IsActive = true;

    [SerializeField]
    private SlideDirection m_SliderDirection = SlideDirection.Vertical;
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
                currentButton.choosed = false;
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

    public void AddUpOutwardAction(PanelButtonActionHandler p_Action)
    {
        m_UpOutwardAction += p_Action;
    }

    public void AddDownOutwardAction(PanelButtonActionHandler p_Action)
    {
        m_DownOutwardAction += p_Action;
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

        switch (m_SliderDirection)
        {
            case SlideDirection.Vertical:
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
                break;
            case SlideDirection.Horizontal:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    SelectMoveUp();
                    KeyArrowAction();
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    SelectMoveDown();
                    KeyArrowAction();
                }
                break;
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

    public void ClearEvents()
    {
        KeyArrowActioneEvent = null;
        m_CancelAction = null;
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
            UpOutwardAction();
            
        }
        else if (m_CurrentButtonId >= m_ButtonsList.Count)
        {
            m_CurrentButtonId = 0;
            DownOutwardAction();
        }

        m_ButtonsList[m_PrevButtonId].selected = false;
        m_ButtonsList[m_CurrentButtonId].selected = true;
        m_ButtonsList[m_PrevButtonId].choosed = false;
    }

    private void ReconstructButtons()
    {
        for (int i = 0; i < m_ButtonsList.Count; i++)
        {
            m_ButtonsList[i].transform.localPosition = new Vector3(220, -70.0f - i * 50, 0.0f);
            m_ButtonsList[i].transform.localScale = Vector3.one;
        }
        m_PrevButtonId = m_CurrentButtonId = 0;
        CheckSelectPosition();
    }

    private void ConfirmAction()
    {
        if (m_ButtonsList.Count > 0)
        {
            m_ButtonsList[m_CurrentButtonId].choosed = true;
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

    private void UpOutwardAction()
    {
        if (m_UpOutwardAction != null)
        {
            m_UpOutwardAction();
        }
    }

    private void DownOutwardAction()
    {
        if (m_DownOutwardAction != null)
        {
            m_DownOutwardAction();
        }
    }
    #endregion
}
