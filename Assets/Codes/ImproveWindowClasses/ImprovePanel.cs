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
        l_ImproveSkillsText += LocalizationDataBase.GetInstance().GetText("Skill:" + l_ImproveData.skills[0].id);
        for (int i = 1; i < l_ImproveData.skills.Count; i++)
        {
            l_ImproveSkillsText += ", " + LocalizationDataBase.GetInstance().GetText("Skill:" + l_ImproveData.skills[i].id);
        }

        string l_Text = "Вы выбрали класс " + LocalizationDataBase.GetInstance().GetText("Improvement:" + l_ImproveName) + l_ImproveSkillsText;
        m_TextPanel.SetText(new List<string>() { l_Text });
        m_TextPanel.AddPopAction(ReturnToMainMenu);

        PanelManager.GetInstance().ShowPanel(m_TextPanel, true);
        Vector3 m_TextPanelLocalPosition = m_TextPanel.myTransform.localPosition;
        m_TextPanelLocalPosition.y = -402;
        m_TextPanel.myTransform.localPosition = m_TextPanelLocalPosition;
    }

    //Called from Animation
    public void EndBlinking()
    {
        Select();
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
        PanelButtonImprove l_PanelButtonImprove = (PanelButtonImprove)m_ImproveButtonList.currentButton;
        l_PanelButtonImprove.ReadyChoose();

        YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
        l_YesNoPanel.AddYesAction(Select);
        l_YesNoPanel.AddNoAction(l_PanelButtonImprove.UnreadyChoose);
        PanelManager.GetInstance().ShowPanel(l_YesNoPanel, true);
    }

    private void Select()
    {
        PanelButtonImprove l_PanelButtonImprove = (PanelButtonImprove)m_ImproveButtonList.currentButton;
        l_PanelButtonImprove.StartBlinking();
        l_PanelButtonImprove.AddAfterBlinkingAnimation(RemoveButtonsButExcept);
        l_PanelButtonImprove.AddAfterSelectionAction(ShowProfile);
        m_ImproveButtonList.isActive = false;

        string l_ImproveId = ((PanelButtonImprove)m_ImproveButtonList.currentButton).improveData.id;
        PlayerEnchancement.GetInstance().AddEnchancement(l_ImproveId);
    }

    private void RemoveButtonsButExcept()
    {
        for (int i = 0; i < m_ImproveButtonList.count; i++)
        {
            if (m_ImproveButtonList[i] != m_ImproveButtonList.currentButton)
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
        m_ImproveCompleteImage.SetNativeSize();
        m_Animator.SetTrigger("Improve");
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("DemoMainScene");
    }
    #endregion
}
