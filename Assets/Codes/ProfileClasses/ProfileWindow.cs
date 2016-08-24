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

    [SerializeField]
    private ButtonList m_SpecialsButtonList = null;

    [SerializeField]
    private ButtonList m_StatsButtonList = null;

    [SerializeField]
    private Text m_SpecialDescriptionText = null;

    [SerializeField]
    private SpecialStatus m_SpecialStatus = null;

    #region Interface
    public void Awake ()
    {
        m_ProfileButtonList = GetComponent<ButtonList>();
        m_ProfileButtonList[0].AddAction(ActiveStatsPanel);
        m_ProfileButtonList[1].AddAction(StartImprove);
        m_ProfileButtonList[2].AddAction(ActiveSpecialListPanel);

        m_SpecialsButtonList.isActive = false;
        m_SpecialsButtonList.AddKeyArrowAction(ShowSpecialDescription);
        m_StatsButtonList.isActive = false;

        InitSpecials();
    }
	
	public void Update ()
    {
        switch (m_CurrentActivePanel)
        {
            case ActivePanels.Profile:
                m_ProfileButtonList.UpdateKey();
                break;
            case ActivePanels.SpecialList:
                m_SpecialsButtonList.UpdateKey();
                break;
            case ActivePanels.Stats:
                m_StatsButtonList.UpdateKey();
                break;
        }        

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("DemoMainScene");
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            switch (m_CurrentActivePanel)
            {
                case ActivePanels.Profile:
                    break;
                case ActivePanels.SpecialList:
                    ActiveProfilePanel();
                    break;
                case ActivePanels.Stats:
                    ActiveProfilePanel();
                    break;
            }
        }
    }
    #endregion

    #region Private
    private void InitSpecials()
    {
        for (int i = 0; i < 10; i++)
        {
            PanelButton l_PanelButton = CreateSpecialButton();
            l_PanelButton.title = "SP" + (i + 1);

            Vector3 l_Position = l_PanelButton.transform.localPosition;
            l_Position.y -= i * 10;
            l_PanelButton.transform.localPosition = l_Position;

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

    private void StartImprove()
    {
        SceneManager.LoadScene("Improve");
    }
    #endregion
}
