using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections.Generic;
using System.Collections;

public class ProfileWindow : MonoBehaviour
{
    private enum ActivePanels
    {
        Profile,
        Stats,
        SpecialList
    }

    private ButtonList m_ProfileButtonList;
    private ArrayList m_SelectedSpecialsList;
    private int m_StatImprovePoints = 0;
    private bool m_HaveStatPoints = false;

    [SerializeField]
    private int m_MaxSelectedSpecialCount = 5;

    [SerializeField]
    private ButtonList m_SpecialsButtonList = null;

    [SerializeField]
    private ButtonList m_StatsButtonList = null;

    [SerializeField]
    private Text m_SpecialDescriptionText = null;

    [SerializeField]
    private SpecialStatus m_SpecialStatus = null;

    [SerializeField]
    private Text m_StatImprovePointsText = null;

    [SerializeField]
    private ButtonListScrolling m_SpecialButtonListScrolling = null;

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

    public void Awake ()
    {
        m_ProfileButtonList = GetComponent<ButtonList>();
        m_ProfileButtonList[0].AddAction(ActiveStatsPanel);
        m_ProfileButtonList[1].AddAction(StartImprove);
        m_ProfileButtonList[2].AddAction(ActiveSpecialListPanel);

        m_SpecialsButtonList.isActive = false;
        m_SpecialsButtonList.AddKeyArrowAction(ShowSpecialDescription);
        m_SpecialsButtonList.AddCancelAction(ActiveProfilePanel);
        m_StatsButtonList.isActive = false;
        m_StatsButtonList.AddCancelAction(ActiveProfilePanel);

        InitStats();
        InitSpecials();

        m_SpecialButtonListScrolling.Init(51.0f, 6);
        m_SpecialsButtonList.AddKeyArrowAction(m_SpecialButtonListScrolling.CheckScrolling);

        if (PlayerData.GetInstance().statImprovePoints > 0)
        {
            m_HaveStatPoints = true;
            m_StatImprovePointsText.gameObject.SetActive(true);
            statImprovePoints = PlayerData.GetInstance().statImprovePoints;
        }
    }
	
	public void Update ()
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

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("DemoMainScene");
        }

        m_ProfileButtonList.UpdateKey();
        m_SpecialsButtonList.UpdateKey();
        m_StatsButtonList.UpdateKey();
    }
    #endregion

    #region Private
    private void InitStats()
    {
        Dictionary<string, int> l_PlayerStats = PlayerStat.GetInstance().GetStats();

        foreach (string l_StatId in l_PlayerStats.Keys)
        {
            PanelButtonStat l_PanelButton = Instantiate(PanelButtonStat.prefab);

            l_PanelButton.title = LocalizationDataBase.GetInstance().GetText("Stat:" + l_StatId);
            l_PanelButton.statId = l_StatId;
            l_PanelButton.statValue = PlayerStat.GetInstance().GetStats()[l_StatId];

            m_StatsButtonList.AddButton(l_PanelButton);
        }
    }

    private void InitSpecials()
    {
        for (int i = 0; i < 12; i++)
        {
            PanelButtonProfileSpecial l_PanelButton = Instantiate(PanelButtonProfileSpecial.prefab);
            l_PanelButton.AddAction(SelectSpecial);
            l_PanelButton.title = "SP" + (i + 1);
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
            PlayerStat.GetInstance().GetStats()[l_PanelButtonStat.statId] = l_PanelButtonStat.statValue;
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

    private void StartImprove()
    {
        SceneManager.LoadScene("Improve");
    }

    private void ResizeSpecialList()
    {
        //m_SpecialsButtonList.gameObject.GetComponent<RectTransform>(). m_SpecialsButtonList.count
    }
    #endregion
}
