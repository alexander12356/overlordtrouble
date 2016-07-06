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
        Player.GetInstance().Attack();
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