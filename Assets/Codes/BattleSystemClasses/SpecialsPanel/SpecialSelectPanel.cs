using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class SpecialSelectPanel : Panel
{
    #region Variables
    private static SpecialSelectPanel m_Instance = null;
    private ChooseEnemyPanel m_ChooseEnemyPanel = null;
    private List<string> m_ChoosedSkills = new List<string>();

    [SerializeField]
    private ButtonList m_SpecialButtonList = null;

    [SerializeField]
    private ButtonList m_ConfirmButtonList = null;

    [SerializeField]
    private ButtonList m_AddedSpecialButtonList = null;
    #endregion

    #region Interface
    public static SpecialSelectPanel prefab
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = Resources.Load<SpecialSelectPanel>("Prefabs/Panels/SelectSpecialPanel");
            }
            return m_Instance;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_ConfirmButtonList.isActive = false;
        m_AddedSpecialButtonList.isActive = false;

        m_ConfirmButtonList[0].AddAction(Confirm);
        m_ConfirmButtonList[1].AddAction(ReturnToMain);

        InitSpecialButtons();

        m_SpecialButtonList.AddKeyArrowAction(CheckScrolling);
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

        if (Input.GetKeyDown(KeyCode.S))
        {
            int l_SpecialCount = m_SpecialButtonList.count - 1;
            int l_CurrentSelectedSpecial = m_SpecialButtonList.currentButtonId;

            int m_delta = m_SpecialButtonList.count % 3;

            float l_Value = 1.0f - (float)l_CurrentSelectedSpecial / ((float)l_SpecialCount - m_delta);

            if (l_Value < 0)
            {
                l_Value = 0;
            }

            ScrollRect l_ScrollRect = m_SpecialButtonList.transform.parent.GetComponent<ScrollRect>();

            Debug.Log("Normilized Position: " + l_Value + ", ScrollRect normilized position: " + l_ScrollRect.verticalNormalizedPosition);
        }
    }
    #endregion

    #region Private
    private void ReturnToMain()
    {
        CancelChoose();

        PanelManager.GetInstance().ClosePanel(this);
    }

    private void ChooseSpecial()
    {
        PanelButtonSpecial l_PanelButton = (PanelButtonSpecial)m_SpecialButtonList.currentButton;

        if (l_PanelButton.isChosen)
        {
            l_PanelButton.Choose(false);
            m_ChoosedSkills.Remove(l_PanelButton.skillId);
            Player.GetInstance().mana += SkillDataBase.GetInstance().GetSkillData(l_PanelButton.skillId).mana;

            m_AddedSpecialButtonList.RemoveButton(l_PanelButton.skillId);
        }
        else
        {
            if (m_ChoosedSkills.Count >=4)
            {
                return;
            }

            l_PanelButton.Choose(true);
            m_ChoosedSkills.Add(l_PanelButton.skillId);
            Player.GetInstance().mana -= SkillDataBase.GetInstance().GetSkillData(l_PanelButton.skillId).mana;

            PanelButtonChosenSpecial l_PanelButtonChosenSpecial = Instantiate(PanelButtonChosenSpecial.prefab);
            l_PanelButtonChosenSpecial.title = LocalizationDataBase.GetInstance().GetText("Skill:" + l_PanelButton.skillId);
            m_AddedSpecialButtonList.AddButton(l_PanelButtonChosenSpecial);
        }
    }

    //TODO Kostil
    private void InitSpecialButtons()
    {
        for (int i = 0; i < 8; i++)
        {
            PanelButtonSpecial l_SpecialButton = Instantiate(PanelButtonSpecial.prefab);
            l_SpecialButton.skillId = "WaterDrops";
            l_SpecialButton.AddAction(ChooseSpecial);
            m_SpecialButtonList.AddButton(l_SpecialButton);
        }

        ReScaleBounds();
    }

    private void Confirm()
    {
        PanelManager.GetInstance().ClosePanel(this);

        m_ChooseEnemyPanel = Instantiate(ChooseEnemyPanel.prefab);
        m_ChooseEnemyPanel.AddChoosedAction(Attack);
        m_ChooseEnemyPanel.AddCancelAction(CancelChoose);
        PanelManager.GetInstance().ShowPanel(m_ChooseEnemyPanel, true, BattleSystem.GetInstance().mainPanelTransform);
    }

    private void Attack()
    {
        PanelManager.GetInstance().ClosePanel(this);

        SpecialUpgradePanel l_SpecialUpgradePanel = Instantiate(SpecialUpgradePanel.prefab);
        l_SpecialUpgradePanel.SetSkills(m_ChoosedSkills);
        l_SpecialUpgradePanel.SetEnemy(m_ChooseEnemyPanel.choosedEnemy);
        PanelManager.GetInstance().ShowPanel(l_SpecialUpgradePanel);
        BattleSystem.GetInstance().SetVisibleAvatarPanel(false);
    }

    private void CancelChoose()
    {
        for (int i = 0; i < m_ChoosedSkills.Count; i++)
        {
            Player.GetInstance().mana += SkillDataBase.GetInstance().GetSkillData(m_ChoosedSkills[i]).mana;
        }
    }

    private void ReScaleBounds()
    {
        if (120.0f * m_SpecialButtonList.count > m_SpecialButtonList.rectTransform.sizeDelta.y)
        {
            Vector2 l_SizeDelta = m_SpecialButtonList.rectTransform.sizeDelta;
            l_SizeDelta.y = 120.0f * m_SpecialButtonList.count;
            m_SpecialButtonList.rectTransform.sizeDelta = l_SizeDelta;
        }
    }

    private void CheckScrolling()
    {
        ScrollRect l_ScrollRect = m_SpecialButtonList.transform.parent.GetComponent<ScrollRect>();

        int l_SpecialCount = m_SpecialButtonList.count - 1;
        int l_CurrentSelectedSpecial = m_SpecialButtonList.currentButtonId;
        
        float l_Value = l_ScrollRect.verticalNormalizedPosition;

        l_CurrentSelectedSpecial /= 3;
        int m_delta = m_SpecialButtonList.count % 3;
        l_Value = 1.0f - ((float)l_CurrentSelectedSpecial * 3) / ((float)l_SpecialCount - m_delta);

        if (l_Value < 0)
        {
            l_Value = 0;
        }

        l_ScrollRect.verticalNormalizedPosition = l_Value;
    }
    #endregion
}
