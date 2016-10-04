using UnityEngine;
using System.Collections.Generic;

public class ChooseEnemyPanel : Panel
{
    #region Variables
    private enum ButtonListType
    {
        EnemyList,
        ConfirmList
    }

    private List<BattleEnemy> m_EnemyList;

    [SerializeField]
    private ButtonList m_EnemyButtonList = null;

    [SerializeField]
    private ButtonList m_ConfirmButtonList = null;

    private static ChooseEnemyPanel m_Prefab = null;
    private BattleEnemy m_ChoosedEnemy = null;
    private PanelActionHandler m_ConfirmAction = null;
    private PanelActionHandler m_CancelAction = null;
    private ButtonListType m_CurrentButtonListType = ButtonListType.EnemyList;
    #endregion

    #region Interface
    public static ChooseEnemyPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<ChooseEnemyPanel>("Prefabs/Panels/ChooseEnemyPanel");
            }
            return m_Prefab;
        }
    }
    public BattleEnemy choosedEnemy
    {
        get { return m_ChoosedEnemy;  }
    }

    public override void Awake()
    {
        base.Awake();

        InitEnemyList();
        m_ConfirmButtonList.isActive = false;
        m_ConfirmButtonList[0].AddAction(Confirm);
        m_ConfirmButtonList[0].title = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Select");
        m_ConfirmButtonList[1].AddAction(Cancel);
        m_ConfirmButtonList[1].title = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:Cancel");

        m_EnemyButtonList.AddKeyArrowAction(SelectEnemyAvatar);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
        {
            return;
        }

        if (m_CurrentButtonListType == ButtonListType.EnemyList)
        {
            m_EnemyButtonList.UpdateKey();
        }
        else if (m_CurrentButtonListType == ButtonListType.ConfirmList)
        {
            m_ConfirmButtonList.UpdateKey();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Cancel();
        }
    }

    public void AddChoosedAction(PanelActionHandler p_Action)
    {
        m_ConfirmAction += p_Action;
    }

    public void RemoveChoosedAction(PanelActionHandler p_Action)
    {
        m_ConfirmAction -= p_Action;
    }

    public void AddCancelAction(PanelActionHandler p_Action)
    {
        m_CancelAction += p_Action;
    }

    public void RemoveCancelAction(PanelActionHandler p_Action)
    {
        m_CancelAction -= p_Action;
    }

    #endregion

    #region Private
    private void InitEnemyList()
    {
        m_EnemyList = BattleSystem.GetInstance().GetEnemyList();

        for (int i = 0; i < m_EnemyList.Count; i++)
        {
			PanelButtonEnemyChoise l_PanelButton = Instantiate (PanelButtonEnemyChoise.prefab);
            l_PanelButton.title = m_EnemyList[i].actorName + " " + m_EnemyList[i].health + "/" + m_EnemyList[i].baseHealth;
			l_PanelButton.titleSizeW = 500;

            l_PanelButton.AddAction(Choose);

			m_EnemyButtonList.AddButton(l_PanelButton);
			l_PanelButton.transform.localPosition = new Vector3 (300f, gameObject.transform.localPosition.y - 50f, 0.0f);

        }

        m_EnemyList[0].selected = true;
    }    

    private void Choose()
    {
        m_ChoosedEnemy = m_EnemyList[m_EnemyButtonList.currentButtonId];

        m_EnemyButtonList.isActive = false;
        m_ConfirmButtonList.isActive = true;
        m_CurrentButtonListType = ButtonListType.ConfirmList;

        m_EnemyButtonList.currentButton.selected = true;
    }

    private void SelectEnemyAvatar()
    {
        m_EnemyList[m_EnemyButtonList.prevButtonId].selected = false;
        m_EnemyList[m_EnemyButtonList.currentButtonId].selected = true;
    }

    private void Cancel()
    {
        if (m_CurrentButtonListType == ButtonListType.ConfirmList)
        {
            m_CurrentButtonListType = ButtonListType.EnemyList;
            m_ConfirmButtonList.isActive = false;
            m_EnemyButtonList.isActive = true;
        }
        else
        {
            m_EnemyList[m_EnemyButtonList.currentButtonId].selected = false;
            CancelAction();
            Close();
        }
    }

    private void ChoosedAction()
    {
        if (m_ConfirmAction != null)
        {
            m_ConfirmAction();
        }
    }

    private void Confirm()
    {
        m_EnemyList[m_EnemyButtonList.currentButtonId].selected = false;
        ChoosedAction();
    }

    private void CancelAction()
    {
        if (m_CancelAction != null)
        {
            m_CancelAction();
        }
    }
    #endregion
}
