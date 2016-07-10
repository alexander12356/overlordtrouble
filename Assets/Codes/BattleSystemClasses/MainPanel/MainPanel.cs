using UnityEngine;

public class MainPanel : Panel
{
    #region Variables
    [SerializeField]
    private ButtonList m_ButtonList = null;

    private static MainPanel m_Prefab;
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
        InitButtons();
    }

    private void InitButtons()
    {
        m_ButtonList[0].AddAction(Attack);
        m_ButtonList[1].AddAction(Special);
    }

    private void Attack()
    {
        ChooseEnemyPanel l_ChooseEnemyPanel = Instantiate(ChooseEnemyPanel.prefab);
        PanelManager.GetInstance().ShowPanel(l_ChooseEnemyPanel);
    }

    private void Special()
    {
        SpecialSelectPanel l_SpecialSelectPanel = Instantiate(SpecialSelectPanel.prefab);
        PanelManager.GetInstance().ShowPanel(l_SpecialSelectPanel);
    }
    #endregion
}