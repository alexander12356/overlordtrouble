using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

public class MonstyleSelectPanel : Panel
{
    #region Variables
    private static MonstyleSelectPanel m_Prefab = null;
    private ChooseEnemyPanel m_ChooseEnemyPanel = null;
    private List<string> m_ChoosedSkills = new List<string>();

    [SerializeField]
    private ButtonList m_MonstyleButtonList = null;

    [SerializeField]
    private ButtonList m_ConfirmButtonList = null;

    [SerializeField]
    private ButtonList m_AddedMonstyleButtonList = null;

    [SerializeField]
    private ButtonListScrolling m_ButtonListScrolling = null;
    #endregion

    #region Interface
    public static MonstyleSelectPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<MonstyleSelectPanel>("Prefabs/Panels/MonstyleSelectPanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_ConfirmButtonList.isActive = false;
        m_AddedMonstyleButtonList.isActive = false;

        m_ConfirmButtonList[0].AddAction(Confirm);
        m_ConfirmButtonList[0].title = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:UseSpecials");
        m_ConfirmButtonList[1].AddAction(ReturnToMain);
        m_ConfirmButtonList[1].title = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Cancel");

        InitMonstyleButtons();

        m_ButtonListScrolling.Init(120.0f, 3);
        m_MonstyleButtonList.AddKeyArrowAction(m_ButtonListScrolling.CheckScrolling);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
        {
            return;
        }

        if (m_MonstyleButtonList.isActive && Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_MonstyleButtonList.isActive = false;
            m_ConfirmButtonList.isActive = true;
        }
        if (m_ConfirmButtonList && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_MonstyleButtonList.isActive = true;
            m_ConfirmButtonList.isActive = false;
        }

        m_MonstyleButtonList.UpdateKey();
        m_ConfirmButtonList.UpdateKey();

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (m_ConfirmButtonList.isActive)
            {
                m_MonstyleButtonList.isActive = true;
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
        ResetChoosedSkills();

        Close();
    }

    private void ChooseSpecial()
    {
        PanelButtonMonstyle l_PanelButton = (PanelButtonMonstyle)m_MonstyleButtonList.currentButton;

        if (l_PanelButton.isChosen)
        {
            l_PanelButton.Choose(false);
            m_ChoosedSkills.Remove(l_PanelButton.monstyleId);
            BattlePlayer.GetInstance().mana += MonstyleDataBase.GetInstance().GetMonstyleData(l_PanelButton.monstyleId).sp;

            m_AddedMonstyleButtonList.RemoveButton(LocalizationDataBase.GetInstance().GetText("Skill:" + l_PanelButton.monstyleId));
        }
        else
        {
            if (m_ChoosedSkills.Count >= 4 || BattlePlayer.GetInstance().mana < MonstyleDataBase.GetInstance().GetMonstyleData(l_PanelButton.monstyleId).sp)
            {
                return;
            }

            l_PanelButton.Choose(true);
            m_ChoosedSkills.Add(l_PanelButton.monstyleId);
            BattlePlayer.GetInstance().mana -= MonstyleDataBase.GetInstance().GetMonstyleData(l_PanelButton.monstyleId).sp;

            PanelButtonChosenSpecial l_PanelButtonChosenSpecial = Instantiate(PanelButtonChosenSpecial.prefab);
            l_PanelButtonChosenSpecial.title = LocalizationDataBase.GetInstance().GetText("Skill:" + l_PanelButton.monstyleId);
            m_AddedMonstyleButtonList.AddButton(l_PanelButtonChosenSpecial);
        }
    }

    private void InitMonstyleButtons()
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
        AddPopAction(ShowChooseEnemy);
        Close();
    }

    private void ShowChooseEnemy()
    {
        m_ChooseEnemyPanel = Instantiate(ChooseEnemyPanel.prefab);
        m_ChooseEnemyPanel.AddChoosedAction(Attack);
        m_ChooseEnemyPanel.AddCancelAction(CancelChooseEnemy);
        BattleSystem.GetInstance().ShowPanel(m_ChooseEnemyPanel, true, BattleSystem.GetInstance().mainPanelTransform);
    }

    private void Attack()
    {
        ShowUpgradePanel();
    }

    private void CancelChooseEnemy()
    {
        ResetChoosedSkills();

        MonstyleSelectPanel l_MonstyleSelectPanel = Instantiate(prefab);
        BattleSystem.GetInstance().ShowPanel(l_MonstyleSelectPanel, true, BattleSystem.GetInstance().mainPanelTransform);
    }

    private void ResetChoosedSkills()
    {
        for (int i = 0; i < m_ChoosedSkills.Count; i++)
        {
            BattlePlayer.GetInstance().mana += MonstyleDataBase.GetInstance().GetMonstyleData(m_ChoosedSkills[i]).sp;
        }
        m_ChoosedSkills.Clear();
    }

    private void InitTestSpecials()
    {
        for (int i = 0; i < 1; i++)
        {
            PanelButtonMonstyle l_MonstyleButton = Instantiate(PanelButtonMonstyle.prefab);
            l_MonstyleButton.monstyleId = "WaterDrops";
            l_MonstyleButton.AddAction(ChooseSpecial);
            m_MonstyleButtonList.AddButton(l_MonstyleButton);
        }
        for (int i = 0; i < 1; i++)
        {
            PanelButtonMonstyle l_MonstyleButton = Instantiate(PanelButtonMonstyle.prefab);
            l_MonstyleButton.monstyleId = "Slap";
            l_MonstyleButton.AddAction(ChooseSpecial);
            m_MonstyleButtonList.AddButton(l_MonstyleButton);
        }
        for (int i = 0; i < 1; i++)
        {
            PanelButtonMonstyle l_MonstyleButton = Instantiate(PanelButtonMonstyle.prefab);
            l_MonstyleButton.monstyleId = "TailWhap";
            l_MonstyleButton.AddAction(ChooseSpecial);
            m_MonstyleButtonList.AddButton(l_MonstyleButton);
        }
        for (int i = 0; i < 1; i++)
        {
            PanelButtonMonstyle l_MonstyleButton = Instantiate(PanelButtonMonstyle.prefab);
            l_MonstyleButton.monstyleId = "Slash";
            l_MonstyleButton.AddAction(ChooseSpecial);
            m_MonstyleButtonList.AddButton(l_MonstyleButton);
        }
    }

    private void InitSpecials()
    {
        List<MonstyleData> l_PlayerSkills = PlayerData.GetInstance().GetSkills();
        for (int i = 0; i < l_PlayerSkills.Count; i++)
        {
            PanelButtonMonstyle l_SpecialButton = Instantiate(PanelButtonMonstyle.prefab);
            l_SpecialButton.monstyleId = l_PlayerSkills[i].id;
            l_SpecialButton.AddAction(ChooseSpecial);
            m_MonstyleButtonList.AddButton(l_SpecialButton);
        }
    }

    private void ShowUpgradePanel()
    {
        MonstyleUpgradePanel l_SpecialUpgradePanel = Instantiate(MonstyleUpgradePanel.prefab);
        l_SpecialUpgradePanel.SetSkills(m_ChoosedSkills);
        l_SpecialUpgradePanel.SetEnemy(m_ChooseEnemyPanel.choosedEnemy);
        BattleSystem.GetInstance().ShowPanel(l_SpecialUpgradePanel);
        BattleSystem.GetInstance().SetVisibleAvatarPanel(false);
    }
    #endregion
}
