using UnityEngine;
using System.Collections.Generic;

public class ChooseEnemyPanel : Panel
{
    #region Variables
    private List<Enemy> m_EnemyList;

    [SerializeField]
    private ButtonList m_ButtonList = null;

    private static ChooseEnemyPanel m_Prefab = null;
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

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
        {
            return;
        }

        m_ButtonList.UpdateKey();
    }
    #endregion

    #region Private
    private void Awake()
    {
        InitEnemyList();

        m_ButtonList.AddKeyArrowAction(SelectEnemy);
    }

    private void InitEnemyList()
    {
        m_EnemyList = EnemyManager.GetInstance().GetEnemy();

        for (int i = 0; i < m_EnemyList.Count; i++)
        {
            PanelButton l_PanelButton = Instantiate(PanelButton.prefab);
            l_PanelButton.title = m_EnemyList[i].actorName + " " + m_EnemyList[i].health + "/" + m_EnemyList[i].baseHealth;
            l_PanelButton.AddAction(Choose);

            m_ButtonList.AddButton(l_PanelButton);
        }

        m_EnemyList[0].selected = true;
    }    

    private void Choose()
    {
        PanelManager.GetInstance().ClosePanel(this);
        Player.GetInstance().Attack(m_EnemyList[m_ButtonList.currentButtonId]);
    }

    private void SelectEnemy()
    {
        m_EnemyList[m_ButtonList.prevButtonId].selected = false;
        m_EnemyList[m_ButtonList.currentButtonId].selected = true;
    }

    public override void PopAction()
    {
        base.PopAction();

        m_EnemyList[m_ButtonList.currentButtonId].selected = false;
    }
    #endregion
}
