using UnityEngine;
using UnityEngine.SceneManagement;

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

        Cursor.visible = false;

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
            SceneManager.LoadScene("DemoMainScene");
            GameManager.GetInstance().isTesting = true;
            PlayerData.GetInstance().InitTestStats();
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            GameManager.GetInstance().UnloadScene();
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            ChangeLaguage("ru");
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            ChangeLaguage("en");
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

    private void ChangeLaguage(string p_LangId)
    {
        LocalizationDataBase.GetInstance().ChangeLanguage(p_LangId);
        SceneManager.LoadScene("MainMenu");
    }
}
