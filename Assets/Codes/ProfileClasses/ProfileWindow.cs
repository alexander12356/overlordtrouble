using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

public class ProfileWindow : MonoBehaviour
{
    private enum ActivePanels
    {
        Profile,
        Stats,
        SpecialList
    }

    private ButtonList m_ProfileButtonList;
    private ActivePanels m_CurrentActivePanel = ActivePanels.Profile;
    private int m_SelectedSpecialCount = 0;
    private int m_StatImprovePoints = 0;
    private bool m_HaveStatPoints = false;

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
                    l_PanelButtonStat.addedStatValue -= 1;
                    statImprovePoints += 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PanelButtonStat l_PanelButtonStat = (PanelButtonStat)m_StatsButtonList.currentButton;
                if (m_StatImprovePoints > 0)
                {
                    l_PanelButtonStat.addedStatValue += 1;
                    statImprovePoints -= 1;
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

        foreach (string l_key in l_PlayerStats.Keys)
        {
            PanelButtonStat l_PanelButton = Instantiate(PanelButtonStat.prefab);

            l_PanelButton.title = l_key;
            l_PanelButton.statId = l_key;
            l_PanelButton.statValue = PlayerStat.GetInstance().GetStats()[l_key];
            l_PanelButton.text.fontSize = 40;

            m_StatsButtonList.AddButton(l_PanelButton);
        }
    }

    private void InitSpecials()
    {
        for (int i = 0; i < 12; i++)
        {
            PanelButton l_PanelButton = CreateSpecialButton();

            l_PanelButton.title = "SP" + (i + 1);
            l_PanelButton.text.fontSize = 40;

            m_SpecialsButtonList.AddButton(l_PanelButton);
        }
    }

    private PanelButton CreateSpecialButton()
    {
        PanelButton l_PanelButton = Instantiate(PanelButton.prefab);

        l_PanelButton.transform.SetParent(m_SpecialsButtonList.transform);
        l_PanelButton.transform.localPosition = Vector3.zero;
        l_PanelButton.transform.localScale = Vector3.one;
        l_PanelButton.AddAction(SelectSpecial);

        return l_PanelButton;
    }

    private void ActiveSpecialListPanel()
    {
        m_CurrentActivePanel = ActivePanels.SpecialList;
        m_ProfileButtonList.isActive = false;
        m_SpecialsButtonList.isActive = true;
    }

    private void ActiveProfilePanel()
    {
        m_CurrentActivePanel = ActivePanels.Profile;
        m_ProfileButtonList.isActive = true;
        m_SpecialsButtonList.isActive = false;
        m_StatsButtonList.isActive = false;
    }

    private void ActiveStatsPanel()
    {
        m_CurrentActivePanel = ActivePanels.Stats;
        m_StatsButtonList.isActive = true;
        m_ProfileButtonList.isActive = false;
    }

    private void ShowSpecialDescription()
    {
        m_SpecialDescriptionText.text = m_SpecialsButtonList.currentButton.title + " description";

        if (m_SpecialsButtonList.currentButton.text.color == Color.green)
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
        if (m_SpecialsButtonList.currentButton.text.color != Color.green)
        {
            if (m_SelectedSpecialCount < 5)
            {
                m_SpecialsButtonList.currentButton.text.color = Color.green;
                m_SelectedSpecialCount++;
                m_SpecialStatus.Selected(true);
            }
        }
        else
        {
            m_SpecialsButtonList.currentButton.text.color = Color.black;
            m_SelectedSpecialCount--;
            m_SpecialStatus.Selected(false);
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
    #endregion
}
