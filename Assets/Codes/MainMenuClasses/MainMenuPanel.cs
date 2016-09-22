using UnityEngine;
using System.Collections;

public class MainMenuPanel : Panel
{
    private ButtonList m_ButtonList = null;
    private PanelManager m_PanelManager = null;

    public override void Awake()
    {
        base.Awake();

        m_PanelManager = GetComponentInParent<PanelManager>();

        m_ButtonList = GetComponentInChildren<ButtonList>();
        m_ButtonList[0].AddAction(RunNewGame);
        m_ButtonList[1].AddAction(QuitGame);
    }

    public void Update()
    {
        UpdatePanel();
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ButtonList.UpdateKey();

        if (Input.GetKeyUp(KeyCode.F12))
        {
            m_PanelManager.StartLocation("DemoMainScene");
        }
    }

    private void RunNewGame()
    {
        m_PanelManager.StartLocation("Town");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
