using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButton : PanelButton
{
    private static PauseMenuButton m_Prefab = null;
    private Animator m_Animator = null;

    public static PauseMenuButton prefab
    {
        get
        {
            if (m_Prefab = null)
            {
                m_Prefab = Resources.Load<PauseMenuButton>("Prefabs/Button/PauseMenuButton");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_Animator = GetComponent<Animator>();
    }

    public override void RunAction()
    {
        base.RunAction();
        m_Animator.SetTrigger("Click");
    }
}
