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

        ImproveSystem.GetInstance().ShowPanel(this);
        m_BackgroundAfterSelectImage  = myTransform.FindChild("BackgroundAfterSelect").GetComponent<Image>();
        m_BackgroundBeforeSelectImage = myTransform.FindChild("BackgroundBeforeSelect").GetComponent<Image>();
        m_ImproveButtonList = GetComponent<ButtonList>();
        m_Animator = GetComponent<Animator>();
        InitImproveIcons();
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ImproveButtonList.UpdateKey();

        if (Input.GetKey(KeyCode.X))
        {
            ReturnToMainMenu();
        }
    }

    // Called from Animation
    public void ImproveComplete()
    {
        string l_ImproveId = ((PanelButtonImprove)m_ImproveButtonList.currentButton).improveData.id;
        string l_SkillsText = string.Empty;
        string l_ImproveName = LocalizationDataBase.GetInstance().GetText("Improvement:" + l_ImproveId);

        ImproveData l_ImproveData = ImproveDataBase.GetInstance().GetImprove(l_ImproveId);
        for (int i = 0; i < l_ImproveData.skills.Count; i++)
        {
            l_SkillsText += LocalizationDataBase.GetInstance().GetText("Special:" + l_ImproveData.skills[i].id);
            if (i < l_ImproveData.skills.Count - 1)
            {
                l_SkillsText += ", ";
            }
        }
        string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:Improve:СlassChoosed", new string[] { l_ImproveName, l_SkillsText });

        TextPanelImproveWindow l_TextPanel = Instantiate(TextPanelImproveWindow.prefab);
        l_TextPanel.SetText(new List<string>() { l_Text });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);
        l_TextPanel.AddPopAction(ReturnToMainMenu);

        ImproveSystem.GetInstance().ShowPanel(l_TextPanel, true);
        Vector3 m_TextPanelLocalPosition = l_TextPanel.myTransform.localPosition;
        m_TextPanelLocalPosition.y = -402;
        l_TextPanel.myTransform.localPosition = m_TextPanelLocalPosition;
    }

    // Called from Animation
    public void EndBlinking()
    {
        SelectImprove();
    }

    #endregion

    #region Private
    private void InitImproveIcons()
    {
        for (int i = 0; i < m_ImproveButtonList.count - 1; i++)
        {
            m_ImproveButtonList[i].AddAction(ShowYesNoPanel);
        }
        ((PanelButtonImprove)m_ImproveButtonList[0]).improveData = ImproveDataBase.GetInstance().GetImprove("LesserWaterElemental");
        ((PanelButtonImprove)m_ImproveButtonList[1]).improveData = ImproveDataBase.GetInstance().GetImprove("ShellLizard");
        ((PanelButtonImprove)m_ImproveButtonList[2]).improveData = ImproveDataBase.GetInstance().GetImprove("Brownie");

        m_ImproveButtonList[m_ImproveButtonList.count - 1].AddAction(ReturnToMainMenu);
        m_ImproveButtonList[m_ImproveButtonList.count - 1].title = LocalizationDataBase.GetInstance().GetText("GUI:Improve:StayMyself");
    }

    private void ShowYesNoPanel()
    {
        PanelButtonImprove l_PanelButtonImprove = (PanelButtonImprove)m_ImproveButtonList.currentButton;
        l_PanelButtonImprove.ReadyChoose();

        YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
        l_YesNoPanel.AddYesAction(SelectImprove);
        l_YesNoPanel.AddNoAction(l_PanelButtonImprove.UnreadyChoose);
        l_YesNoPanel.SetText(LocalizationDataBase.GetInstance().GetText("GUI:Improve:AreYouShure"));
        ImproveSystem.GetInstance().ShowPanel(l_YesNoPanel, true);
    }

    private void SelectImprove()
    {
        PanelButtonImprove l_PanelButtonImprove = (PanelButtonImprove)m_ImproveButtonList.currentButton;
        l_PanelButtonImprove.StartBlinking();
        l_PanelButtonImprove.AddAfterBlinkingAnimation(RemoveImproveIconsButExcept);
        l_PanelButtonImprove.AddAfterSelectionAction(ShowProfile);
        m_ImproveButtonList.isActive = false;

        string l_ImproveId = ((PanelButtonImprove)m_ImproveButtonList.currentButton).improveData.id;
        PlayerData.GetInstance().AddEnchancement(l_ImproveId);
        PlayerData.GetInstance().classImprovePoints--;
    }

    private void RemoveImproveIconsButExcept()
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
        ImproveSystem.GetInstance().UnloadScene();
    }
    #endregion
}
