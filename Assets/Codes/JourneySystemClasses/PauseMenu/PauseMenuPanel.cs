using UnityEngine;
using System.Collections;

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
        //m_ButtonList[0].AddAction(OpenProfilePanel);
        m_ButtonList[3].AddAction(OpenQuitQuestionPanel);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            JourneySystem.GetInstance().SetControl(ControlType.Player);
            Close();
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
        l_YesNoPanel.SetText(LocalizationDataBase.GetInstance().GetText("ReturnMainMenu"));
        l_YesNoPanel.AddYesAction(ConfirmReturnToMenu);
        JourneySystem.GetInstance().ShowPanel(l_YesNoPanel);
    }

    private void OpenProfilePanel()
    {
        JourneySystem.GetInstance().AddScene("Profile");
    }
}
