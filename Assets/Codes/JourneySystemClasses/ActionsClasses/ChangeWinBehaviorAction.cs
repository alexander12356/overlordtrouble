using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWinBehaviorAction : MonoBehaviour
{
    [SerializeField]
    private JourneyEnemy m_JourneyEnemy;

    [SerializeField]
    private string m_WinBehaviorId;

    public void Run()
    {
        m_JourneyEnemy.ChangeWinBehavior(m_WinBehaviorId);
    }
}
