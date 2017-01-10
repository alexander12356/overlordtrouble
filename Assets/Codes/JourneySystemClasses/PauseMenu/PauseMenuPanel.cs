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
        m_ButtonList[3].AddAction(OpenQuitQuestionPanel);
        m_ButtonList.AddCancelAction(ReturnToJourney);
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
        JourneySystem.GetInstance().StartLocation("MainMenu");
    }

    private void OpenQuitQuestionPanel()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Panel);
        YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
        l_YesNoPanel.SetText(LocalizationDataBase.GetInstance().GetText("GUI:Journey:ReturnMainMenu"));
        l_YesNoPanel.AddYesAction(ConfirmReturnToMenu);
        JourneySystem.GetInstance().ShowPanel(l_YesNoPanel);
    }

    private void OpenProfilePanel()
    {
        JourneySystem.GetInstance().AddScene("Profile");
    }

    private void OpenInventoryPanel()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Panel);
        InventoryPanel lInventoryPanel = Instantiate(InventoryPanel.prefab);
        JourneySystem.GetInstance().ShowPanel(lInventoryPanel);
    }

    private void OpenEncyclopedia()
    {
        JourneySystem.GetInstance().AddScene("Encyclopedia");
    }

    private void ReturnToJourney()
    {
        Close();
        JourneySystem.GetInstance().SetControl(ControlType.Player);
    }
}
