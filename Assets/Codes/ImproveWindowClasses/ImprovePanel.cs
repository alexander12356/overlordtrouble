using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

public class ImprovePanel : Panel
{
    private ButtonList m_ImproveButtonList = null;
    private ButtonList m_CancelButtonList = null;
    private Animator   m_Animator = null;
    private Image m_BackgroundAfterSelectImage = null;
    private Image m_BackgroundBeforeSelectImage = null;
    private string m_CurrentClassId = null;
    private GridLayoutGroup m_ClassesLayoutGroup = null;
    private GridLayoutGroup m_CancelLayoutGroup = null;

    [SerializeField]
    private Image m_ImproveCompleteImage = null;
    
    public override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        if (GameManager.IsInstance() == false)
        {
            PlayerData.GetInstance().AddEnchancement("DewElemental");
        }
#endif

        m_BackgroundAfterSelectImage  = myTransform.FindChild("BackgroundAfterSelect").GetComponent<Image>();
        m_BackgroundBeforeSelectImage = myTransform.FindChild("BackgroundBeforeSelect").GetComponent<Image>();
        m_ImproveButtonList = transform.FindChild("Buttons").FindChild("NewClasses").GetComponent<ButtonList>();
        m_CancelButtonList = transform.FindChild("Buttons").FindChild("Cancel").GetComponent<ButtonList>();
        m_Animator = GetComponent<Animator>();
        m_ClassesLayoutGroup = transform.FindChild("Buttons").FindChild("NewClasses").GetComponent<GridLayoutGroup>();
        m_CancelLayoutGroup = transform.FindChild("Buttons").FindChild("Cancel").GetComponent<GridLayoutGroup>();
    }

    public void Start()
    {
        ImproveSystem.GetInstance().ShowPanel(this);

        m_CurrentClassId = PlayerData.GetInstance().GetCurrentEnchancement();
        InitNewClassesIcon();
        InitCancelIcon();

        if (m_ImproveButtonList.count > 0)
        {
            m_ImproveButtonList.isActive = true;
            m_CancelButtonList.isActive = false;
        }
        else
        {
            m_ImproveButtonList.isActive = false;
            m_CancelButtonList.isActive = true;
        }
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ImproveButtonList.UpdateKey();
        m_CancelButtonList.UpdateKey();

        if (ControlSystem.ExitButton())
        {
            ReturnToMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && m_ImproveButtonList.count > 0)
        {
            m_ImproveButtonList.isActive = true;
            m_CancelButtonList.isActive = false;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_ImproveButtonList.isActive = false;
            m_CancelButtonList.isActive = true;
        }
    }

    #region IMPROVE
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
        LayoutDisable();

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
        (m_CancelButtonList[0] as PanelButtonImprove).Away();

        m_ImproveButtonList.isActive = false;
    }

    private void ShowProfile()
    {
        string l_ProfileImagePath = ((PanelButtonImprove)m_ImproveButtonList.currentButton).improveData.profileImagePath;
        m_ImproveCompleteImage.sprite = Resources.Load<Sprite>(l_ProfileImagePath);
        m_ImproveCompleteImage.SetNativeSize();
        m_Animator.SetTrigger("Improve");
    }
    #endregion

    #region INIT_BUTTONS
    private void InitNewClassesIcon()
    {
        List<string> l_NewClassesIds = ImproveHierarchyDataBase.GetInstance().GetImproveList(m_CurrentClassId);

        for (int i = 0; i < l_NewClassesIds.Count; i++)
        {
            PanelButtonImprove l_Button = Instantiate(PanelButtonImprove.prefab);
            l_Button.improveData = ImproveDataBase.GetInstance().GetImprove(l_NewClassesIds[i]);
            l_Button.AddAction(ShowYesNoPanel);

            m_ImproveButtonList.AddButton(l_Button);
        }
    }

    private void InitCancelIcon()
    {
        PanelButtonImprove l_Button = Instantiate(PanelButtonImprove.prefab);
        l_Button.improveData = ImproveDataBase.GetInstance().GetImprove(m_CurrentClassId);
        l_Button.title = LocalizationDataBase.GetInstance().GetText("GUI:Improve:StayMyself");
        l_Button.AddAction(ReturnToMainMenu);

        m_CancelButtonList.AddButton(l_Button);
    }

    private void LayoutDisable()
    {
        m_ClassesLayoutGroup.enabled = false;
        m_CancelLayoutGroup.enabled = false;
    }
    #endregion

    private void ReturnToMainMenu()
    {
        ImproveSystem.GetInstance().UnloadScene();
    }
}
