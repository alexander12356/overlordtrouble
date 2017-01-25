using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPanel : Panel
{
    #region Variables
    private static MainMenuPanel m_Prefab;
    private ButtonList m_MainMenuButtonList = null;
    private Animator m_MainMenuTransitionAnimator = null;

    #endregion

    #region Properties
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

    private ButtonList mainMenuButtonList
    {
        get
        {
            if(m_MainMenuButtonList == null)
            {
                m_MainMenuButtonList = transform.FindChild("Transitor").FindChild("MainMenu").GetComponentInChildren<ButtonList>();
            }
            return m_MainMenuButtonList;
        }
    }

    private Animator mainMenuTransitionAnimator
    {
        get
        {
            if(m_MainMenuTransitionAnimator == null)
            {
                m_MainMenuTransitionAnimator = GetComponentInChildren<Animator>();
            }
            return m_MainMenuTransitionAnimator;
        }
    }

    #endregion

    #region Methods

    public override void Awake()
    {
        base.Awake();

        GameManager.GetInstance();

        Cursor.visible = false;
        MainMenuSystem.GetInstance().ShowPanel(this);

        InitButtonList();
    }

    public void Start()
    {
        AudioSystem.GetInstance().PlayMusic("MainMenu");
    }

    private void InitButtonList()
    {
        mainMenuButtonList.isActive = true;

        if(IsSaveExist())
        {
            MainMenuButton l_ButtonContinue = Instantiate(MainMenuButton.prefab);
            l_ButtonContinue.AddAction(ContinueGame);
            l_ButtonContinue.title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:Continue");
            mainMenuButtonList.AddButton(l_ButtonContinue);
        }

        MainMenuButton l_ButtonNewGame = Instantiate(MainMenuButton.prefab);
        l_ButtonNewGame.AddAction(RunNewGame);
        l_ButtonNewGame.title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:NewGame");
        mainMenuButtonList.AddButton(l_ButtonNewGame);

        if(IsGameCompleted())
        {
            MainMenuButton l_ButtonNewGamePlus = Instantiate(MainMenuButton.prefab);
            l_ButtonNewGamePlus.AddAction(RunNewGamePlus);
            l_ButtonNewGamePlus.title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:NewGamePlus");
            mainMenuButtonList.AddButton(l_ButtonNewGamePlus);
        }
        if(IsSecretEndingUnlocked())
        {
            MainMenuButton l_ButtonNewGameAfterSecretEnding = Instantiate(MainMenuButton.prefab);
            l_ButtonNewGameAfterSecretEnding.AddAction(RunNewGameAfterSecretEnding);
            l_ButtonNewGameAfterSecretEnding.title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:NewGameAfterSecretEnding");
            mainMenuButtonList.AddButton(l_ButtonNewGameAfterSecretEnding);
        }

        MainMenuButton l_ButtonLoadGame = Instantiate(MainMenuButton.prefab);
        l_ButtonLoadGame.AddAction(LoadGame);
        l_ButtonLoadGame.title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:Load");
        mainMenuButtonList.AddButton(l_ButtonLoadGame);

        MainMenuButton l_ButtonSettings = Instantiate(MainMenuButton.prefab);
        l_ButtonSettings.AddAction(OpenSettings);
        l_ButtonSettings.title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:Settings");
        mainMenuButtonList.AddButton(l_ButtonSettings);

        MainMenuButton l_ButtonQuit = Instantiate(MainMenuButton.prefab);
        l_ButtonQuit.AddAction(QuitGame);
        l_ButtonQuit.title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:Exit");
        mainMenuButtonList.AddButton(l_ButtonQuit);

        mainMenuTransitionAnimator.SetTrigger("StartTransition");
    }

    private void SelectMainMenu()
    {
        mainMenuButtonList.isActive = true;
    }

    private void OpenSettings()
    {
        mainMenuButtonList.isActive = false;
        SettingsPanel l_SettingsPanel = Instantiate(SettingsPanel.prefab);
        MainMenuSystem.GetInstance().ShowPanel(l_SettingsPanel);
        l_SettingsPanel.AddCancelAction(SelectMainMenu);
    }

    private void LoadGame()
    {
        LoadingPanel l_LoadingPanel = Instantiate(LoadingPanel.prefab);
        MainMenuSystem.GetInstance().ShowPanel(l_LoadingPanel, true);
    }

    private void RunNewGameAfterSecretEnding()
    {
        throw new NotImplementedException();
    }

    private bool IsSecretEndingUnlocked()
    {
        return false;
    }

    private void RunNewGamePlus()
    {
        mainMenuButtonList.isActive = false;
        LoadingPanel l_LoadingPanel = Instantiate(LoadingPanel.prefab);
        l_LoadingPanel.AddCancelAction(SelectMainMenu);
        MainMenuSystem.GetInstance().ShowPanel(l_LoadingPanel);
    }

    private bool IsGameCompleted()
    {
        return false;
    }

    private void ContinueGame()
    {
        throw new NotImplementedException();
    }

    private bool IsSaveExist()
    {
        return false;
    }

    private void RunNewGame()
    {
        MainMenuSystem.GetInstance().StartLocation("EnterName");
        AudioSystem.GetInstance().StopMusic("MainMenu");
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

    #endregion
}
