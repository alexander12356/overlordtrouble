using UnityEngine;
using System.Collections.Generic;

public class SpecialSelectPanel : Panel
{
    private enum ButtonListType
    {
        SpecialList,
        ConfirmList
    }

    #region Variables
    private static SpecialSelectPanel m_Instance = null;

    [SerializeField]
    private ButtonList m_SpecialButtonList = null;

    [SerializeField]
    private ButtonList m_ConfirmButtonList = null;

    [SerializeField]
    private ButtonList m_AddedSpecialButtonList = null;

    private ButtonListType m_CurrentButtonListType = ButtonListType.SpecialList;
    private Dictionary<string, Special> m_SpecialList = new Dictionary<string, Special>();
    private Dictionary<string, Special> m_AddedSpecialList = new Dictionary<string, Special>();
    private ChooseEnemyPanel m_ChooseEnemyPanel = null;
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

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
        {
            return;
        }

        if (m_CurrentButtonListType == ButtonListType.SpecialList && Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_CurrentButtonListType = ButtonListType.ConfirmList;
            m_SpecialButtonList.isActive = false;
            m_ConfirmButtonList.isActive = true;
        }
        if (m_CurrentButtonListType == ButtonListType.ConfirmList && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_CurrentButtonListType = ButtonListType.SpecialList;
            m_SpecialButtonList.isActive = true;
            m_ConfirmButtonList.isActive = false;
        }

        switch (m_CurrentButtonListType)
        {
            case ButtonListType.SpecialList:
                m_SpecialButtonList.UpdateKey();
                break;
            case ButtonListType.ConfirmList:
                m_ConfirmButtonList.UpdateKey();
                break;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (m_CurrentButtonListType == ButtonListType.ConfirmList)
            {
                m_CurrentButtonListType = ButtonListType.SpecialList;
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
    private void Awake()
    {
        m_ConfirmButtonList.isActive = false;
        m_AddedSpecialButtonList.isActive = false;

        m_ConfirmButtonList[0].AddAction(Confirm);
        m_ConfirmButtonList[1].AddAction(ReturnToMain);

        InitSpecialList();
        InitSpecialButtons();
    }

    private void ReturnToMain()
    {
        foreach (Special l_Special in m_AddedSpecialList.Values)
        {
            Player.GetInstance().mana += l_Special.mana;
        }

        PanelManager.GetInstance().ClosePanel(this);
    }

    private void SelectSpecial()
    {
        PanelButton l_PanelButton = m_SpecialButtonList.currentButton;

        if (m_AddedSpecialList.ContainsKey(l_PanelButton.title))
        {
            m_AddedSpecialList.Remove(l_PanelButton.title);
            m_AddedSpecialButtonList.RemoveButton(l_PanelButton.title);

            Player.GetInstance().mana += m_SpecialList[l_PanelButton.title].mana;
        }
        else if (Player.GetInstance().mana - m_SpecialList[l_PanelButton.title].mana >= 0)
        {
            m_AddedSpecialList.Add(l_PanelButton.title, m_SpecialList[l_PanelButton.title]);

            PanelButton l_NewButton = Instantiate(PanelButton.prefab);
            l_NewButton.title = l_PanelButton.title;
            m_AddedSpecialButtonList.AddButton(l_NewButton);

            Player.GetInstance().mana -= m_SpecialList[l_PanelButton.title].mana;
        }
    }

    //TODO Kostil
    private void InitSpecialList()
    {
        for (int  i = 0; i < 4; i++)
        {
            Special l_Special = new Special("SP" + i);
            m_SpecialList.Add(l_Special.id, l_Special);
        }
    }

    //TODO Kostil
    private void InitSpecialButtons()
    {
        foreach (string p_SpecialId in m_SpecialList.Keys)
        {
            PanelButton l_SpecialButton = Instantiate(PanelButton.prefab);
            l_SpecialButton.title = p_SpecialId;
            l_SpecialButton.AddAction(SelectSpecial);
            m_SpecialButtonList.AddButton(l_SpecialButton);
        }
    }

    private void Confirm()
    {
        PanelManager.GetInstance().ClosePanel(this);

        m_ChooseEnemyPanel = Instantiate(ChooseEnemyPanel.prefab);
        m_ChooseEnemyPanel.AddChoosedAction(Attack);
        PanelManager.GetInstance().ShowPanel(m_ChooseEnemyPanel);
    }

    private void Attack()
    {
        PanelManager.GetInstance().ClosePanel(this);

        SpecialUpgradePanel l_SpecialUpgradePanel = Instantiate(SpecialUpgradePanel.prefab);
        l_SpecialUpgradePanel.SetSpecials(m_AddedSpecialList);
        l_SpecialUpgradePanel.SetEnemy(m_ChooseEnemyPanel.choosedEnemy);
        PanelManager.GetInstance().ShowPanel(l_SpecialUpgradePanel);
    }
    #endregion
}
