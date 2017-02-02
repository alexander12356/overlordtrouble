using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWarpBehavior : MonoBehaviour
{
    [SerializeField]
    private Warp m_Warp = null;

    [SerializeField]
    private string m_BehaviorId = string.Empty;

    public void Run()
    {
        m_Warp.ChangeBehavior(m_BehaviorId);
    }
}
