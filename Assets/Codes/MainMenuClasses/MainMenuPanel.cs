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
        m_ButtonList[0].title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:NewGame");
        m_ButtonList[1].AddAction(QuitGame);
        m_ButtonList[1].title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:Exit");
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ButtonList.UpdateKey();

        if (Input.GetKeyUp(KeyCode.F12))
        {
            MainMenuSystem.GetInstance().StartLocation("DemoMainScene");
            GameManager.GetInstance().isTesting = true;
            PlayerData.GetInstance().InitTestStats();
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            GameManager.GetInstance().UnloadScene();
        }
    }

    private void RunNewGame()
    {
        PlayerData.GetInstance().ResetData();
        MainMenuSystem.GetInstance().StartLocation("Town");
        GameManager.GetInstance().isTesting = false;
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
