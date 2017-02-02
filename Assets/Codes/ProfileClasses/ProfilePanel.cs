using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections.Generic;
using System.Collections;

public class ProfilePanel : Panel
{
    private enum ActivePanels
    {
        Profile,
        Stats,
        SpecialList
    }

    private int m_StatImprovePoints = 0;
    private int m_BaseStatImprovePoints = 0;
    private bool m_HaveStatPoints = false;
    private bool m_HaveClassupPoints = false;

    private ArrayList m_SelectedSpecialButtons;
    private ButtonList m_SpecialsButtonList = null;
    private ButtonList m_StatsButtonList = null;
    private ButtonList m_ProfileButtonList = null;
    private ButtonListScrolling m_SpecialButtonListScrolling = null;
    private Text m_SpecialDescriptionText = null;
    private Text m_StatImprovePointsText = null;
    private Text m_LevelText = null;
    private Text m_PlayerNameText = null;
    private Text m_NextLevelupText = null;
    private Text m_ClassName = null;
    private Text m_ClassDescription = null;
    private Text m_ClassSpecialty = null;
    private SpecialStatus m_SpecialStatus = null;
    private Image m_ClassupBackgroundImage = null;
    private Image m_ProfileAvatar = null;

    [SerializeField]
    private int m_MaxSelectedSpecialCount = 5;

    #region Interface
    public int statImprovePoints
    {
        get { return m_StatImprovePoints; }
        set
        {
            m_StatImprovePoints = value;
            m_StatImprovePointsText.text = LocalizationDataBase.GetInstance().GetText("GUI:Profile:StatsPoints", new string[] { m_StatImprovePoints.ToString() });
        }
    }

    public override void Awake()
    {
        base.Awake();
        
        if (GameManager.GetInstance().isTesting)
        {
            PlayerData.GetInstance().InitTestStats();
        }

        InitAdditionalInfo();
        InitButtonLists();
        InitStats();
        InitMonstyles();
        CheckCanClassup();
        CheckCanStatImprove();

        ProfileSystem.GetInstance().ShowPanel(this);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (m_StatsButtonList.isActive && m_HaveStatPoints)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                PanelButtonStat l_PanelButtonStat = (PanelButtonStat)m_StatsButtonList.currentButton;
                if (l_PanelButtonStat.addedStatValue > 0)
                {
                    l_PanelButtonStat.PlayAnim("StatMinus");
                    l_PanelButtonStat.addedStatValue -= 1;
                    statImprovePoints += 1;
                }
                else
                {
                    l_PanelButtonStat.PlayAnim("StatCannotMinus");
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PanelButtonStat l_PanelButtonStat = (PanelButtonStat)m_StatsButtonList.currentButton;
                if (statImprovePoints > 0)
                {
                    l_PanelButtonStat.PlayAnim("StatPlus");
                    l_PanelButtonStat.addedStatValue += 1;
                    statImprovePoints -= 1;
                }
                else
                {
                    l_PanelButtonStat.PlayAnim("StatCannotPlus");
                }
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                if (m_BaseStatImprovePoints != m_StatImprovePoints)
                {
                    QuestionStatImprove();
                }
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                CancelStatImprove();
            }
        }

