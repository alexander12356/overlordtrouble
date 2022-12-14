using System.Collections.Generic;

using UnityEngine;

public class CreatingMonstylePanel : Panel
{
    #region Variables
    private static CreatingMonstylePanel m_Prefab = null;
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
    public static CreatingMonstylePanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<CreatingMonstylePanel>("Prefabs/Panels/CreatingMonstylePanel");
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

        InitSpecialsButtons();

        m_ButtonListScrolling.Init(120.0f, 3);
        m_SpecialButtonList.AddKeyArrowAction(m_ButtonListScrolling.CheckScrolling);

        m_ConfirmButtonList.AddCancelAction(TryExit);
        m_SpecialButtonList.AddCancelAction(TryExit);
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
    }

    private void TryExit()
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
    #endregion

    #region Private
    private void ReturnToMain()
    {
        ResetChoosedSkills();

        Close();
    }

    private void ChooseSpecial()
    {
        PanelButtonSpecial l_PanelButton = (PanelButtonSpecial)m_SpecialButtonList.currentButton;

        if (l_PanelButton.isChosen)
        {
            l_PanelButton.Choose(false);
            m_ChoosedSkills.Remove(l_PanelButton.specialId);
            BattlePlayer.GetInstance().specialPoints += SpecialDataBase.GetInstance().GetSpecialData(l_PanelButton.specialId).sp;

            m_AddedSpecialButtonList.RemoveButton(LocalizationDataBase.GetInstance().GetText("Special:" + l_PanelButton.specialId));
        }
        else
        {
            if (m_ChoosedSkills.Count >= BattlePlayer.GetInstance().monstyleCapacity || BattlePlayer.GetInstance().specialPoints < SpecialDataBase.GetInstance().GetSpecialData(l_PanelButton.specialId).sp)
            {
                return;
            }

            l_PanelButton.Choose(true);
            m_ChoosedSkills.Add(l_PanelButton.specialId);
            BattlePlayer.GetInstance().specialPoints -= SpecialDataBase.GetInstance().GetSpecialData(l_PanelButton.specialId).sp;

            PanelButtonChosenSpecial l_PanelButtonChosenSpecial = Instantiate(PanelButtonChosenSpecial.prefab);
            l_PanelButtonChosenSpecial.title = LocalizationDataBase.GetInstance().GetText("Special:" + l_PanelButton.specialId);
            m_AddedSpecialButtonList.AddButton(l_PanelButtonChosenSpecial);
        }
    }

    private void InitSpecialsButtons()
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
        if (m_ChoosedSkills.Count > 0)
        {
            AddPopAction(ShowChooseEnemy);
            Close();
        }
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
        BattleSystem.GetInstance().CloseMainPanel();
        BattleSystem.GetInstance().mainPanel.AddPopAction(ShowUpgradePanel);
    }

    private void CancelChooseEnemy()
    {
        ResetChoosedSkills();

        CreatingMonstylePanel l_MonstyleSelectPanel = Instantiate(prefab);
        BattleSystem.GetInstance().ShowPanel(l_MonstyleSelectPanel, true, BattleSystem.GetInstance().mainPanelTransform);
    }

    private void ResetChoosedSkills()
    {
        for (int i = 0; i < m_ChoosedSkills.Count; i++)
        {
            BattlePlayer.GetInstance().specialPoints += SpecialDataBase.GetInstance().GetSpecialData(m_ChoosedSkills[i]).sp;
        }
        m_ChoosedSkills.Clear();
    }

    private void InitTestSpecials()
    {
        CreateSpecialButton("WaterDrops");
        CreateSpecialButton("Slap");
        CreateSpecialButton("ProtectiveShell");

        CreateSpecialButton("WaterBullet");
        CreateSpecialButton("WaterDash");
        CreateSpecialButton("HealingSplash");

        CreateSpecialButton("TailWhap");
        CreateSpecialButton("Slash");
        CreateSpecialButton("Molting");

        CreateSpecialButton("StaffStrike");
        CreateSpecialButton("Ember");
        CreateSpecialButton("Concentration");

        CreateSpecialButton("Playercrush");
        CreateSpecialButton("Pillarstones");
        CreateSpecialButton("Refresh");

        CreateSpecialButton("WaterFusillade");
        CreateSpecialButton("IceHarpoon");
        CreateSpecialButton("IceShield");

        CreateSpecialButton("BreathOfEarth");
        CreateSpecialButton("RammingAttack");
        CreateSpecialButton("GroundPower");

        CreateSpecialButton("BigClaw");
        CreateSpecialButton("WaveOfDarkness");
        CreateSpecialButton("MirrorOfTheAbyss");

        CreateSpecialButton("PoisonedDagger");
        CreateSpecialButton("ManaBeam");
        CreateSpecialButton("Accelerate");

        CreateSpecialButton("MagicPunch");
        CreateSpecialButton("MagicSphere");
        CreateSpecialButton("Cleanliness");
    }

    private void InitSpecials()
    {
        List<SpecialData> l_PlayerSkills = PlayerData.GetInstance().GetSelectedSkills();
        for (int i = 0; i < l_PlayerSkills.Count; i++)
        {
            CreateSpecialButton(l_PlayerSkills[i].id);
        }
    }

    private void CreateSpecialButton(string p_Id)
    {
        PanelButtonSpecial l_SpecialButton = Instantiate(PanelButtonSpecial.prefab);
        l_SpecialButton.specialId = p_Id;
        l_SpecialButton.AddAction(ChooseSpecial);
        m_SpecialButtonList.AddButton(l_SpecialButton);
    }

    private void ShowUpgradePanel()
    {
        UsingMonstylePanel l_SpecialUpgradePanel = Instantiate(UsingMonstylePanel.prefab);
        l_SpecialUpgradePanel.SetSkills(m_ChoosedSkills);
        l_SpecialUpgradePanel.SetEnemy(m_ChooseEnemyPanel.choosedEnemy);
        BattleSystem.GetInstance().ShowPanel(l_SpecialUpgradePanel);
        BattleSystem.GetInstance().SetVisibleAvatarPanel(false);
    }
    #endregion
}
