using UnityEngine;
using System.Collections;

public class MainMenuPanel : Panel
{
    private static MainMenuPanel m_Prefab;
    private ButtonList m_ButtonList = null;
    
    public static MainMenuPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<MainMenuPanel>("Prefabs/Panels/MainMenuPanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_ButtonList = GetComponentInChildren<ButtonList>();
        m_ButtonList[0].AddAction(RunNewGame);
        m_ButtonList[1].AddAction(QuitGame);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ButtonList.UpdateKey();

        if (Input.GetKeyUp(KeyCode.F12))
        {
            MainMenuSystem.GetInstance().StartLocation("DemoMainScene");
        }
    }

    private void RunNewGame()
    {
        MainMenuSystem.GetInstance().StartLocation("Town");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
