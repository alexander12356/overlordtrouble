using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ButtonList : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private List<PanelButton> m_ButtonsList;

    private int m_CurrentButtonId = 0;
    private int m_PrevButtonId = 0;
    #endregion

    #region Interface
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

    }
    #endregion

    #region Private

    private void Awake()
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

    private void ButtonActionInit()
    {

    }
    #endregion
}
