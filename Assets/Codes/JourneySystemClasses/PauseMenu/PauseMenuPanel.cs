using UnityEngine;
using System.Collections;
using System;

public class PauseMenuPanel : Panel
{
    private static PauseMenuPanel m_Prefab;
    private ButtonList m_ButtonList = null;

    public static PauseMenuPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<PauseMenuPanel>("Prefabs/Panels/PauseMenuPanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_ButtonList = GetComponentInChildren<ButtonList>();
        m_ButtonList[0].AddAction(OpenProfilePanel);
        m_ButtonList[1].AddAction(OpenInventoryPanel);
        m_ButtonList[2].AddAction(OpenEncyclopedia);
        //m_ButtonList[3].AddAction(OpenOptions);
        m_ButtonList[4].AddAction(OpenQuitQuestionPanel);
        m_ButtonList.AddCancelAction(ReturnToJourney);

        JourneySystem.GetInstance().Pause();
    }

    public void Start()
    {
        m_ButtonList[0].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Profile");
        m_ButtonList[1].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Inventory");
        m_ButtonList[2].title = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Encyclopedia");
        m_ButtonList[3].title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:Settings");
        m_ButtonList[4].title = LocalizationDataBase.GetInstance().GetText("GUI:MainMenuPanel:Exit");
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ReturnToJourney();
        }

        m_ButtonList.UpdateKey();
    }

    private void ConfirmReturnToMenu()
    {
        AudioSystem.GetInstance().StopTheme();
        JourneySystem.GetInstance().ReturnToMainMenu();
    }

    private void OpenQuitQuestionPanel()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Panel);
        YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
        l_YesNoPanel.SetText(LocalizationDataBase.GetInstance().GetText("GUI:Journey:ReturnMainMenu"));
        l_YesNoPanel.AddYesAction(ConfirmReturnToMenu);
        JourneySystem.GetInstance().ShowPanel(l_YesNoPanel, true);
    }

    private void OpenProfilePanel()
    {
        JourneySystem.GetInstance().AddScene("Profile");
    }

    private void OpenInventoryPanel()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Panel);
        InventoryPanel lInventoryPanel = Instantiate(InventoryPanel.prefab);
        JourneySystem.GetInstance().ShowPanel(lInventoryPanel, true);
    }

    private void OpenEncyclopedia()
    {
        JourneySystem.GetInstance().AddScene("Encyclopedia");
    }

    private void ReturnToJourney()
    {
        Close();

        JourneySystem.GetInstance().SetControl(ControlType.Player);
        JourneySystem.GetInstance().Resume();

        AudioSystem.GetInstance().ResumeMainTheme();
    }
}
