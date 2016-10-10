using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

public class SpecialSelectPanel : Panel
{
    #region Variables
    private static SpecialSelectPanel m_Prefab = null;
    private ChooseEnemyPanel m_ChooseEnemyPanel = null;
    private List<string> m_ChoosedSkills = new List<string>();

    [SerializeField]
    private ButtonList m_SpecialButtonList = null;

    [SerializeField]
    private ButtonList m_ConfirmButtonList = null;

    [SerializeField]
    private ButtonList m_AddedSpecialButtonList = null;

    [SerializeField]
    private ButtonListScrolling m_ButtonListScrolling = null;
    #endregion

    #region Interface
    public static SpecialSelectPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<SpecialSelectPanel>("Prefabs/Panels/SelectSpecialPanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_ConfirmButtonList.isActive = false;
        m_AddedSpecialButtonList.isActive = false;

        m_ConfirmButtonList[0].AddAction(Confirm);
        m_ConfirmButtonList[0].title = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:UseSpecials");
        m_ConfirmButtonList[1].AddAction(ReturnToMain);
        m_ConfirmButtonList[1].title = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Cancel");

        InitSpecialButtons();

        m_ButtonListScrolling.Init(120.0f, 3);
        m_SpecialButtonList.AddKeyArrowAction(m_ButtonListScrolling.CheckScrolling);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
        {
            return;
        }

        if (m_SpecialButtonList.isActive && Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_SpecialButtonList.isActive = false;
            m_ConfirmButtonList.isActive = true;
        }
        if (m_ConfirmButtonList && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_SpecialButtonList.isActive = true;
            m_ConfirmButtonList.isActive = false;
        }

        m_SpecialButtonList.UpdateKey();
        m_ConfirmButtonList.UpdateKey();

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (m_ConfirmButtonList.isActive)
            {
                m_SpecialButtonList.isActive = true;
                m_ConfirmButtonList.isActive = false;
            }
            else
            {
                ReturnToMain();
            }
        }
    }
    #endregion

    #region Private
    private void ReturnToMain()
    {
        CancelChoose();

        Close();
    }

    private void ChooseSpecial()
    {
        PanelButtonSpecial l_PanelButton = (PanelButtonSpecial)m_SpecialButtonList.currentButton;

        if (l_PanelButton.isChosen)
        {
            l_PanelButton.Choose(false);
            m_ChoosedSkills.Remove(l_PanelButton.skillId);
            BattlePlayer.GetInstance().mana += SkillDataBase.GetInstance().GetSkillData(l_PanelButton.skillId).mana;

            m_AddedSpecialButtonList.RemoveButton(LocalizationDataBase.GetInstance().GetText("Skill:" + l_PanelButton.skillId));
        }
        else
        {
            if (m_ChoosedSkills.Count >= 4 || BattlePlayer.GetInstance().mana < SkillDataBase.GetInstance().GetSkillData(l_PanelButton.skillId).mana)
            {
                return;
            }

            l_PanelButton.Choose(true);
            m_ChoosedSkills.Add(l_PanelButton.skillId);
            BattlePlayer.GetInstance().mana -= SkillDataBase.GetInstance().GetSkillData(l_PanelButton.skillId).mana;

            PanelButtonChosenSpecial l_PanelButtonChosenSpecial = Instantiate(PanelButtonChosenSpecial.prefab);
            l_PanelButtonChosenSpecial.title = LocalizationDataBase.GetInstance().GetText("Skill:" + l_PanelButton.skillId);
            m_AddedSpecialButtonList.AddButton(l_PanelButtonChosenSpecial);
        }
    }

    //TODO Kostil
    private void InitSpecialButtons()
    {
        if (BattleSystem.GetInstance().battleData.id.Contains("TestBattle"))
        {
            InitTestSpecials();
        }
        else
        {
            InitSpecials();
        }
    }

    private void Confirm()
    {
        m_ChooseEnemyPanel = Instantiate(ChooseEnemyPanel.prefab);
        m_ChooseEnemyPanel.AddChoosedAction(Attack);
        m_ChooseEnemyPanel.AddCancelAction(CancelChoose);
        BattleSystem.GetInstance().ShowPanel(m_ChooseEnemyPanel, true, BattleSystem.GetInstance().mainPanelTransform);
    }

    private void Attack()
    {
        Close();
        m_ChooseEnemyPanel.Close();

        SpecialUpgradePanel l_SpecialUpgradePanel = Instantiate(SpecialUpgradePanel.prefab);
        l_SpecialUpgradePanel.SetSkills(m_ChoosedSkills);
        l_SpecialUpgradePanel.SetEnemy(m_ChooseEnemyPanel.choosedEnemy);
        BattleSystem.GetInstance().ShowPanel(l_SpecialUpgradePanel);
        BattleSystem.GetInstance().SetVisibleAvatarPanel(false);
    }

    private void CancelChoose()
    {
        for (int i = 0; i < m_ChoosedSkills.Count; i++)
        {
            BattlePlayer.GetInstance().mana += SkillDataBase.GetInstance().GetSkillData(m_ChoosedSkills[i]).mana;
        }
    }

    private void InitTestSpecials()
    {
        for (int i = 0; i < 1; i++)
        {
            PanelButtonSpecial l_SpecialButton = Instantiate(PanelButtonSpecial.prefab);
            l_SpecialButton.skillId = "WaterDrops";
            l_SpecialButton.AddAction(ChooseSpecial);
            m_SpecialButtonList.AddButton(l_SpecialButton);
        }
        for (int i = 0; i < 1; i++)
        {
            PanelButtonSpecial l_SpecialButton = Instantiate(PanelButtonSpecial.prefab);
            l_SpecialButton.skillId = "Slap";
            l_SpecialButton.AddAction(ChooseSpecial);
            m_SpecialButtonList.AddButton(l_SpecialButton);
        }
        for (int i = 0; i < 1; i++)
        {
            PanelButtonSpecial l_SpecialButton = Instantiate(PanelButtonSpecial.prefab);
            l_SpecialButton.skillId = "TailWhap";
            l_SpecialButton.AddAction(ChooseSpecial);
            m_SpecialButtonList.AddButton(l_SpecialButton);
        }
        for (int i = 0; i < 1; i++)
        {
            PanelButtonSpecial l_SpecialButton = Instantiate(PanelButtonSpecial.prefab);
            l_SpecialButton.skillId = "Slash";
            l_SpecialButton.AddAction(ChooseSpecial);
            m_SpecialButtonList.AddButton(l_SpecialButton);
        }
    }

    private void InitSpecials()
    {
        List<SkillData> l_PlayerSkills = PlayerData.GetInstance().GetSkills();
        for (int i = 0; i < l_PlayerSkills.Count; i++)
        {
            PanelButtonSpecial l_SpecialButton = Instantiate(PanelButtonSpecial.prefab);
            l_SpecialButton.skillId = l_PlayerSkills[i].id;
            l_SpecialButton.AddAction(ChooseSpecial);
            m_SpecialButtonList.AddButton(l_SpecialButton);
        }
    }
    #endregion
}
