using UnityEngine;

public class MainPanel : Panel
{
    #region Variables
    private static MainPanel m_Instance = null;

    [SerializeField]
    private CanvasGroup m_ButtonsCanvasGroup = null;

    [SerializeField]
    private ButtonList m_ButtonList = null;

    #endregion   

    #region Interface
    
    #endregion

    #region Private
    private void InitButtons()
    {
        
    }

    private void Attack()
    {

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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            m_ButtonList.Action();
        }
    }
    #endregion
}