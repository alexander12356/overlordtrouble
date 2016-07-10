using System.Collections.Generic;

using UnityEngine;

public delegate void KeyArrowActionHandler();
public class ButtonList : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private List<PanelButton> m_ButtonsList;

    private int m_CurrentButtonId = 0;
    private int m_PrevButtonId = 0;

    private event KeyArrowActionHandler KeyArrowActioneEvent;
    #endregion

    #region Interface
    public void AddKeyArrowAction(KeyArrowActionHandler p_Action)
    {
        KeyArrowActioneEvent += p_Action;
    }

    public void RemoveKeyArrowAction(KeyArrowActionHandler p_Action)
    {
        KeyArrowActioneEvent -= p_Action;
    }

    public int currentButtonId
    {
        get { return m_CurrentButtonId; }
    }

    public int prevButtonId
    {
        get { return m_PrevButtonId; }
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

    public void Action()
    {
        m_ButtonsList[m_CurrentButtonId].RunAction();
    }

    public PanelButton this[int i]
    {
        get { return m_ButtonsList[i];  }
        set { m_ButtonsList[i] = value; }
    }

    public void AddButton(PanelButton p_Button)
    {
        p_Button.transform.SetParent(transform);
        p_Button.transform.localPosition = new Vector3(-500, 130.0f - m_ButtonsList.Count * 50, 0.0f);
        p_Button.transform.localScale = Vector3.one;
        m_ButtonsList.Add(p_Button);
    }

    public void UpdateKey()
    {
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

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z))
        {
            Action();
        }
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
        CheckSelectPosition();
    }

    private void CheckSelectPosition()
    {
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
    #endregion
}
