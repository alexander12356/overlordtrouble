using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JourneyEnemy : JourneyNPC
{
    [SerializeField]
    private ActionStruct m_WinAction;

    [SerializeField]
    private ActionStruct m_LoseAction;

    public ActionStruct winAction
    {
        get { return m_WinAction; }
        set { m_WinAction = value; }
    }
    public ActionStruct loseAction
    {
        get { return m_LoseAction; }
        set { m_LoseAction = value; }
    }

    public void Win()
    {
        if (m_WinAction.id != "")
        {
            m_WinAction.actionEvent.Invoke();
        }
    }

    public void Lose()
    {
        if (m_LoseAction.id != "")
        {
            m_LoseAction.actionEvent.Invoke();
        }
        else
        {
            Die();
        }
    }
}
