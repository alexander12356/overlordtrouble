using System.Collections.Generic;

using UnityEngine;

public class EnterNameSystem : MonoBehaviour
{
    private static EnterNameSystem m_Instance = null;

    [SerializeField]
    private PanelManager m_PanelManager = null;

    public static EnterNameSystem GetInstance()
    {
        return m_Instance;
    }

    public void Awake()
    {
        GameManager.GetInstance();
        m_Instance = this;
    }

	public void Start ()
    {
        EnterNameTextPanel l_TextPanel = Instantiate(EnterNameTextPanel.prefab);
        l_TextPanel.AddButtonAction(l_TextPanel.Close);
        l_TextPanel.AddButtonAction(ShowEnterNamePanel);
        l_TextPanel.SetText(new List<string>() { LocalizationDataBase.GetInstance().GetText("GUI:EnterName:EnterName") });
        ShowPanel(l_TextPanel);
    }

    public void ShowPanel(Panel p_Panel, bool p_WithOverlay = false)
    {
        m_PanelManager.ShowPanel(p_Panel, p_WithOverlay);
    }

    public void StartGame()
    {
        PlayerPrefs.SetString("SenderLocation", "NewGame");
        PlayerPrefs.SetString("TargetRoomId", "HeroHome");
        SaveSystem.GetInstance().StartDuration();
        SaveSystem.GetInstance().Init("Town");
        AudioSystem.GetInstance().SetTheme("Town");
        m_PanelManager.StartLocation("Town");
    }

    private void ShowEnterNamePanel()
    {
        EnterNamePanel l_EnterNamePanel = Instantiate(EnterNamePanel.prefab);
        ShowPanel(l_EnterNamePanel);
    }
}