        m_ProfileButtonList.UpdateKey();
        m_SpecialsButtonList.UpdateKey();
        m_StatsButtonList.UpdateKey();
    }

    public void OnEnable()
    {
        CheckCanClassup();
        LoadEnchancement();
    }

    #endregion

    #region Private
    private void InitStats()
    {
        Dictionary<string, int> l_PlayerStats = PlayerData.GetInstance().GetStats();

        foreach (string l_StatId in l_PlayerStats.Keys)
        {
            PanelButtonStat l_PanelButton = Instantiate(PanelButtonStat.prefab);

            l_PanelButton.title = LocalizationDataBase.GetInstance().GetText("Stat:" + l_StatId);
            l_PanelButton.statId = l_StatId;
            l_PanelButton.statValue = l_PlayerStats[l_StatId];

            m_StatsButtonList.AddButton(l_PanelButton);
        }
    }

    private void InitMonstyles()
    {
        m_SelectedSpecialButtons = new ArrayList();
        List<SpecialData> l_MonstyleList = PlayerData.GetInstance().GetSkills();
        List<SpecialData> l_SelectedSpecialsList = PlayerData.GetInstance().GetSelectedSkills();
        for (int i = 0; i < l_MonstyleList.Count; i++)
        {
            PanelButtonProfileSpecial l_SpecialButton = Instantiate(PanelButtonProfileSpecial.prefab);
            l_SpecialButton.AddAction(SelectSpecial);
            l_SpecialButton.monstyleId = l_MonstyleList[i].id;
            // проверка на то, был ли выбран этот спешл ранее
            if (l_SelectedSpecialsList.Contains(SpecialDataBase.GetInstance().GetSpecialData(l_SpecialButton.monstyleId)))
            {
                l_SpecialButton.initChoosen = true;
                m_SelectedSpecialButtons.Add(l_SpecialButton);
            }
            m_SpecialsButtonList.AddButton(l_SpecialButton);
        }
        m_SpecialButtonListScrolling.Init(51.0f, 6);
    }

    private void ActiveSpecialListPanel()
    {
        m_ProfileButtonList.isActive = false;
        m_SpecialsButtonList.isActive = true;
    }

    private void ActiveProfilePanel()
    {
        m_ProfileButtonList.isActive = true;
        m_SpecialsButtonList.isActive = false;
        m_StatsButtonList.isActive = false;
    }

    private void ActiveStatsPanel()
    {
        if (m_HaveStatPoints)
        {
            m_StatsButtonList.isActive = true;
            m_ProfileButtonList.isActive = false;
        }
    }

    private void ShowSpecialDescription()
    {
        PanelButtonProfileSpecial l_PanelButtonProfileSpecial = (PanelButtonProfileSpecial)m_SpecialsButtonList.currentButton;
        m_SpecialDescriptionText.text = LocalizationDataBase.GetInstance().GetText("Special:" + l_PanelButtonProfileSpecial.monstyleId + ":Description");

        if (l_PanelButtonProfileSpecial.chosen)
        {
            m_SpecialStatus.Selected(true);
        }
        else
        {
            m_SpecialStatus.Selected(false);
        }
    }

    private void SelectSpecial()
    {
        PanelButtonProfileSpecial l_SpecialButton = (PanelButtonProfileSpecial)m_SpecialsButtonList.currentButton;
        if (!l_SpecialButton.chosen)
        {
            l_SpecialButton.chosen = true;
            PlayerData.GetInstance().SelectSkill(l_SpecialButton.monstyleId);
            m_SelectedSpecialButtons.Add(l_SpecialButton);
            m_SpecialStatus.Selected(true);

            if (PlayerData.GetInstance().GetSelectedSkills().Count > m_MaxSelectedSpecialCount)
            {
                PanelButtonProfileSpecial l_SpecialUnselectButton = (PanelButtonProfileSpecial)m_SelectedSpecialButtons[0];
                l_SpecialUnselectButton.chosen = false;
                m_SpecialStatus.Selected(false);
                PlayerData.GetInstance().RemoveFirstSelectedSkill();
                m_SelectedSpecialButtons.RemoveAt(0);
            }
        }
        else
        {
            l_SpecialButton.chosen = false;
            m_SpecialStatus.Selected(false);
            PlayerData.GetInstance().UnselectSkill(l_SpecialButton.monstyleId);
            m_SelectedSpecialButtons.Remove(l_SpecialButton);
        }
    }

    private void QuestionStatImprove()
    {
        YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
        l_YesNoPanel.AddYesAction(ConfirmStatImprove);
        l_YesNoPanel.SetText(LocalizationDataBase.GetInstance().GetText("GUI:Profile:QuestionImproveStats"));

        ProfileSystem.GetInstance().ShowPanel(l_YesNoPanel, true);
    }

    private void ConfirmStatImprove()
    {
        PlayerData.GetInstance().statImprovePoints = m_BaseStatImprovePoints = m_StatImprovePoints;

        for (int i = 0; i < m_StatsButtonList.count; i++)
        {
            PanelButtonStat l_PanelButtonStat = (PanelButtonStat)m_StatsButtonList[i];
            l_PanelButtonStat.ConfirmAddedStatValue();
            
            switch (l_PanelButtonStat.statId)
            {
                case "HealthPoints":
                    PlayerData.GetInstance().health += l_PanelButtonStat.statValue - PlayerData.GetInstance().GetStats()[l_PanelButtonStat.statId];
                    break;
                case "MonstylePoints":
                    PlayerData.GetInstance().specialPoints += l_PanelButtonStat.statValue - PlayerData.GetInstance().GetStats()[l_PanelButtonStat.statId];
                    break;
            }

            PlayerData.GetInstance().GetStats()[l_PanelButtonStat.statId] = l_PanelButtonStat.statValue;
        }
        if (statImprovePoints == 0)
        {
            m_HaveStatPoints = false;
            m_StatImprovePointsText.gameObject.SetActive(false);
        }
    }

    private void CancelStatImprove()
    {
        for (int i = 0; i < m_StatsButtonList.count; i++)
        {
            PanelButtonStat l_PanelButtonStat = (PanelButtonStat)m_StatsButtonList[i];
            statImprovePoints += l_PanelButtonStat.CancelAddedStatValue();
        }
    }

    private void OpenImprovePanel()
    {
        if (m_HaveClassupPoints)
        {
            ProfileSystem.GetInstance().AddScene("Improve");
        }
    }

    private void InitButtonLists()
    {
        m_SpecialsButtonList = transform.FindChild("SpecialList").FindChild("MaskSpecialList").GetComponentInChildren<ButtonList>();
        m_SpecialsButtonList.isActive = false;
        m_SpecialsButtonList.AddKeyArrowAction(ShowSpecialDescription);
        m_SpecialsButtonList.AddCancelAction(ActiveProfilePanel);

        m_SpecialButtonListScrolling = transform.FindChild("SpecialList").GetComponentInChildren<ButtonListScrolling>();
        m_SpecialsButtonList.AddKeyArrowAction(m_SpecialButtonListScrolling.CheckScrolling);

        m_StatsButtonList = transform.FindChild("Stats").GetComponentInChildren<ButtonList>();
        m_StatsButtonList.isActive = false;
        m_StatsButtonList.AddCancelAction(ActiveProfilePanel);

        m_ProfileButtonList = GetComponent<ButtonList>();
        m_ProfileButtonList[0].AddAction(ActiveStatsPanel);
        m_ProfileButtonList[1].AddAction(OpenImprovePanel);
        m_ProfileButtonList[1].title = LocalizationDataBase.GetInstance().GetText("GUI:Profile:OpenImprovePanel");
        m_ProfileButtonList[2].AddAction(ActiveSpecialListPanel);
        m_ProfileButtonList.AddCancelAction(ReturnToJourney);
    }

    private void CheckCanStatImprove()
    {
        if (PlayerData.GetInstance().statImprovePoints > 0)
        {
            m_HaveStatPoints = true;
            m_StatImprovePointsText.gameObject.SetActive(true);
            statImprovePoints = m_BaseStatImprovePoints = PlayerData.GetInstance().statImprovePoints;
        }
        else
        {
            m_StatImprovePointsText.gameObject.SetActive(false);
        }
    }

    private void InitAdditionalInfo()
    {
        m_SpecialDescriptionText = transform.FindChild("SpecialDescription").GetComponentInChildren<Text>();
        m_StatImprovePointsText = transform.FindChild("StatPoints").GetComponentInChildren<Text>();
        m_LevelText = transform.FindChild("Level").GetComponentInChildren<Text>();
        m_PlayerNameText = transform.FindChild("Name").GetComponentInChildren<Text>();
        m_NextLevelupText = transform.FindChild("Experience").GetComponentInChildren<Text>();
        m_SpecialStatus = transform.FindChild("SpecialSelect").GetComponent<SpecialStatus>();
        m_ClassupBackgroundImage = transform.FindChild("Improve").FindChild("Background").GetComponent<Image>();
        m_ProfileAvatar = transform.FindChild("Avatar").GetComponentInChildren<Image>();
        m_ClassName = transform.FindChild("ImprovementTitle").GetComponentInChildren<Text>();
        m_ClassDescription = transform.FindChild("ImprovementDescription").GetComponentInChildren<Text>();
        m_ClassSpecialty = transform.FindChild("ImprovementSpecialty").GetComponentInChildren<Text>();

        m_LevelText.text = LocalizationDataBase.GetInstance().GetText("GUI:Profile:Level") + " " + (PlayerData.GetInstance().GetLevel() + 1);
        m_PlayerNameText.text = PlayerData.GetInstance().GetPlayerName();
        m_NextLevelupText.text = LocalizationDataBase.GetInstance().GetText("GUI:Profile:NextLevelup") + " " + PlayerData.GetInstance().GetNextLevelupExperience() + " exp";

        m_ProfileAvatar.sprite = PlayerData.GetInstance().GetProfileAvatar();
        m_ProfileAvatar.SetNativeSize();
    }

    private void CheckCanClassup()
    {
        if (PlayerData.GetInstance().classImprovePoints > 0)
        {
            m_ClassupBackgroundImage.sprite = Resources.Load<Sprite>("Sprites/GUI/Profile/ClassupActivated");
            m_HaveClassupPoints = true;
        }
        else
        {
            m_ClassupBackgroundImage.sprite = Resources.Load<Sprite>("Sprites/GUI/Profile/ClassupUnactivated");
            m_HaveClassupPoints = false;
        }
    }

    private void LoadEnchancement()
    {
        m_ProfileAvatar.sprite = PlayerData.GetInstance().GetProfileAvatar();
        m_ProfileAvatar.SetNativeSize();

        m_SpecialsButtonList.Clear();
        InitMonstyles();
        InitClassDescription();
    }

    private void ReturnToJourney()
    {
        ProfileSystem.GetInstance().UnloadScene();

        if (JourneySystem.IsInstance())
        {
            JourneySystem.GetInstance().SetControl(ControlType.Panel);
        }
    }

    private void InitClassDescription()
    {
        m_ClassName.text = LocalizationDataBase.GetInstance().GetText("Improvement:" + PlayerData.GetInstance().GetCurrentEnchancement());
        m_ClassDescription.text = LocalizationDataBase.GetInstance().GetText("Improvement:" + PlayerData.GetInstance().GetCurrentEnchancement() + ":Description");
        m_ClassSpecialty.text = LocalizationDataBase.GetInstance().GetText("Improvement:" + PlayerData.GetInstance().GetCurrentEnchancement() + ":Specialty");
    }
    #endregion
}
