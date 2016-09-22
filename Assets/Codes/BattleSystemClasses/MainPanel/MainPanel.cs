using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPanel : Panel
{
    #region Variables
    [SerializeField]
    private ButtonList m_ButtonList = null;

    [SerializeField]
    private Transform m_DoughterPanelTransform = null;

    private static MainPanel m_Prefab;
    private ChooseEnemyPanel m_ChoosedEnemyPanel = null;
    #endregion

    #region Interface
    public static MainPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<MainPanel>("Prefabs/Panels/MainPanel");
            }
            return m_Prefab;
        }
    }
    public Transform doughterTransform
    {
        get { return m_DoughterPanelTransform; }
    }

    public override void Awake()
    {
        base.Awake();

        InitButtons();
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
    private void InitButtons()
    {
        m_ButtonList[0].AddAction(Attack);
        m_ButtonList[1].AddAction(Special);
        m_ButtonList[3].AddAction(Retreat);
    }

    private void Attack()
    {
        m_ChoosedEnemyPanel = Instantiate(ChooseEnemyPanel.prefab);
        m_ChoosedEnemyPanel.AddChoosedAction(AttackEnemy);
        BattleSystem.GetInstance().ShowPanel(m_ChoosedEnemyPanel, true, BattleSystem.GetInstance().mainPanelTransform);
    }

    private void Special()
    {
        SpecialSelectPanel l_SpecialSelectPanel = Instantiate(SpecialSelectPanel.prefab);
        BattleSystem.GetInstance().ShowPanel(l_SpecialSelectPanel, true, BattleSystem.GetInstance().mainPanelTransform);
    }

    private void AttackEnemy()
    {
        m_ChoosedEnemyPanel.Close();
        BattlePlayer.GetInstance().Attack(m_ChoosedEnemyPanel.choosedEnemy);
    }

    private void Retreat()
    {
        BattleSystem.GetInstance().Retreat();
    }
    #endregion
}