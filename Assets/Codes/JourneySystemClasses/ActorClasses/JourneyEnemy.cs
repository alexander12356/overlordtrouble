using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JourneyEnemy : JourneyNPC
{
    [SerializeField]
    private ActionStruct m_WinAction;

    [SerializeField]
    private ActionStruct m_LoseAction;

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
            Destroy(gameObject);
        }
    }
}
