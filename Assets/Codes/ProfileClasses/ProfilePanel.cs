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

    private ArrayList m_SelectedSpecialsList;
    private int m_StatImprovePoints = 0;
    private bool m_HaveStatPoints = false;
    private bool m_HaveClassupPoints = false;

    private ButtonList m_SpecialsButtonList = null;
    private ButtonList m_StatsButtonList = null;
    private ButtonList m_ProfileButtonList = null;
    private ButtonListScrolling m_SpecialButtonListScrolling = null;
    private Text m_SpecialDescriptionText = null;
    private Text m_StatImprovePointsText = null;
    private Text m_LevelText = null;
    private Text m_PlayerNameText = null;
    private Text m_NextLevelupText = null;
    private SpecialStatus m_SpecialStatus = null;
    private Image m_ClassupBackgroundImage = null;
    private Image m_ProfileAvatar = null;

    [SerializeField]
    private int m_MaxSelectedSpecialCount = 4;

    #region Interface
    public int statImprovePoints
    {
        get { return m_StatImprovePoints; }
        set
        {
            m_StatImprovePoints = value;

            PlayerData.GetInstance().statImprovePoints = m_StatImprovePoints;
            m_StatImprovePointsText.text = "Осталось очков:" + m_StatImprovePoints.ToString();
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
    }

    public void Update()
    {
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
                if (m_StatImprovePoints > 0)
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
                ConfirmStatImprove();
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                CancelStatImprove();
            }
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            if (GameManager.GetInstance().isTesting)
            {
                ProfileSystem.GetInstance().StartLocation("DemoMainScene");
            }
            else
            {
                ProfileSystem.GetInstance().UnloadScene();
                JourneySystem.GetInstance().SetControl(ControlType.Panel);
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
        List<SkillData> m_MonstyleList = PlayerData.GetInstance().GetSkills();
        for (int i = 0; i < m_MonstyleList.Count; i++)
        {
            PanelButtonProfileSpecial l_PanelButton = Instantiate(PanelButtonProfileSpecial.prefab);
            l_PanelButton.AddAction(SelectSpecial);
            l_PanelButton.title = LocalizationDataBase.GetInstance().GetText("Skill:" + m_MonstyleList[i].id);
            l_PanelButton.text.fontSize = 40;

            m_SpecialsButtonList.AddButton(l_PanelButton);
        }
        m_SelectedSpecialsList = new ArrayList();
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
        m_SpecialDescriptionText.text = l_PanelButtonProfileSpecial.title + " description";

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
        PanelButtonProfileSpecial l_PanelButtonProfileSpecial = (PanelButtonProfileSpecial)m_SpecialsButtonList.currentButton;
        if (!l_PanelButtonProfileSpecial.chosen)
        {
            l_PanelButtonProfileSpecial.chosen = true;
            m_SelectedSpecialsList.Add(l_PanelButtonProfileSpecial);
            m_SpecialStatus.Selected(true);

            if (m_SelectedSpecialsList.Count > m_MaxSelectedSpecialCount)
            {
                PanelButtonProfileSpecial l_PanelButtonProfileSpecialHead = (PanelButtonProfileSpecial)m_SelectedSpecialsList[0];
                l_PanelButtonProfileSpecialHead.chosen = false;
                m_SpecialStatus.Selected(false);
                m_SelectedSpecialsList.RemoveAt(0);
            }
        }
        else
        {
            l_PanelButtonProfileSpecial.chosen = false;
            m_SpecialStatus.Selected(false);
            m_SelectedSpecialsList.Remove(l_PanelButtonProfileSpecial);
        }
    }

    private void ConfirmStatImprove()
    {
        for (int i = 0; i < m_StatsButtonList.count; i++)
        {
            PanelButtonStat l_PanelButtonStat = (PanelButtonStat)m_StatsButtonList[i];
            l_PanelButtonStat.ConfirmAddedStatValue();
            PlayerData.GetInstance().GetStats()[l_PanelButtonStat.statId] = l_PanelButtonStat.statValue;
        }
        if (m_StatImprovePoints == 0)
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
        m_SpecialButtonListScrolling.Init(51.0f, 6);
        m_SpecialsButtonList.AddKeyArrowAction(m_SpecialButtonListScrolling.CheckScrolling);

        m_StatsButtonList = transform.FindChild("Stats").GetComponentInChildren<ButtonList>();
        m_StatsButtonList.isActive = false;
        m_StatsButtonList.AddCancelAction(ActiveProfilePanel);

        m_ProfileButtonList = GetComponent<ButtonList>();
        m_ProfileButtonList[0].AddAction(ActiveStatsPanel);
        m_ProfileButtonList[1].AddAction(OpenImprovePanel);
        m_ProfileButtonList[2].AddAction(ActiveSpecialListPanel);
    }

    private void CheckCanStatImprove()
    {
        if (PlayerData.GetInstance().statImprovePoints > 0)
        {
            m_HaveStatPoints = true;
            m_StatImprovePointsText.gameObject.SetActive(true);
            statImprovePoints = PlayerData.GetInstance().statImprovePoints;
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
    }
    #endregion
}
