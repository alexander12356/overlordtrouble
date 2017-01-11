using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNpcBehavior : BaseCollideBehavior
{
    protected string m_State = string.Empty;

    public string state
    {
        get { return m_State;  }
        set { m_State = value; }
    }
}
