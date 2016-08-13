using UnityEngine;
using UnityEngine.SceneManagement;

public class ImproveListPanel : Panel
{
    private ButtonList m_ImproveButtonList = null;

    public override void Awake()
    {
        base.Awake();

        m_ImproveButtonList = GetComponent<ButtonList>();
        InitButtonActions();

        PanelManager.GetInstance().ShowPanel(this);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ImproveButtonList.UpdateKey();

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("DemoMainScene");
        }
    }

    private void InitButtonActions()
    {
        m_ImproveButtonList[0].AddAction(ShowYesNoPanel);
        m_ImproveButtonList[1].AddAction(ShowYesNoPanel);
        m_ImproveButtonList[2].AddAction(ShowYesNoPanel);
        m_ImproveButtonList[3].AddAction(ShowYesNoPanel);
    }

    private void ShowYesNoPanel()
    {
        YesNoPanel m_YesNoPanel = Instantiate(YesNoPanel.prefab);
        PanelManager.GetInstance().ShowPanel(m_YesNoPanel, true);
    }
}
