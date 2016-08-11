﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileWindow : MonoBehaviour
{
    private enum ActivePanels
    {
        Profile,
        Stats,
        SpecialList
    }

    private ButtonList m_ButtonList;
    private ActivePanels m_CurrentActivePanel = ActivePanels.Profile;

    [SerializeField]
    private ButtonList m_SpecialsButtonList = null;

    [SerializeField]
    private ButtonList m_StatsButtonList = null;

	public void Awake ()
    {
        m_ButtonList = GetComponent<ButtonList>();
        m_ButtonList[0].AddAction(ActiveStatsPanel);
        m_ButtonList[2].AddAction(ActiveSpecialListPanel);

        m_SpecialsButtonList.isActive = false;
        m_StatsButtonList.isActive = false;

        InitSpecials();
    }
	
	public void Update ()
    {
        switch (m_CurrentActivePanel)
        {
            case ActivePanels.Profile:
                m_ButtonList.UpdateKey();
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

        return l_PanelButton;
    }

    private void ActiveSpecialListPanel()
    {
        m_CurrentActivePanel = ActivePanels.SpecialList;
        m_ButtonList.isActive = false;
        m_SpecialsButtonList.isActive = true;
    }

    private void ActiveProfilePanel()
    {
        m_CurrentActivePanel = ActivePanels.Profile;
        m_ButtonList.isActive = true;
        m_SpecialsButtonList.isActive = false;
        m_StatsButtonList.isActive = false;
    }

    private void ActiveStatsPanel()
    {
        m_CurrentActivePanel = ActivePanels.Stats;
        m_StatsButtonList.isActive = true;
        m_ButtonList.isActive = false;
    }
}
