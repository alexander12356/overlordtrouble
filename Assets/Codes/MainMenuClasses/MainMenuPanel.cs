using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPanel : Panel
{
    private ButtonList m_MainMenuButtonList = null;

    public override void Awake()
    {
        base.Awake();

        Cursor.visible = false;

        MainMenuSystem.GetInstance().ShowPanel(this);

        InitButtonList();
    }

    private void InitButtonList()
    {
        m_MainMenuButtonList = transform.FindChild("MainMenu").FindChild("MainMenuBorderMainPart").GetComponent<ButtonList>();
        m_MainMenuButtonList.isActive = true;

        if(IsSaveExist())
        {
            MainMenuButton l_ButtonContinue = Instantiate(MainMenuButton.prefab);
            l_ButtonContinue.AddAction(ContinueGame);
            l_ButtonContinue.title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:Continue");
            m_MainMenuButtonList.AddButton(l_ButtonContinue);
        }

        MainMenuButton l_ButtonNewGame = Instantiate(MainMenuButton.prefab);
        l_ButtonNewGame.AddAction(RunNewGame);
        l_ButtonNewGame.title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:NewGame");
        m_MainMenuButtonList.AddButton(l_ButtonNewGame);

        if(IsGameCompleted())
        {
            MainMenuButton l_ButtonNewGamePlus = Instantiate(MainMenuButton.prefab);
            l_ButtonNewGamePlus.AddAction(RunNewGamePlus);
            l_ButtonNewGamePlus.title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:NewGamePlus");
            m_MainMenuButtonList.AddButton(l_ButtonNewGamePlus);
        }
        if(IsSecretEndingUnlocked())
        {
            MainMenuButton l_ButtonNewGameAfterSecretEnding = Instantiate(MainMenuButton.prefab);
            l_ButtonNewGameAfterSecretEnding.AddAction(RunNewGameAfterSecretEnding);
            l_ButtonNewGameAfterSecretEnding.title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:NewGameAfterSecretEnding");
            m_MainMenuButtonList.AddButton(l_ButtonNewGameAfterSecretEnding);
        }

        MainMenuButton l_ButtonLoadGame = Instantiate(MainMenuButton.prefab);
        l_ButtonLoadGame.AddAction(LoadGame);
        l_ButtonLoadGame.title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:Load");
        m_MainMenuButtonList.AddButton(l_ButtonLoadGame);

        MainMenuButton l_ButtonSettings = Instantiate(MainMenuButton.prefab);
        l_ButtonSettings.AddAction(OpenSettings);
        l_ButtonSettings.title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:Settings");
        m_MainMenuButtonList.AddButton(l_ButtonSettings);

        MainMenuButton l_ButtonQuit = Instantiate(MainMenuButton.prefab);
        l_ButtonQuit.AddAction(QuitGame);
        l_ButtonQuit.title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:Exit");
        m_MainMenuButtonList.AddButton(l_ButtonQuit);
    }

    private void OpenSettings()
    {
        throw new NotImplementedException();
    }

    private void LoadGame()
    {
        throw new NotImplementedException();
    }

    private void RunNewGameAfterSecretEnding()
    {
        throw new NotImplementedException();
    }

    private bool IsSecretEndingUnlocked()
    {
        return true;
    }

    private void RunNewGamePlus()
    {
        throw new NotImplementedException();
    }

    private bool IsGameCompleted()
    {
        return true;
    }

    private void ContinueGame()
    {
        throw new NotImplementedException();
    }

    private bool IsSaveExist()
    {
        return true;
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

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_MainMenuButtonList.UpdateKey();

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
}
