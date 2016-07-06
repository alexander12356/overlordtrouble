using UnityEngine;

public class MainPanel : Panel
{
    #region Variables
    [SerializeField]
    private ButtonList m_ButtonList = null;

    #endregion   

    #region Interface
    
    #endregion

    #region Private
    private void Awake()
    {
        InitButtons();
    }

    private void InitButtons()
    {
        m_ButtonList[0].AddAction(Attack);
    }

    private void Attack()
    {
        //Player.GetInstance().Attack();
        TextPanel l_TextPanel = Instantiate(PanelManager.GetInstance().textPanelPrefab);
        PanelManager.GetInstance().ShowPanel(l_TextPanel);
        l_TextPanel.SetText("Вы житель мелкого поселения монстров, уже давно как не было войны с людьми и ваше поселение смогло даже наладить торговые пути с некоторыми людскими поселками, однако многие другие люди все еще не доверяют вам как и многие другие монстры не доверяют им.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_ButtonList.SelectMoveUp();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_ButtonList.SelectMoveDown();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z))
        {
            m_ButtonList.Action();
        }
    }
    #endregion
}