using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BedAction : MonoBehaviour
{
    [SerializeField]
    private Transform m_TextBoxTransform = null;

    public virtual void Sleep()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Panel);
        StartCoroutine(StartingSleep());
    }

    private IEnumerator StartingSleep()
    {
        yield return StartCoroutine(JourneySystem.GetInstance().panelManager.screenFader.FadeToBlack());

        JourneyTextPanel l_TextPanel = Instantiate(JourneyTextPanel.prefab);
        l_TextPanel.AddButtonAction(EndSleep);
        l_TextPanel.AddButtonAction(l_TextPanel.Close);
        string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:Journey:Sleep");
        l_TextPanel.SetText(new List<string>() { l_Text });

        JourneySystem.GetInstance().ShowPanel(l_TextPanel, false, m_TextBoxTransform);
    }

    private void EndSleep()
    {
        StartCoroutine(EndingSleep());
    }

    private IEnumerator EndingSleep()
    {
        PlayerData.GetInstance().health = PlayerData.GetInstance().GetStats()["HealthPoints"];
        PlayerData.GetInstance().specialPoints = PlayerData.GetInstance().GetStats()["MonstylePoints"];

        yield return StartCoroutine(JourneySystem.GetInstance().panelManager.screenFader.FadeToClear());

        JourneySystem.GetInstance().SetControl(ControlType.Player);
    }
}
