using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

public class ImprovePanel : Panel
{
    #region Variables
    private ButtonList m_ImproveButtonList = null;
    private Animator   m_Animator = null;
    private Image m_BackgroundAfterSelectImage = null;
    private Image m_BackgroundBeforeSelectImage = null;

    [SerializeField]
    private List<ImproveItem> m_ImproveList = null;

    [SerializeField]
    private Image m_ImproveCompleteImage = null;
    #endregion

    #region Interface
    public override void Awake()
    {
        base.Awake();

        m_BackgroundAfterSelectImage  = myTransform.FindChild("BackgroundAfterSelect").GetComponent<Image>();
        m_BackgroundBeforeSelectImage = myTransform.FindChild("BackgroundBeforeSelect").GetComponent<Image>();
        m_ImproveButtonList = GetComponent<ButtonList>();
        m_Animator = GetComponent<Animator>();
        InitImproveButtons();

        PanelManager.GetInstance().ShowPanel(this);
        ImproveDataBase.GetInstance();
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ImproveButtonList.UpdateKey();

        if (Input.GetKey(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }
    }

    // Called from Animation
    public void ImproveComplete()
    {
        TextPanelImproveWindow m_TextPanel = Instantiate(TextPanelImproveWindow.prefab);

        string l_ImproveName = ((PanelButtonImprove)m_ImproveButtonList.currentButton).improveData.id;
        string l_ImproveSkillsText = ", вы получили следующие приемы:\n";
        ImproveData l_ImproveData = ImproveDataBase.GetInstance().GetImprove(l_ImproveName);
        l_ImproveSkillsText += l_ImproveData.skills[0].id;
        for (int i = 1; i < l_ImproveData.skills.Count; i++)
        {
            l_ImproveSkillsText += ", " + l_ImproveData.skills[i].id;
        }

        string l_Text = "Вы выбрали класс " + l_ImproveName + l_ImproveSkillsText;
        m_TextPanel.SetText(new List<string>() { l_Text });
        m_TextPanel.AddPopAction(ReturnToMainMenu);

        PanelManager.GetInstance().ShowPanel(m_TextPanel, true);
        Vector3 m_TextPanelLocalPosition = m_TextPanel.myTransform.localPosition;
        m_TextPanelLocalPosition.y = -402;
        m_TextPanel.myTransform.localPosition = m_TextPanelLocalPosition;
    }
    #endregion

    #region Private
    private void InitImproveButtons()
    {
        for (int i = 0; i < m_ImproveButtonList.count; i++)
        {
            m_ImproveButtonList[i].AddAction(ShowYesNoPanel);
        }
        ((PanelButtonImprove)m_ImproveButtonList[0]).improveData = ImproveDataBase.GetInstance().GetImprove("LesserWaterElemental");
        ((PanelButtonImprove)m_ImproveButtonList[1]).improveData = ImproveDataBase.GetInstance().GetImprove("ShellLizard");
        ((PanelButtonImprove)m_ImproveButtonList[2]).improveData = ImproveDataBase.GetInstance().GetImprove("Brownie");

    }

    private void ShowYesNoPanel()
    {
        YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
        l_YesNoPanel.AddYesAction(Select);
        l_YesNoPanel.AddYesAction(l_YesNoPanel.Cancel);
        PanelManager.GetInstance().ShowPanel(l_YesNoPanel, true);
    }

    private void Select()
    {
        PanelButtonImprove l_PanelButtonImprove = (PanelButtonImprove)m_ImproveButtonList.currentButton;
        l_PanelButtonImprove.Choose();
        l_PanelButtonImprove.AddShowProfileAction(ShowProfile);

        for (int i = 0; i < m_ImproveButtonList.count; i++)
        {
            if (m_ImproveButtonList[i] != l_PanelButtonImprove)
            {
                ((PanelButtonImprove)m_ImproveButtonList[i]).Away();
            }
        }
        m_ImproveButtonList.isActive = false;
    }

    private void ShowProfile()
    {
        string l_ProfileImagePath = ((PanelButtonImprove)m_ImproveButtonList.currentButton).improveData.profileImagePath;
        m_ImproveCompleteImage.sprite = Resources.Load<Sprite>(l_ProfileImagePath);
        m_Animator.SetTrigger("Improve");
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("DemoMainScene");
    }
    #endregion
}
